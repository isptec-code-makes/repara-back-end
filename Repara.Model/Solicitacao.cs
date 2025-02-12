using System.ComponentModel.DataAnnotations.Schema;
using Repara.Model.Enum;

namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Classe abstreai a solicitação de serviços a empresa
[Table("Solicitacoes")]
public class Solicitacao : TableBase
{

    // Id do cliente que faz a solicitação
    public int ClienteId { get; set; }

    // Cliente que faz a solicitação
    public Cliente Cliente { get; set; }

    // Prioridade da solicitação
    public SolicitacaoPrioridade Prioridade { get; set; }

    // Id do funcionário que recebeu a solicitação
    public int FuncionarioId { get; set; }

    // Funcionario que recebeu a solicitação
    public Funcionario Funcionario { get; set; }

    // Estado actual da solicitação
    public SolicitacaoEstado Estado { get; set; } = SolicitacaoEstado.Recebido;

    // Descrição inicial do problema
    public string DescricaoProblema { get; set; }

    // Data que os equipamentos foram entregues ao cliente
    public DateTime? DataEntrega { get; set; } = null;

    // Preço final a ser pago pelo cliente
    public decimal? Preco { get; set; } = null;

    // Estadios que o processo levou
    public string Estagios { get; set; } = string.Empty;

    // Coleção de equipamentos a serem processados
    public List<Equipamento> Equipamentos { get; set; } = [];


}