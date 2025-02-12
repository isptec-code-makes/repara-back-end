using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Peca
{
    public class PecaUpdateDTO
    {
        // Nome da peça
        public string? Designacao { get; set; }

        // o preço que deve ser pago para a peça
        public decimal? Preco { get; set; }

        // Modelo da peça
        public string? Modelo { get; set; }

        // A marca do fabricante da peça
        public string? Marca { get; set; }

        // Categoria da peça
        public EquipamentoCategoria? Categoria { get; set; }
    }
}
