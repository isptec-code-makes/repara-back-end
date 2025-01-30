using Repara.DTO;
using Repara.DTO.Cliente;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IClienteRepository: IRepositoryBase<Cliente>
{
    PagedList<Cliente> GetAllPaged(ClienteFilterParameters parameters);
}