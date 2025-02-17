using Repara.DTO;
using Repara.DTO.Montagem;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IMontagemRepository : IRepositoryBase<Montagem>
{
    PagedList<Montagem> GetAllPaged(MontagemFilterParameters parameters);

    Task<Montagem?> GetDiagnosticoPorPrioridadeAsync();

    Task<ICollection<Montagem>> GetAllBySolicitacaoAsync(Solicitacao solicitacao);

    Task<(long, long)> GetMinMaxMontagemTimeAsync();

    Task LoadPeca(Montagem montagem);
}