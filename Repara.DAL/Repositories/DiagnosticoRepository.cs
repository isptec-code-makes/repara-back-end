using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Repara.DTO;
using Repara.DTO.Diagnostico;
using Repara.Helpers;
using Repara.Model;
using Repara.Model.Enum;
using Microsoft.EntityFrameworkCore;

namespace Repara.DAL.Repositories;

public class DiagnosticoRepository : RepositoryBase<Diagnostico>, IDiagnosticoRepository
{
    public DiagnosticoRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public PagedList<Diagnostico> GetAllPaged(DiagnosticoFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters))
            .OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Diagnostico>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Diagnostico?> GetDiagnosticoPorPrioridadeAsync()
    {
        return await FindByCondition(c => c.Estado == ServicoEstado.Pendente && c.FuncionarioId == null)
            .OrderBy(c => c.Equipamento.Solicitacao.Prioridade)
            .ThenBy(c => c.Equipamento.Solicitacao.CreatedOn)
            .FirstOrDefaultAsync();
    }

    public async Task LoadFuncionario(Diagnostico diagnostico)
    {
        await Entity()
           .Entry(diagnostico)
           .Reference(e => e.Funcionario)
           .LoadAsync();
    }

    public async Task<(long, long)> GetMinMaxMontagemTimeAsync()
    {
        var min = await FindByCondition(c => c.DateEnd != null && c.DateInit != null && c.Estado == ServicoEstado.Terminado).MinAsync(c => (c.DateEnd!.Value - c.DateInit!.Value).Ticks);
        var max = await FindByCondition(c => c.DateEnd != null && c.DateInit != null && c.Estado == ServicoEstado.Terminado).MaxAsync(c => (c.DateEnd!.Value - c.DateInit!.Value).Ticks);
        return (min, max);
    }

    private Expression<Func<Diagnostico, bool>> BuildWhereClause(DiagnosticoFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Diagnostico>(true);


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
            var searchPredicate = PredicateBuilder.New<Diagnostico>(false);
            /*
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            */
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}