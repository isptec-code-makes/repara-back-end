using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

// TODO: Adicionar os dataanotations para validadar os parametros da rquisição

namespace Repara.DTO.Diagnostico
{
    public class DiagnosticoCreateDTO
    {

        public int EquipamentoId { get; set; }

        public int FuncionarioId { get; set; }

    }
}
