using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Repara.DTO;
using Repara.DTO.PecaPedido;
using Repara.Helpers;
using Repara.Model;

namespace DAL.Repositories;

public class PecaPedidoRepository: RepositoryBase<PecaPedido>, IPecaPedidoRepository
{
    public PecaPedidoRepository(AppDbContext appDbContext) : base(appDbContext)  {}

    public PagedList<PecaPedido> GetAllPaged(PecaPedidoFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters)).OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<PecaPedido>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize); 
    }
    
    private Expression<Func<PecaPedido, bool>> BuildWhereClause(PecaPedidoFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<PecaPedido>(true);

        
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
            var searchPredicate = PredicateBuilder.New<PecaPedido>(false);
            /*
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            */
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}