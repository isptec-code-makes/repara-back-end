using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repara.Model.Enum;

// TODO: Adicionar os dataanotations para validadar os parametros da rquisição

namespace Repara.DTO.Montagem
{
    public class MontagemCreateDTO
    {

        // Id do funcionario
        public int? FuncionarioId { get; set; }

        // o id do equipamento que é montado
        public int EquipamentoId { get; set; }

        // Peça a ser montada
        public int PecaId { get; set; }
    }
}
