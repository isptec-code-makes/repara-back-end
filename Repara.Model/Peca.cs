using System.ComponentModel.DataAnnotations.Schema;
using Repara.Model.Enum;

namespace Repara.Model;

// Classe representa uma peça de equipamentos
// TODO: Adicionar os dataanotations
[Table("Pecas")]
public class Peca : TableBase
{

    // Nome da peça
    public string Designacao { get; set; }

    // o preço que deve ser pago para a peça
    public decimal Preco { get; set; }

    // Modelo da peça
    public string Modelo { get; set; }

    // A marca do fabricante da peça
    public string Marca { get; set; }

    // Categoria da peça
    public EquipamentoCategoria Categoria { get; set; }

    // quando de peças existentes no estoque da empresa
    public int Estoque { get; set; }
}