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
        private readonly IMapper _mapper;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository, IMapper mapper)
        {
            _funcionarioRepository = funcionarioRepository;
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
