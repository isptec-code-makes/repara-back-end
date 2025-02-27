using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Repara.DTO;
using Repara.DTO.Funcionario;
using Repara.Helpers;
using Repara.Model;

namespace Repara.DAL.Repositories;

public class FuncionarioRepository : RepositoryBase<Funcionario>, IFuncionarioRepository
{
    public FuncionarioRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public PagedList<Funcionario> GetAllPaged(FuncionarioFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters))
            .OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Funcionario>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Funcionario?> GetFreeFuncionario(string especialidade)
    {
        return await FindByCondition(c => c.Especialidades.Contains(especialidade) && !c.Ocupado)
            .FirstOrDefaultAsync();
    }

    public async Task LoadMontagensAsync(Funcionario funcionario)
    {
        await Entity()
             .Entry(funcionario)
             .Collection(c => c.Montagens)
             .LoadAsync();
    }

    public async Task LoadDiagnosticosAsync(Funcionario funcionario)
    {
        await Entity()
            .Entry(funcionario)
            .Collection(c => c.Diagnosticos)
            .LoadAsync();
    }


    private Expression<Func<Funcionario, bool>> BuildWhereClause(FuncionarioFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Funcionario>(true);


        if (filter.CreatedOn.HasValue)
            predicate = predicate.And(
                c => c.CreatedOn.Date == filter.CreatedOn.Value.ToDateTime(TimeOnly.MinValue).Date);

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
            var searchPredicate = PredicateBuilder.New<Funcionario>(false);
            /*
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            */
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}