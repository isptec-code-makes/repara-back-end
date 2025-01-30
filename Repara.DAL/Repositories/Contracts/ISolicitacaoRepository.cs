using Repara.DTO;
using Repara.DTO.Solicitacao;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface ISolicitacaoRepository: IRepositoryBase<Solicitacao>
{
    PagedList<Solicitacao> GetAllPaged(SolicitacaoFilterParameters parameters);
}