using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.PecaPedido;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;

namespace Repara.API.Controllers
{
    [Route("api/peca-pedidos")]
    [ApiController]
    public class PecaPedidoController : ControllerBase
    {
        private readonly IPecaPedidoService _pecaPedidoService;
        private readonly ILogger<PecaPedidoController> _logger;

        public PecaPedidoController(IPecaPedidoService pecaPedidoService, ILogger<PecaPedidoController> logger)
        {
            _pecaPedidoService = pecaPedidoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPecaPedidos([FromQuery] PecaPedidoFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var pecaPedidos = _pecaPedidoService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    pecaPedidos.TotalCount,
                    pecaPedidos.PageSize,
                    pecaPedidos.CurrentPage,
                    pecaPedidos.TotalPages,
                    pecaPedidos.HasNext,
                    pecaPedidos.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(pecaPedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de pecaPedidos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPecaPedidoById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var pecaPedido = await _pecaPedidoService.GetByIdAsync(id);
                if (pecaPedido == null)
                {
                    return NotFound();
                }
                return Ok(pecaPedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o pecaPedido com ID {PecaPedidoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePecaPedido([FromBody] PecaPedidoCreateDTO pecaPedidoCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (pecaPedidoCreateDto == null)
                {
                    return BadRequest();
                }

                var createdPecaPedido = await _pecaPedidoService.CreateAsync(pecaPedidoCreateDto);
                if (createdPecaPedido == null)
                {
                    return BadRequest("Erro ao criar o pecaPedido. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetPecaPedidoById), new { id = createdPecaPedido.Id }, createdPecaPedido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo pecaPedido.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePecaPedido(int id, [FromBody] PecaPedidoUpdateDTO pecaPedidoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (pecaPedidoUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedPecaPedido = await _pecaPedidoService.UpdateAsync(id, pecaPedidoUpdateDto);

                if (updatedPecaPedido == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o pecaPedido com ID {PecaPedidoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePecaPedido(int id)
        {
            try
            {
                var pecaPedido = await _pecaPedidoService.GetByIdAsync(id);
                if (pecaPedido == null)
                {
                    return NotFound();
                }

                await _pecaPedidoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o pecaPedido com ID {PecaPedidoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
