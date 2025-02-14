using System.Linq.Expressions;
using DAL.Repositories.Contracts;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Repara.DTO;
using Repara.DTO.Solicitacao;
using Repara.Helpers;
using Repara.Model;

namespace Repara.DAL.Repositories;

public class SolicitacaoRepository : RepositoryBase<Solicitacao>, ISolicitacaoRepository
{
    public SolicitacaoRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public PagedList<Solicitacao> GetAllPaged(SolicitacaoFilterParameters parameters)
    {
        var queryable = FindByCondition(BuildWhereClause(parameters)).OrderByField(parameters.SortBy, parameters.IsDecsending);
        return PagedList<Solicitacao>.ToPagedList(queryable, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Solicitacao?> GetByServico(Montagem servico)
    {
        return await FindByCondition(c => c.Equipamentos.Any(t => t.Id == servico.EquipamentoId)).FirstOrDefaultAsync();
    }

    public async Task<Solicitacao?> GetByServico(Diagnostico servico)
    {
        return await FindByCondition(c => c.Equipamentos.Any(t => t.Id == servico.EquipamentoId)).FirstOrDefaultAsync();
    }

    public async Task LoadEquipamentos(Solicitacao solicitacao)
    {
        solicitacao.Equipamentos = await FindByCondition(c => c.Id == solicitacao.Id).Include(c => c.Equipamentos).Select(c => c.Equipamentos).FirstOrDefaultAsync() ?? [];
    }


    private Expression<Func<Solicitacao, bool>> BuildWhereClause(SolicitacaoFilterParameters filter)
    {
        var predicate = PredicateBuilder.New<Solicitacao>(true);


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
            var searchPredicate = PredicateBuilder.New<Solicitacao>(false);
            /*
            searchPredicate = searchPredicate.Or(c => c.Nome.ToLower().Contains(searchTerm));
            */
            predicate = predicate.And(searchPredicate);
        }

        return predicate;
    }
}