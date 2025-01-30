namespace Repara.Model;

// Classe representa as os principais atributos de uma tabela a ser representada na BD
public abstract class TableBase
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}