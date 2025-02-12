using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Equipamento
{
    public class EquipamentoDTO
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // Categoria do equipamento
        public EquipamentoCategoria Categoria { get; set; }

        // Id da solicitação
        public int SolicitacaoId { get; set; }

        // Marca do equipamento
        public string? Marca { get; set; }

        // Modelo do equipamento
        public string? Modelo { get; set; }

        // Estagios que o equipamento passo. Exemplo: Inicio do diagnostico;Conclusão do diagnostico; Inicio da montagem; Finalização da montagem;
        public string Estagios { get; set; }
    }
}
