using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

// TODO: Adicionar os dataanotations para validadar os parametros da rquisição

namespace Repara.DTO.Equipamento
{
    public class EquipamentoCreateDTO
    {

        // Categoria do equipamento
        public EquipamentoCategoria Categoria { get; set; }

        // Id da solicitação
        public int SolicitacaoId { get; set; }

        // Marca do equipamento
        public string? Marca { get; set; }

        // Modelo do equipamento
        public string? Modelo { get; set; }

    }
}
