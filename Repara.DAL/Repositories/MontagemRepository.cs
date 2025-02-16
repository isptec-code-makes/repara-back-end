using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Repara.DTO;
using Repara.DTO.Montagem;
using Repara.Helpers;
using Repara.Model;
using Repara.Model.Enum;
namespace Repara.DAL.Repositories;

public class MontagemRepository : RepositoryBase<Montagem>, IMontagemRepository
{
    public MontagemRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public PagedList<Montagem> GetAllPaged(MontagemFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters))
            .OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Montagem>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Montagem?> GetDiagnosticoPorPrioridadeAsync()
    {
        return await FindByCondition(c => c.Estado == ServicoEstado.Pendente && c.FuncionarioId == null)
            .OrderBy(c => c.Equipamento.Solicitacao.Prioridade)
            .ThenBy(c => c.Equipamento.Solicitacao.CreatedOn)
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Montagem>> GetAllBySolicitacaoAsync(Solicitacao solicitacao)
    {
        return await FindByCondition(c => c.Equipamento.Solicitacao.Id == solicitacao.Id).ToListAsync();
    }

    public async Task<(long, long)> GetMinMaxMontagemTimeAsync()
    {
        var min = await FindByCondition(c => c.DateEnd != null && c.DateInit != null && c.Estado == ServicoEstado.Terminado).MinAsync(c => (c.DateEnd!.Value - c.DateInit!.Value).Ticks);
        var max = await FindByCondition(c => c.DateEnd != null && c.DateInit != null && c.Estado == ServicoEstado.Terminado).MaxAsync(c => (c.DateEnd!.Value - c.DateInit!.Value).Ticks);
        return (min, max);
    }

    private Expression<Func<Montagem, bool>> BuildWhereClause(MontagemFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Montagem>(true);


        if (filter.CreatedOn.HasValue)
            predicate = predicate.And(
                c => c.CreatedOn.Date == filter.CreatedOn.Value.ToDateTime(TimeOnly.MinValue).Date);


        if (filter.EquipamentoId.HasValue)
        {
            predicate = predicate.And(c => c.EquipamentoId == filter.EquipamentoId);
        }



        // Filtros do Search
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchTerm = filter.Search.Trim().ToLower();
            var searchPredicate = PredicateBuilder.New<Montagem>(false);
            /*
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            */
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}