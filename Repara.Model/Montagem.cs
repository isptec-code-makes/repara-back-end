using System.ComponentModel.DataAnnotations.Schema;

namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Classe abstrai um serviço de motagens de peças em um equipamento
[Table("Montagens")]
public class Montagem : Servico
{
    // Coleção de peças a serem motada
    public ICollection<PecaPedido> Pecas { get; set; } = new List<PecaPedido>();

    // o id do equipamento que é montado
    public int EquipamentoId { get; set; }

    // o equipamento que é montado
    public Equipamento Equipamento { get; set; }
}