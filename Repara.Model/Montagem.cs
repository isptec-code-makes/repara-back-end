using System.ComponentModel.DataAnnotations.Schema;

namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Classe abstrai um serviço de motagens de peças em um equipamento
[Table("Montagens")]
public class Montagem : Servico
{
    // Peça a ser montada
    public PecaPedido? PecaPedido { get; set; }

    public Peca Peca { get; set; }

    public int PecaId { get; set; }


    // o id do equipamento que é montado
    public int EquipamentoId { get; set; }

    // o equipamento que é montado
    public Equipamento Equipamento { get; set; }
}