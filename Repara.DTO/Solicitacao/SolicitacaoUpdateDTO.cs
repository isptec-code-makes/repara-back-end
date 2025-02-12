using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Solicitacao
{
    public class SolicitacaoUpdateDTO
    {

        // Prioridade da solicitação
        public SolicitacaoPrioridade? Prioridade { get; set; }

        // Estado actual da solicitação
        public SolicitacaoEstado? Estado { get; set; }

    }
}
