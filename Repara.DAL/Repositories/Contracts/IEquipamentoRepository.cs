using Repara.DTO;
using Repara.DTO.Equipamento;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IEquipamentoRepository : IRepositoryBase<Equipamento>
{
    PagedList<Equipamento> GetAllPaged(EquipamentoFilterParameters parameters);
}