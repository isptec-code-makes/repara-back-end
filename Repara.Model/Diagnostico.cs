namespace Repara.Model;

// TODO: Adicionar os data anotations
// Classe abstrai um diagnostico, que é realizado sobre um equipamento
public class Diagnostico: Servico
{
    // o id do equipamento que é diagnosticado
    public int EquipamentoId { get; set; }
    
    // o equipamento a ser diagnosticado
    public Equipamento Equipamento { get; set; }
}