using Repara.DTO;
using Repara.DTO.Cliente;
using Repara.DTO.Peca;
using Repara.DTO.PecaPedido;
using Repara.Model;

namespace DAL.Repositories.Contracts;

public interface IPecaPedidoRepository: IRepositoryBase<PecaPedido>
{
    PagedList<PecaPedido> GetAllPaged(PecaPedidoFilterParameters parameters);
}