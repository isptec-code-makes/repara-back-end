using Repara.DTO.Diagnostico;
using Repara.DTO.Equipamento;

namespace Repara.Services.Contracts;

public interface IEquipamentoService : IServiceBase<EquipamentoDTO, EquipamentoFilterParameters, EquipamentoCreateDTO, EquipamentoUpdateDTO>
{
    Task<DiagnosticoDTO?> GetDiagnosticoAsync(int id);

}