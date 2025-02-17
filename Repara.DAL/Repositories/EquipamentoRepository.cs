using System.Data.Entity;
using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Repara.DTO;
using Repara.DTO.Equipamento;
using Repara.Helpers;
using Repara.Model;

namespace Repara.DAL.Repositories;

public class EquipamentoRepository : RepositoryBase<Equipamento>, IEquipamentoRepository
{
    public EquipamentoRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public PagedList<Equipamento> GetAllPaged(EquipamentoFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters))
            .OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Equipamento>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize);
    }

    // TODO: Implementar a forma correcta de carregar a montagem
    public async Task LoadMontagens(Equipamento equipamento)
    {
        await Entity()
           .Entry(equipamento)
           .Collection(e => e.Montagens)
           .LoadAsync();
    }

    public async Task LoadDiagnostico(Equipamento equipamento)
    {
        await Entity()
            .Entry(equipamento)
            .Reference(e => e.Diagnostico)
            .LoadAsync();
    }

    private Expression<Func<Equipamento, bool>> BuildWhereClause(EquipamentoFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Equipamento>(true);


        if (filter.CreatedOn.HasValue)
            predicate = predicate.And(
                c => c.CreatedOn.Date == filter.CreatedOn.Value.ToDateTime(TimeOnly.MinValue).Date);

        if (filter.SolicitacaoId.HasValue)
        {
            predicate = predicate.And(c => c.SolicitacaoId == filter.SolicitacaoId);
        }


        // Filtros do Search
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchTerm = filter.Search.Trim().ToLower();
            var searchPredicate = PredicateBuilder.New<Equipamento>(false);
            /*
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            */
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}