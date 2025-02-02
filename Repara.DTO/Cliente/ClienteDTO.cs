using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repara.DTO.Cliente
{
    public class ClienteDTO
    {

        public int Id { get; set; }

        public string Nome { get; set; }

        public string? Endereco { get; set; }

        // Número de telefone do cliente
        public string? Telefone { get; set; }

        // Endereço de email do cliente
        public string? Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
