using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Diagnostico;
using Repara.DTO.Equipamento;
using Repara.Model;
using Repara.Model.Enum;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class EquipamentoService : IEquipamentoService
    {
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly ISolicitacaoRepository _solicitacaoRepository;
        private readonly IDiagnosticoRepository _diagnosticoRepository;

        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IMapper _mapper;

        public EquipamentoService(IEquipamentoRepository equipamentoRepository, ISolicitacaoRepository solicitacaoRepository, IDiagnosticoRepository diagnosticoRepository, IMapper mapper, IFuncionarioRepository funcionarioRepository)
        {
            _equipamentoRepository = equipamentoRepository;
            _solicitacaoRepository = solicitacaoRepository;
            _diagnosticoRepository = diagnosticoRepository;
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
        }

        public PagedList<EquipamentoDTO> GetAllPaged(EquipamentoFilterParameters parameters)
        {
            var equipamentos = _equipamentoRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<EquipamentoDTO>>(equipamentos);
        }

        public async Task<EquipamentoDTO?> GetByIdAsync(int id)
        {
            var equipamento = await _equipamentoRepository.GetByIdAsync(id);
            if (equipamento is null) return null;

            return _mapper.Map<EquipamentoDTO>(equipamento);
        }

        public async Task<DiagnosticoDTO?> GetDiagnosticoAsync(int id)
        {
            var equipamento = await _equipamentoRepository.GetByIdAsync(id, tracking: false);
            if (equipamento is null) return null;

            await _equipamentoRepository.LoadDiagnostico(equipamento);

            if (equipamento.Diagnostico is null) return null;

            return _mapper.Map<DiagnosticoDTO>(equipamento.Diagnostico);
        }

        public async Task<EquipamentoEstatisticaDTO?> GetEstatisticaAsync(int id)
        {
            var equipamento = await _equipamentoRepository.GetByIdAsync(id, tracking: false);
            if (equipamento is null) return null;

            await _equipamentoRepository.LoadDiagnostico(equipamento);
            await _equipamentoRepository.LoadMontagens(equipamento);

            var diagnostico = equipamento.Diagnostico;
            var montagens = equipamento.Montagens;

            EquipamentoEstatisticaDTO estatisticaDTO = new EquipamentoEstatisticaDTO
            {
                Estado = ServicoEstado.Pendente
            };

            if (diagnostico != null && diagnostico.Estado == ServicoEstado.Iniciado)
            {
                estatisticaDTO.Estado = ServicoEstado.Iniciado;
            }

            if (diagnostico != null && diagnostico.Estado == ServicoEstado.Cancelado)
            {
                estatisticaDTO.Estado = ServicoEstado.Cancelado;
                return estatisticaDTO;
            }

            if (montagens != null && montagens.All(m => m.Estado == ServicoEstado.Terminado || m.Estado == ServicoEstado.Cancelado))
            {
                estatisticaDTO.Estado = ServicoEstado.Terminado;
            }

            if (montagens != null && montagens.All(m => m.Estado == ServicoEstado.Cancelado))
            {
                estatisticaDTO.Estado = ServicoEstado.Cancelado;
            }


            return estatisticaDTO;
        }

        public async Task<EquipamentoDTO?> CreateAsync(EquipamentoCreateDTO request)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(request.SolicitacaoId);
            if (solicitacao is null)
            {
                throw new BadRequestException("Solicitação não encontrada");
            }

            var equipamento = _mapper.Map<Equipamento>(request);

            equipamento.Diagnostico = new Diagnostico
            {
                Especialidade = "diagnostico",
            };


            _equipamentoRepository.Add(equipamento);


            try
            {
                await _equipamentoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar equipamento", e);
            }

            await AtribuiFuncionario();

            return _mapper.Map<EquipamentoDTO>(equipamento);
        }

        public async Task<EquipamentoDTO?> UpdateAsync(int id, EquipamentoUpdateDTO request)
        {
            bool changed = false;

            var equipamento = await _equipamentoRepository.GetByIdAsync(id);
            if (equipamento is null) return null;

            if (request.Categoria.HasValue)
            {
                equipamento.Categoria = request.Categoria.Value;
                changed = true;
            }

            if (!string.IsNullOrEmpty(request.Marca))
            {
                equipamento.Marca = request.Marca;
                changed = true;
            }

            if (!string.IsNullOrEmpty(request.Modelo))
            {
                equipamento.Modelo = request.Modelo;
                changed = true;
            }

            _mapper.Map(request, equipamento);

            if (changed)
            {
                equipamento.UpdatedOn = DateTime.Now;
            }

            _equipamentoRepository.Update(equipamento);

            try
            {
                await _equipamentoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar equipamento", e);
            }

            if (changed)
            {
                // do something
            }

            return _mapper.Map<EquipamentoDTO>(equipamento);
        }

        public async Task DeleteAsync(int id)
        {
            var equipamento = await _equipamentoRepository.GetByIdAsync(id);
            if (equipamento is null)
            {
                throw new NotFoundException("Equipamento não encontrado");
            }

            _equipamentoRepository.Remove(equipamento);

            try
            {
                await _equipamentoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar equipamento", e);
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
