using AutoMapper;
using DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Repara.DTO;
using Repara.DTO.Diagnostico;
using Repara.Model;
using Repara.Model.Enum;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class DiagnosticoService : IDiagnosticoService
    {
        private readonly IDiagnosticoRepository _diagnosticoRepository;
        private readonly IMapper _mapper;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IPecaRepository _pecaRepository;
        private readonly IEquipamentoRepository _equipamentoRepository;

        private readonly ISolicitacaoRepository _solicitacaoRepository;


        public DiagnosticoService(
            IDiagnosticoRepository diagnosticoRepository,
            IMapper mapper,
            IFuncionarioRepository funcionarioRepository,
            IPecaRepository pecaRepository,
            IEquipamentoRepository equipamentoRepository,
            ISolicitacaoRepository solicitacaoRepository
            )
        {
            _diagnosticoRepository = diagnosticoRepository;
            _mapper = mapper;
            _funcionarioRepository = funcionarioRepository;
            _pecaRepository = pecaRepository;
            _equipamentoRepository = equipamentoRepository;
            _solicitacaoRepository = solicitacaoRepository;
        }

        public PagedList<DiagnosticoDTO> GetAllPaged(DiagnosticoFilterParameters parameters)
        {
            var diagnosticos = _diagnosticoRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<DiagnosticoDTO>>(diagnosticos);
        }

        public async Task<DiagnosticoDTO?> GetByIdAsync(int id)
        {
            var diagnostico = await _diagnosticoRepository.GetByIdAsync(id);
            if (diagnostico is null) return null;

            return _mapper.Map<DiagnosticoDTO>(diagnostico);
        }

        public async Task<DiagnosticoDTO?> CreateAsync(DiagnosticoCreateDTO request)
        {

            var equipamento = await _equipamentoRepository.GetByIdAsync(request.EquipamentoId, tracking: true);
            if (equipamento is null)
            {
                throw new BadRequestException("Equipamento não encontrado");
            }

            var diagnostico = new Diagnostico()
            {
                Equipamento = equipamento,
            };

            _diagnosticoRepository.Add(diagnostico);

            try
            {
                await _diagnosticoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar diagnostico", e);
            }

            // dispara o trigger para atribuir um funcionario ao diagnostico
            await AtribuiFuncionario(diagnostico.Especialidade);

            return _mapper.Map<DiagnosticoDTO>(diagnostico);
        }

        public async Task<DiagnosticoDTO?> UpdateAsync(int id, DiagnosticoUpdateDTO request)
        {

            bool changed = false;

            var diagnostico = await _diagnosticoRepository.GetByIdAsync(id);
            if (diagnostico is null) return null;

            // copia a diagnostico para um objeto temporário
            var diagnosticoTmp = _mapper.Map<Diagnostico>(diagnostico);

            if (request.FuncionarioId is not null)
            {
                var funcionario = await _funcionarioRepository.GetByIdAsync(request.FuncionarioId ?? 0, tracking: false);
                if (funcionario is null)
                {
                    throw new BadRequestException("Funcionario não encontrado");
                }

                diagnostico.Funcionario = funcionario;
                changed = true;
            }

            if (request.Estado is not null)
            {
                diagnostico.Estado = request.Estado.Value;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Relatorio))
            {
                diagnostico.Relatorio = request.Relatorio;
                changed = true;
            }


            if (changed)
            {
                diagnostico.UpdatedOn = DateTime.Now;

                if (diagnostico.Estado != diagnosticoTmp.Estado)
                {
                    if (diagnostico.Estado is ServicoEstado.Cancelado or ServicoEstado.Terminado)
                    {
                        if (diagnostico.FuncionarioId is not null)
                        {
                            diagnostico.Funcionario = await _funcionarioRepository.GetByIdAsync(diagnostico.FuncionarioId ?? 0);
                            if (diagnostico.Funcionario is not null)
                            {
                                diagnostico.Funcionario.Ocupado = false;
                            }
                        }
                    }
                }
            }

            _diagnosticoRepository.Update(diagnostico);

            try
            {
                await _diagnosticoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar diagnostico", e);
            }

            if (changed)
            {
                if (diagnostico.Estado != diagnosticoTmp.Estado)
                {
                    if (diagnostico.Estado is ServicoEstado.Cancelado or ServicoEstado.Terminado)
                    {
                        // dispara o trigger para atribuir um funcionario ao diagnostico
                        await AtribuiFuncionario(diagnostico.Especialidade);

                    }

                }

                if (diagnostico.Funcionario is not null)
                {
                    await ActualizaSolicitacao(diagnostico);
                }
            }


            return _mapper.Map<DiagnosticoDTO>(diagnostico);
        }

        public async Task DeleteAsync(int id)
        {
            var diagnostico = await _diagnosticoRepository.GetByIdAsync(id);
            if (diagnostico is null)
            {
                throw new NotFoundException("Diagnostico não encontrado");
            }

            _diagnosticoRepository.Remove(diagnostico);

            try
            {
                await _diagnosticoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar diagnostico", e);
            }
        }

        // método responsável por atribuir um funcionario a um diagnostico baseado na prioridade
        private async Task AtribuiFuncionario(string especialidade = "diagnostico")
        {
            var diagnostico = await _diagnosticoRepository.GetDiagnosticoPorPrioridadeAsync();
            if (diagnostico is null) return;

            var funcionario = await _funcionarioRepository.GetFreeFuncionario(especialidade);
            if (funcionario is null) return;

            diagnostico.Funcionario = funcionario;
            diagnostico.UpdatedOn = DateTime.Now;

            funcionario.Ocupado = true;

            await ActualizaSolicitacao(diagnostico);

            _diagnosticoRepository.Update(diagnostico);

            try
            {
                await _diagnosticoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atribuir funcionario ao diagnostico", e);

            }
        }

        private async Task ActualizaSolicitacao(Diagnostico diagnostico)
        {
            var solicitacao = await _solicitacaoRepository.GetByServico(diagnostico);
            if (solicitacao is null) return;

            if (solicitacao.Estado == SolicitacaoEstado.Pendente)
            {
                solicitacao.Estado = SolicitacaoEstado.Andamento;
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
