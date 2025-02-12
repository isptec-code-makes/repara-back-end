using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

// TODO: Adicionar os dataanotations para validadar os parametros da rquisição

namespace Repara.DTO.Peca
{
    public class PecaCreateDTO
    {

        // Nome da peça
        [Required]
        public string Designacao { get; set; }

        // o preço que deve ser pago para a peça
        [Required]
        public decimal Preco { get; set; }

        // Modelo da peça
        [Required]
        public string Modelo { get; set; }

        // A marca do fabricante da peça
        [Required]
        public string Marca { get; set; }

        // Categoria da peça
        [Required]
        public EquipamentoCategoria Categoria { get; set; }

        // quando de peças existentes no estoque da empresa
        [Required]
        public int Estoque { get; set; }
    }
}
