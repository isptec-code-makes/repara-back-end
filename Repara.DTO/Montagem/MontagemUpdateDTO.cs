using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Montagem
{
    public class MontagemUpdateDTO
    {

        // Relatorio do Serviço criado pelo funcionario
        public string? Relatorio { get; set; }

        // Id do funcionario
        public int? FuncionarioId { get; set; }

        public ServicoEstado? Estado { get; set; }

    }
}
