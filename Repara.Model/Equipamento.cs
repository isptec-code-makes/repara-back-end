using Repara.Model.Enum;

namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Classe que abstrai um equipamento
public class Equipamento: TableBase
{

    // Categoria do equipamento
    public EquipamentoCategoria Categoria { get; set; }
    
    // Id da solicitação
    public int SolicitacaoId { get; set; }
    
    // Solitação que o equipamento pertence
    public Solicitacao Solicitacao { get; set; }
    
    // Marca do equipamento
    public string? Marca { get; set; }
    
    // Modelo do equipamento
    public string? Modelo { get; set; }

    // Diagnostico realizado sobre o equipamento
    public Diagnostico? Diagnostico { get; set; }

    // Montagem de peças realizadas sobre o equipamento
    public Montagem? Montagem { get; set; }

    // Estagios que o equipamento passo. Exemplo: Inicio do diagnostico;Conclusão do diagnostico; Inicio da montagem; Finalização da montagem;
    public ICollection<string> Estagios = new List<string>();
}