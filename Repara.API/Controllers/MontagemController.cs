using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Montagem;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;

namespace Repara.API.Controllers
{
    [Route("api/montagems")]
    [ApiController]
    public class MontagemController : ControllerBase
    {
        private readonly IMontagemService _montagemService;
        private readonly ILogger<MontagemController> _logger;

        public MontagemController(IMontagemService montagemService, ILogger<MontagemController> logger)
        {
            _montagemService = montagemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMontagems([FromQuery] MontagemFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var montagems = _montagemService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    montagems.TotalCount,
                    montagems.PageSize,
                    montagems.CurrentPage,
                    montagems.TotalPages,
                    montagems.HasNext,
                    montagems.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(montagems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de montagems.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMontagemById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var montagem = await _montagemService.GetByIdAsync(id);
                if (montagem == null)
                {
                    return NotFound();
                }
                return Ok(montagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o montagem com ID {MontagemId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMontagem([FromBody] MontagemCreateDTO montagemCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (montagemCreateDto == null)
                {
                    return BadRequest();
                }

                var createdMontagem = await _montagemService.CreateAsync(montagemCreateDto);
                if (createdMontagem == null)
                {
                    return BadRequest("Erro ao criar o montagem. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetMontagemById), new { id = createdMontagem.Id }, createdMontagem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo montagem.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMontagem(int id, [FromBody] MontagemUpdateDTO montagemUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (montagemUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedMontagem = await _montagemService.UpdateAsync(id, montagemUpdateDto);

                if (updatedMontagem == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o montagem com ID {MontagemId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMontagem(int id)
        {
            try
            {
                var montagem = await _montagemService.GetByIdAsync(id);
                if (montagem == null)
                {
                    return NotFound();
                }

                await _montagemService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o montagem com ID {MontagemId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
