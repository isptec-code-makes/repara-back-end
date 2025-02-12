using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Diagnostico
{
    public class DiagnosticoUpdateDTO
    {

        public string? Relatorio { get; set; }

        public int? FuncionarioId { get; set; }

        public ServicoEstado? Estado { get; set; }
    }
}
