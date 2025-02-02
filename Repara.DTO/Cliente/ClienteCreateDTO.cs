using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Adicionar os dataanotations para validadar os parametros da rquisição

namespace Repara.DTO.Cliente
{
    public class ClienteCreateDTO
    {
        
        public string Nome { get; set; }

        public string? Endereco { get; set; }

        public string Telefone { get; set; }

        public string? Email { get; set; }
    }
}
