using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.PecaPedido
{
    public class PecaPedidoUpdateDTO
    {

        // Estado do pedido da peça
        public PecaPedidoEstado? Estado { get; set; }
    }
}
