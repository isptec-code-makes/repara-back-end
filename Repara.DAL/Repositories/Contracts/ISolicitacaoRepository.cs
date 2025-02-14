using Repara.DTO;
using Repara.DTO.Solicitacao;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface ISolicitacaoRepository : IRepositoryBase<Solicitacao>
{
    PagedList<Solicitacao> GetAllPaged(SolicitacaoFilterParameters parameters);

    Task<Solicitacao?> GetByServico(Montagem servico);
    Task<Solicitacao?> GetByServico(Diagnostico servico);
    Task LoadEquipamentos(Solicitacao solicitacao);
}