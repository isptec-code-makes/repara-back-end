using System.ComponentModel.DataAnnotations.Schema;
using Repara.Model.Enum;

namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Solicitação de peças ao departamento responsável
[Table("PecaPedidos")]
public class PecaPedido : TableBase
{

    private PecaPedidoEstado _estado = PecaPedidoEstado.Pendente;

    // preço do momento da solicitação da peça
    public decimal? Preco { get; set; } = 0;

    // Id da peça a ser solicitada
    public int PecaId { get; set; }

    // Peça a ser solicitada
    public Peca Peca { get; set; }

    // relaciona com a montagem
    public Montagem Montagem { get; set; }

    public int MontagemId { get; set; }


    // Data em que o pedido foi processado
    public DateTime? DateProcessed { get; set; } = null;

    // Estado do pedido da peça
    public PecaPedidoEstado Estado
    {
        get => _estado;
        set
        {
            // quando o estado do pedido mudar, é atribuido a data actual
            if (value != _estado)
            {
                DateProcessed = DateTime.Now;
            }
            _estado = value;
        }
    }
}