using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Repara.DTO;
using Repara.DTO.Peca;
using Repara.Helpers;
using Repara.Model;

namespace Repara.DAL.Repositories;

public class PecaRepository : RepositoryBase<Peca>, IPecaRepository
{
    public PecaRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public PagedList<Peca> GetAllPaged(PecaFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters)).OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Peca>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize);
    }

    private Expression<Func<Peca, bool>> BuildWhereClause(PecaFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Peca>(true);


        if (filter.CreatedOn.HasValue)
            predicate = predicate.And(c => c.CreatedOn.Date == filter.CreatedOn.Value.ToDateTime(TimeOnly.MinValue).Date);

        /*
        if (!string.IsNullOrWhiteSpace(filter.DataInicio) && !filter.CreationTime.HasValue && !filter.CreatedOn.HasValue)
        {
            var date = DateHelper.StringToDateOnly(filter.DataInicio).ToDateTime(TimeOnly.MinValue).Date ;
            predicate = predicate.And(c => c.CreatedOn.Date >= date);
        }
        
        */


        // Filtros do Search
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchTerm = filter.Search.Trim().ToLower();
            var searchPredicate = PredicateBuilder.New<Peca>(false);

            searchPredicate = searchPredicate.Or(c => c.Marca != null && c.Marca.ToLower().Contains(searchTerm));
            searchPredicate = searchPredicate.Or(c => c.Modelo != null && c.Modelo.ToLower().Contains(searchTerm));

            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}