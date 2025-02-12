using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

// TODO: Adicionar os dataanotations para validadar os parametros da rquisição

namespace Repara.DTO.Solicitacao
{
    public class SolicitacaoCreateDTO
    {

        // Id do cliente que faz a solicitação
        [Required]
        public int ClienteId { get; set; }

        // Prioridade da solicitação
        [Required]
        public SolicitacaoPrioridade Prioridade { get; set; }

        [Required]
        // Id do funcionário que recebeu a solicitação
        public int FuncionarioId { get; set; }

        // Descrição inicial do problema
        [Required]
        public string DescricaoProblema { get; set; }


    }
}
