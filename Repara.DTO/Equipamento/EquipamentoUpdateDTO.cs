using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

namespace Repara.DTO.Equipamento
{
    public class EquipamentoUpdateDTO
    {

        // Categoria do equipamento
        public EquipamentoCategoria? Categoria { get; set; }

        // Marca do equipamento
        public string? Marca { get; set; }

        // Modelo do equipamento
        public string? Modelo { get; set; }
    }
}
