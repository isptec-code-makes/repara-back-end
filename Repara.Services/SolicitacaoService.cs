using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Solicitacao;
using Repara.Model;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class SolicitacaoService : ISolicitacaoService
    {
        private readonly ISolicitacaoRepository _solicitacaoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IMapper _mapper;

        public SolicitacaoService(
            ISolicitacaoRepository solicitacaoRepository,
            IClienteRepository clienteRepository,
            IFuncionarioRepository funcionarioRepository,
            IMapper mapper)
        {
            _solicitacaoRepository = solicitacaoRepository;
            _clienteRepository = clienteRepository;
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
        }

        public PagedList<SolicitacaoDTO> GetAllPaged(SolicitacaoFilterParameters parameters)
        {
            var solicitacaos = _solicitacaoRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<SolicitacaoDTO>>(solicitacaos);
        }

        public async Task<SolicitacaoDTO?> GetByIdAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao is null) return null;

            return _mapper.Map<SolicitacaoDTO>(solicitacao);
        }

        public async Task<SolicitacaoDTO?> CreateAsync(SolicitacaoCreateDTO request)
        {
            var cliente = await _clienteRepository.GetByIdAsync(request.ClienteId, true);
            if (cliente is null)
            {
                throw new BadRequestException("Cliente não encontrado");
            }

            var funcionario = await _funcionarioRepository.GetByIdAsync(request.FuncionarioId, true);
            if (funcionario is null)
            {
                throw new BadRequestException("funcionario não encontrado");
            }

            var solicitacao = _mapper.Map<Solicitacao>(request);

            solicitacao.Cliente = cliente;
            solicitacao.Funcionario = funcionario;

            _solicitacaoRepository.Add(solicitacao);

            try
            {
                await _solicitacaoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar Solicitacao", e);
            }

            return _mapper.Map<SolicitacaoDTO>(solicitacao);
        }

        public async Task<SolicitacaoDTO?> UpdateAsync(int id, SolicitacaoUpdateDTO request)
        {
            bool changed = false;

            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao is null) return null;

            if (request.Estado.HasValue && request.Estado.Value != solicitacao.Estado)
            {
                solicitacao.Estado = request.Estado.Value;
                changed = true;
            }

            if (request.Prioridade.HasValue && request.Prioridade.Value != solicitacao.Prioridade)
            {
                solicitacao.Prioridade = request.Prioridade.Value;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.DescricaoProblema))
            {
                solicitacao.DescricaoProblema = request.DescricaoProblema;
                changed = true;
            }

            if (changed)
            {
                solicitacao.UpdatedOn = DateTime.Now;
            }

            _solicitacaoRepository.Update(solicitacao);

            try
            {
                await _solicitacaoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar Solicitacao", e);
            }

            if (changed)
            {
                // fazer alguma coisa
            }


            return _mapper.Map<SolicitacaoDTO>(solicitacao);
        }

        public async Task DeleteAsync(int id)
        {
            var solicitacao = await _solicitacaoRepository.GetByIdAsync(id);
            if (solicitacao is null)
            {
                throw new NotFoundException("Solicitacao não encontrada");
            }

            _solicitacaoRepository.Remove(solicitacao);

            try
            {
                await _solicitacaoRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar Solicitacao", e);
            }
        }
    }
}
