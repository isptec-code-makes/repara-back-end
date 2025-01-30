using Repara.DTO;
using Repara.DTO.Cliente;
using Repara.DTO.Montagem;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IMontagemRepository: IRepositoryBase<Montagem>
{
    PagedList<Montagem> GetAllPaged(MontagemFilterParameters parameters);
}