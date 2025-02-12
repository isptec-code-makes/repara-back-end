using Repara.Model.Enum;


namespace Repara.DTO.Solicitacao
{
    public class SolicitacaoDTO
    {

        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        // Id do cliente que faz a solicitação
        public int ClienteId { get; set; }

        // Prioridade da solicitação
        public SolicitacaoPrioridade Prioridade { get; set; }

        // Id do funcionário que recebeu a solicitação
        public int FuncionarioId { get; set; }

        // Estado actual da solicitação
        public SolicitacaoEstado Estado { get; set; }

        // Descrição inicial do problema
        public string DescricaoProblema { get; set; }

        // Data que os equipamentos foram entregues ao cliente
        public DateTime DataEntrega { get; set; }

        // Preço final a ser pago pelo cliente
        public decimal Preco { get; set; }

        // Estadios que o processo levou
        public string Estagios { get; set; }
    }
}
