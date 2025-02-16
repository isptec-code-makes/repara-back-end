using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Diagnostico;
using Newtonsoft.Json;

namespace Repara.API.Controllers
{
    [Route("api/diagnosticos")]
    [ApiController]
    public class DiagnosticoController : ControllerBase
    {
        private readonly IDiagnosticoService _diagnosticoService;
        private readonly ILogger<DiagnosticoController> _logger;

        public DiagnosticoController(IDiagnosticoService diagnosticoService, ILogger<DiagnosticoController> logger)
        {
            _diagnosticoService = diagnosticoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiagnosticos([FromQuery] DiagnosticoFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var diagnosticos = _diagnosticoService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    diagnosticos.TotalCount,
                    diagnosticos.PageSize,
                    diagnosticos.CurrentPage,
                    diagnosticos.TotalPages,
                    diagnosticos.HasNext,
                    diagnosticos.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(diagnosticos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de diagnosticos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiagnosticoById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var diagnostico = await _diagnosticoService.GetByIdAsync(id);
                if (diagnostico == null)
                {
                    return NotFound();
                }
                return Ok(diagnostico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o diagnostico com ID {DiagnosticoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiagnostico([FromBody] DiagnosticoCreateDTO diagnosticoCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (diagnosticoCreateDto == null)
                {
                    return BadRequest();
                }

                var createdDiagnostico = await _diagnosticoService.CreateAsync(diagnosticoCreateDto);
                if (createdDiagnostico == null)
                {
                    return BadRequest("Erro ao criar o diagnostico. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetDiagnosticoById), new { id = createdDiagnostico.Id }, createdDiagnostico);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo diagnostico.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiagnostico(int id, [FromBody] DiagnosticoUpdateDTO diagnosticoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (diagnosticoUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedDiagnostico = await _diagnosticoService.UpdateAsync(id, diagnosticoUpdateDto);

                if (updatedDiagnostico == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o diagnostico com ID {DiagnosticoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiagnostico(int id)
        {
            try
            {
                var diagnostico = await _diagnosticoService.GetByIdAsync(id);
                if (diagnostico == null)
                {
                    return NotFound();
                }

                await _diagnosticoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o diagnostico com ID {DiagnosticoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
