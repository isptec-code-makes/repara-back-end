using Repara.DTO;
using Repara.DTO.Diagnostico;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IDiagnosticoRepository : IRepositoryBase<Diagnostico>
{
    PagedList<Diagnostico> GetAllPaged(DiagnosticoFilterParameters parameters);

    Task<Diagnostico?> GetDiagnosticoPorPrioridadeAsync();
}