using Repara.DTO;
using Repara.DTO.Peca;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IPecaRepository : IRepositoryBase<Peca>
{
    PagedList<Peca> GetAllPaged(PecaFilterParameters parameters);
}