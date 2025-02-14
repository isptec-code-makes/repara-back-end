using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Peca;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;

namespace Repara.API.Controllers
{
    [Route("api/pecas")]
    [ApiController]
    public class PecaController : ControllerBase
    {
        private readonly IPecaService _pecaService;
        private readonly ILogger<PecaController> _logger;

        public PecaController(IPecaService pecaService, ILogger<PecaController> logger)
        {
            _pecaService = pecaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPecas([FromQuery] PecaFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var pecas = _pecaService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    pecas.TotalCount,
                    pecas.PageSize,
                    pecas.CurrentPage,
                    pecas.TotalPages,
                    pecas.HasNext,
                    pecas.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(pecas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de pecas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPecaById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var peca = await _pecaService.GetByIdAsync(id);
                if (peca == null)
                {
                    return NotFound();
                }
                return Ok(peca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o peca com ID {PecaId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePeca([FromBody] PecaCreateDTO pecaCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (pecaCreateDto == null)
                {
                    return BadRequest();
                }

                var createdPeca = await _pecaService.CreateAsync(pecaCreateDto);
                if (createdPeca == null)
                {
                    return BadRequest("Erro ao criar o peca. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetPecaById), new { id = createdPeca.Id }, createdPeca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo peca.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePeca(int id, [FromBody] PecaUpdateDTO pecaUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (pecaUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedPeca = await _pecaService.UpdateAsync(id, pecaUpdateDto);

                if (updatedPeca == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o peca com ID {PecaId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeca(int id)
        {
            try
            {
                var peca = await _pecaService.GetByIdAsync(id);
                if (peca == null)
                {
                    return NotFound();
                }

                await _pecaService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o peca com ID {PecaId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
