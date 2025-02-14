using AutoMapper;
using DAL.Repositories.Contracts;
using Repara.DTO;
using Repara.DTO.Funcionario;
using Repara.Model;
using Repara.Services.Contracts;
using Repara.Shared.Exceptions;


namespace Repara.Services
{

    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IDiagnosticoRepository _diagnosticoRepository;
        private readonly IMontagemRepository _montagemRepository;
        private readonly IMapper _mapper;

        public FuncionarioService(
            IFuncionarioRepository funcionarioRepository,
            IDiagnosticoRepository diagnosticoRepository,
            IMontagemRepository montagemRepository,
            IMapper mapper)
        {
            _funcionarioRepository = funcionarioRepository;
            _diagnosticoRepository = diagnosticoRepository;
            _montagemRepository = montagemRepository;
            _mapper = mapper;
        }

        public PagedList<FuncionarioDTO> GetAllPaged(FuncionarioFilterParameters parameters)
        {
            var funcionarios = _funcionarioRepository.GetAllPaged(parameters);
            return _mapper.Map<PagedList<FuncionarioDTO>>(funcionarios);
        }

        public async Task<FuncionarioDTO?> GetByIdAsync(int id)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario is null) return null;

            return _mapper.Map<FuncionarioDTO>(funcionario);
        }

        public async Task<double> CalculaDesempenhoAsync(int id)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario is null)
            {
                throw new NotFoundException("Funcionario não encontrado");
            }

            List<Servico> servicos = new List<Servico>();

            await _funcionarioRepository.LoadDiagnosticosAsync(funcionario);
            await _funcionarioRepository.LoadMontagensAsync(funcionario);

            servicos.AddRange(funcionario.Diagnosticos.Where(c => c.Estado == Model.Enum.ServicoEstado.Terminado && c.DateEnd.HasValue && c.DateInit.HasValue));
            servicos.AddRange(funcionario.Montagens.Where(c => c.Estado == Model.Enum.ServicoEstado.Terminado && c.DateEnd.HasValue && c.DateInit.HasValue));

            if (servicos.Count > 0)
            {
                var totalTicks = servicos.Sum(s => (s.DateEnd.Value - s.DateInit.Value).Ticks);
                var tempo = totalTicks / servicos.Count;

                var (montagemMin, montagemMax) = await _montagemRepository.GetMinMaxMontagemTimeAsync();
                var (diagnosticoMin, diagnosticoMax) = await _diagnosticoRepository.GetMinMaxMontagemTimeAsync();

                var min = Math.Min(montagemMin, diagnosticoMin);
                var max = Math.Max(montagemMax, diagnosticoMax);

                if ((max - min) == 0) return 0;


                double desempenho = (max - tempo) / (max - min) * 100;

                return desempenho;

            }
            else
            {
                return 0;
            }
        }

        public async Task<FuncionarioDTO?> CreateAsync(FuncionarioCreateDTO request)
        {
            var exists = await _funcionarioRepository.AnyAsync(c => (c.Email != null && c.Email.Equals(request.Email)) || c.Telefone.Equals(request.Telefone));

            if (exists) return null;

            var funcionario = _mapper.Map<Funcionario>(request);
            funcionario.Especialidades = funcionario.Especialidades.ToLower();

            _funcionarioRepository.Add(funcionario);

            try
            {
                await _funcionarioRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar funcionario", e);
            }

            return _mapper.Map<FuncionarioDTO>(funcionario);
        }

        public async Task<FuncionarioDTO?> UpdateAsync(int id, FuncionarioUpdateDTO request)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario is null) return null;

            _mapper.Map(request, funcionario);

            _funcionarioRepository.Update(funcionario);

            try
            {
                await _funcionarioRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao atualizar funcionario", e);
            }

            return _mapper.Map<FuncionarioDTO>(funcionario);
        }

        public async Task DeleteAsync(int id)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            if (funcionario is null)
            {
                throw new NotFoundException("Funcionario não encontrado");
            }

            _funcionarioRepository.Remove(funcionario);

            try
            {
                await _funcionarioRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao deletar funcionario", e);
            }
        }
    }
}
