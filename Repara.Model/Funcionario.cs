namespace Repara.Model;

// Classe reprenseta um funcinario no sistema
//  TODO: Adicionar os dataanotations
public class Funcionario: TableBase
{
    // nome do funcionario
    public string Nome { get; set; }
    
    // endereço de emaio do funcionaro
    public string Email { get; set; }
    
    // número de telefone do funcionario
    public string Telefone { get; set; }
    
    // indica se o funcionario está disponivel para receber novos trabalhos
    public bool Disponibilidade { get; set; }
    
    // horios de trabalho que o funcionario está disponível para trabalho
    public string? HorarioTrabalho { get; set; }
    
    // Coleção de especioalidades do funcionario
    public string Especialidades { get; set; }

    // Coleção de solicitações do funcionario
    public ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();

    // Coleção de serviços de diagnosticos feitos pelo funcionaio
    public ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();

    // Coleção de Serviços de mostagem realizadas pelo funcionario
    public ICollection<Montagem> Montagens { get; set; } = new List<Montagem>();
}