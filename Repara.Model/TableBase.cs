using System.ComponentModel.DataAnnotations;

namespace Repara.Model;

// Classe representa as os principais atributos de uma tabela a ser representada na BD
public abstract class TableBase
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; }
}