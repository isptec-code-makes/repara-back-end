using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Diagnostico
{
    public class DiagnosticoDTO
    {

        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public int EquipamentoId { get; set; }

        // Data de inicialização do serviçço
        public DateTime? DateInit { get; set; }

        // Data de finalização do serviço
        public DateTime? DateEnd { get; set; }

        // Relatorio do Serviço criado pelo funcionario
        public string? Relatorio { get; set; }

        // Id do funcionario
        public int? FuncionarioId { get; set; }

        // TODO: Não mapear na BD
        public ServicoEstado Estado { get; set; }
    }
}
