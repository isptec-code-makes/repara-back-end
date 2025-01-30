namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Casse representa um cliente no sistema, aquele que solicita por um serviço
public class Cliente: TableBase
{

    // Nome completo do cliente
    public string Nome { get; set; }

    // Endereço de onde mora o cliente
    public string? Endereco { get; set; }

    // Número de telefone do cliente
    public string? Telefone { get; set; }

    // Endereço de email do cliente
    public string? Email { get; set; }

    // Coleção de Solicitações
    public ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();

}