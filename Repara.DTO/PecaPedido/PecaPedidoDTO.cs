using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.PecaPedido
{
    public class PecaPedidoDTO
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // preço do momento da solicitação da peça
        public decimal Preco { get; set; }

        // Id da peça a ser solicitada
        public int PecaId { get; set; }

        public int MontagemId { get; set; }

        // Data em que o pedido foi processado
        public DateTime DateProcessed { get; set; }

        // Estado do pedido da peça
        public PecaPedidoEstado Estado { get; set; }
    }
}
