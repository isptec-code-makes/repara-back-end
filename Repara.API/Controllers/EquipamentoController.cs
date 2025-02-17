using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Equipamento;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;
using Repara.DTO.Montagem;

namespace Repara.API.Controllers
{
    [Route("api/equipamentos")]
    [ApiController]
    public class EquipamentoController : ControllerBase
    {
        private readonly IEquipamentoService _equipamentoService;
        private readonly IMontagemService _montagemService;
        private readonly IDiagnosticoService _diagnosticoService;
        private readonly ILogger<EquipamentoController> _logger;

        public EquipamentoController(
            IEquipamentoService equipamentoService,
            ILogger<EquipamentoController> logger,
            IMontagemService montagemService,
            IDiagnosticoService diagnosticoService)
        {
            _equipamentoService = equipamentoService;
            _logger = logger;
            _montagemService = montagemService;
            _diagnosticoService = diagnosticoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquipamento([FromQuery] EquipamentoFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var equipamentos = _equipamentoService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    equipamentos.TotalCount,
                    equipamentos.PageSize,
                    equipamentos.CurrentPage,
                    equipamentos.TotalPages,
                    equipamentos.HasNext,
                    equipamentos.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(equipamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de equipamentos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquipamentoById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var equipamento = await _equipamentoService.GetByIdAsync(id);
                if (equipamento == null)
                {
                    return NotFound();
                }
                return Ok(equipamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o equipamento com ID {EquipamentoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}/estatiscas")]
        public async Task<IActionResult> GetEstatistica(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var equipamento = await _equipamentoService.GetEstatisticaAsync(id);
                if (equipamento == null)
                {
                    return NotFound();
                }
                return Ok(equipamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o equipamento com ID {EquipamentoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        // Retorna a lista de montagems de um equipamento
        [HttpGet("{id:int}/montagens")]
        public async Task<IActionResult> GetAllMontagem(int id, [FromQuery] MontagemFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                filterParameters.EquipamentoId = id;

                var montagens = _montagemService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    montagens.TotalCount,
                    montagens.PageSize,
                    montagens.CurrentPage,
                    montagens.TotalPages,
                    montagens.HasNext,
                    montagens.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(montagens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de montagems.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        // Retorna a lista de diagnósticos de um equipamento
        [HttpGet("{id:int}/diagnostico")]
        public async Task<IActionResult> GetDiagnostico(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var diagnostico = await _equipamentoService.GetDiagnosticoAsync(id);
                if (diagnostico == null)
                {
                    return NotFound();
                }


                return Ok(diagnostico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter diagnostico.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEquipamento([FromBody] EquipamentoCreateDTO equipamentoCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (equipamentoCreateDto == null)
                {
                    return BadRequest();
                }

                var createdEquipamento = await _equipamentoService.CreateAsync(equipamentoCreateDto);
                if (createdEquipamento == null)
                {
                    return BadRequest("Erro ao criar o equipamento. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetEquipamentoById), new { id = createdEquipamento.Id }, createdEquipamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo equipamento.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipamento(int id, [FromBody] EquipamentoUpdateDTO equipamentoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (equipamentoUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedEquipamento = await _equipamentoService.UpdateAsync(id, equipamentoUpdateDto);

                if (updatedEquipamento == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o equipamento com ID {EquipamentoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipamento(int id)
        {
            try
            {
                var equipamento = await _equipamentoService.GetByIdAsync(id);
                if (equipamento == null)
                {
                    return NotFound();
                }

                await _equipamentoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o equipamento com ID {EquipamentoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
