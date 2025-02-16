using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Diagnostico;
using Repara.DTO.Equipamento;
using Repara.Model;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class EquipamentoService : IEquipamentoService
    {
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly ISolicitacaoRepository _solicitacaoRepository;
        private readonly IDiagnosticoRepository _diagnosticoRepository;
        private readonly IMapper _mapper;

        public EquipamentoService(IEquipamentoRepository equipamentoRepository, ISolicitacaoRepository solicitacaoRepository, IDiagnosticoRepository diagnosticoRepository, IMapper mapper)
        {
            _equipamentoRepository = equipamentoRepository;
            _solicitacaoRepository = solicitacaoRepository;
            _diagnosticoRepository = diagnosticoRepository;
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

        public async Task GetEstatistica(int id)
        {
            var equipamento = await _equipamentoRepository.GetByIdAsync(id, tracking: false);
            if (equipamento is null) return;

            await _equipamentoRepository.LoadDiagnostico(equipamento);
            await _equipamentoRepository.LoadMontagens(equipamento);




            if (equipamento.Diagnostico is null) return;

            // do something
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
    }
}
