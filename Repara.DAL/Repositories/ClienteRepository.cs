using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Repara.DTO;
using Repara.DTO.Cliente;
using Repara.Helpers;
using Repara.Model;

namespace DAL.Repositories;

public class ClienteRepository: RepositoryBase<Cliente>, IClienteRepository
{
    public ClienteRepository(AppDbContext appDbContext) : base(appDbContext)  {}

    public PagedList<Cliente> GetAllPaged(ClienteFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters)).OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Cliente>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize); 
    }
    
    private Expression<Func<Cliente, bool>> BuildWhereClause(ClienteFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Cliente>(true);

        
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
            // Aplicamos Trim e ToLower uma vez
            var searchTerm = filter.Search.Trim().ToLower();
            var searchPredicate = PredicateBuilder.New<Cliente>(false);
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}