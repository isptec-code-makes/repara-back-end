using System.ComponentModel.DataAnnotations.Schema;
using Repara.Model.Enum;

namespace Repara.Model;

// TODO: Adicionar os dataanotations
// Classe abstreai a solicitação de serviços a empresa
[Table("Enventos")]
public class Evento : TableBase
{
    public string Titulo { get; set; }
    public string Descricao { get; set; }
}