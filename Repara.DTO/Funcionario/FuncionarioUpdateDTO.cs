using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repara.DTO.Funcionario
{
    public class FuncionarioUpdateDTO
    {
        // nome do funcionario
        public string? Nome { get; set; }

        // endereço de email do funcionaro
        public string? Email { get; set; }

        // número de telefone do funcionario
        public string? Telefone { get; set; }

        // indica se o funcionario está disponivel para receber novos trabalhos
        public bool Ocupado { get; set; } = false;

        // horios de trabalho que o funcionario está disponível para trabalho
        public string? HorarioTrabalho { get; set; }

        // Coleção de especioalidades do funcionario
        public string? Especialidades { get; set; }
    }
}
