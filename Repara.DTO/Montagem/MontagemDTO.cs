using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Montagem
{
    public class MontagemDTO
    {

        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        // Data de inicialização do serviçço
        public DateTime? DateInit { get; set; } = DateTime.Now;

        // Data de finalização do serviço
        public DateTime? DateEnd { get; set; }

        // Relatorio do Serviço criado pelo funcionario
        public string? Relatorio { get; set; }

        // Id do funcionario
        public int? FuncionarioId { get; set; }

        // TODO: Não mapear na BD
        public ServicoEstado? Estado { get; set; }

        // o id do equipamento que é montado
        public int EquipamentoId { get; set; }

        public int? PecaPedidoId { get; set; }

        public int PecaId { get; set; }
    }
}
