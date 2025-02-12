using Repara.DTO;
using Repara.DTO.Funcionario;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IFuncionarioRepository : IRepositoryBase<Funcionario>
{
    PagedList<Funcionario> GetAllPaged(FuncionarioFilterParameters parameters);

    Task<Funcionario?> GetFreeFuncionario(string especialidade);
}