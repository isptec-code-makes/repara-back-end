using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Montagem;
using Repara.Model;
using Repara.Model.Enum;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class MontagemService : IMontagemService
    {
        private readonly IMontagemRepository _montagemRepository;
        private readonly IMapper _mapper;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IPecaRepository _pecaRepository;
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly ISolicitacaoRepository _solicitacaoRepository;

        public MontagemService(
            IMontagemRepository montagemRepository,
            IMapper mapper,
            IFuncionarioRepository funcionarioRepository,
            IPecaRepository pecaRepository,
            IEquipamentoRepository equipamentoRepository,
            ISolicitacaoRepository solicitacaoRepository)
        {
            _montagemRepository = montagemRepository;
            _mapper = mapper;
            _funcionarioRepository = funcionarioRepository;
            _pecaRepository = pecaRepository;
            _equipamentoRepository = equipamentoRepository;
            _solicitacaoRepository = solicitacaoRepository;
        }

        public PagedList<MontagemDTO> GetAllPaged(MontagemFilterParameters parameters)
        {
            var montagems = _montagemRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<MontagemDTO>>(montagems);
        }

        public async Task<MontagemDTO?> GetByIdAsync(int id)
        {
            var montagem = await _montagemRepository.GetByIdAsync(id);
            if (montagem is null) return null;

            return _mapper.Map<MontagemDTO>(montagem);
        }

        public async Task<MontagemDTO?> CreateAsync(MontagemCreateDTO request)
        {
            var peca = await _pecaRepository.GetByIdAsync(request.PecaId, tracking: true);
            if (peca is null)
            {
                throw new BadRequestException("Peça não encontrada");
            }

            var equipamento = await _equipamentoRepository.GetByIdAsync(request.EquipamentoId, tracking: true);
            if (equipamento is null)
            {
                throw new BadRequestException("Equipamento não encontrado");
            }

            var montagem = new Montagem()
            {
                Peca = peca,
                Equipamento = equipamento,
            };

            // se não existir em estoque, cria um pedido de peça
            if (peca.Estoque < 1)
            {
                montagem.PecaPedido = new PecaPedido()
                {
                    Peca = peca,
                    Montagem = montagem
                };
            }
            else
            {
                peca.Estoque--;
            }

            _montagemRepository.Add(montagem);

            try
            {
                await _montagemRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar montagem", e);
            }

            // dispara o trigger para atribuir um funcionario ao montagem
            await AtribuiFuncionario(montagem.Especialidade);

            return _mapper.Map<MontagemDTO>(montagem);
        }

        public async Task<MontagemDTO?> UpdateAsync(int id, MontagemUpdateDTO request)
        {
            bool changed = false;

            var montagem = await _montagemRepository.GetByIdAsync(id);
            if (montagem is null) return null;

            // copia a montagem para um objeto temporário
            var montagemTmp = _mapper.Map<Montagem>(montagem);

            if (request.FuncionarioId is not null)
            {
                var funcionario = await _funcionarioRepository.GetByIdAsync(request.FuncionarioId ?? 0, tracking: false);
                if (funcionario is null)
                {
                    throw new BadRequestException("Funcionario não encontrado");
                }

                montagem.Funcionario = funcionario;
                changed = true;
            }

            if (request.Estado is not null)
            {
                montagem.Estado = request.Estado.Value;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Relatorio))
            {
                montagem.Relatorio = request.Relatorio;
                changed = true;
            }

            if (changed)
            {
                montagem.UpdatedOn = DateTime.Now;

                if (montagem.Estado != montagemTmp.Estado)
                {
                    if (montagem.Estado is ServicoEstado.Cancelado or ServicoEstado.Terminado)
                    {
                        if (montagem.FuncionarioId is not null)
                        {
                            montagem.Funcionario = await _funcionarioRepository.GetByIdAsync(montagem.FuncionarioId ?? 0);
                            if (montagem.Funcionario is not null)
                            {
                                montagem.Funcionario.Ocupado = false;
                            }
                        }
                    }

                    if (montagem.Estado == ServicoEstado.Terminado)
                    {
                        await _montagemRepository.LoadPeca(montagem);
                        montagem.Peca.Estoque++;
                    }


                    await ActualizaSolicitacao(montagem);
                }
            }

            _montagemRepository.Update(montagem);

            try
            {
                await _montagemRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar montagem", e);
            }

            return _mapper.Map<MontagemDTO>(montagem);
        }

        public async Task DeleteAsync(int id)
        {
            var montagem = await _montagemRepository.GetByIdAsync(id);
            if (montagem is null)
            {
                throw new NotFoundException("Montagem não encontrado");
            }

            _montagemRepository.Remove(montagem);

            try
            {
                await _montagemRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar montagem", e);
            }
        }

        // método responsável por atribuir um funcionario a um diagnostico baseado na prioridade
        private async Task AtribuiFuncionario(string especialidade = "montagem")
        {
            var diagnostico = await _montagemRepository.GetDiagnosticoPorPrioridadeAsync();
            if (diagnostico is null) return;

            var funcionario = await _funcionarioRepository.GetFreeFuncionario(especialidade);
            if (funcionario is null) return;

            diagnostico.Funcionario = funcionario;
            diagnostico.UpdatedOn = DateTime.Now;

            funcionario.Ocupado = true;

            _montagemRepository.Update(diagnostico);

            try
            {
                await _montagemRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atribuir funcionario ao diagnostico", e);
            }
        }

        private async Task ActualizaSolicitacao(Montagem montagem)
        {
            var solicitacao = await _solicitacaoRepository.GetByServico(montagem);
            if (solicitacao is null) return;


            if (solicitacao.Estado == SolicitacaoEstado.Andamento)
            {
                await _solicitacaoRepository.LoadEquipamentos(solicitacao);

                var montagens = await _montagemRepository.GetAllBySolicitacaoAsync(solicitacao);

                var pendentes = montagens.Where(c => c.Estado == ServicoEstado.Pendente).ToList();

                if (pendentes.Count == 0)
                {
                    solicitacao.Estado = SolicitacaoEstado.Concluido;
                }

                solicitacao.UpdatedOn = DateTime.Now;
            }

            try
            {
                await _solicitacaoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar solicitacao", e);
            }
        }
    }
}
