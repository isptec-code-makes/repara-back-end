using System.ComponentModel.DataAnnotations.Schema;

namespace Repara.Model;

// TODO: Adicionar os data anotations
// Classe abstrai um diagnostico, que é realizado sobre um equipamento
[Table("Diagnosticos")]
public class Diagnostico: Servico
{
    // o id do equipamento que é diagnosticado
    public int EquipamentoId { get; set; }
    
    // o equipamento a ser diagnosticado
    public Equipamento Equipamento { get; set; }
}