using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Cliente;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;

namespace Repara.API.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IClienteService clienteService, ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientes([FromQuery] ClienteFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var clientes = _clienteService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    clientes.TotalCount,
                    clientes.PageSize,
                    clientes.CurrentPage,
                    clientes.TotalPages,
                    clientes.HasNext,
                    clientes.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de clientes.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClienteById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cliente = await _clienteService.GetByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o cliente com ID {ClienteId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteCreateDTO clienteCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (clienteCreateDto == null)
                {
                    return BadRequest();
                }

                var createdCliente = await _clienteService.CreateAsync(clienteCreateDto);
                if (createdCliente == null)
                {
                    return BadRequest("Erro ao criar o cliente. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetClienteById), new { id = createdCliente.Id }, createdCliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo cliente.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDTO clienteUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (clienteUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedCliente = await _clienteService.UpdateAsync(id, clienteUpdateDto);

                if (updatedCliente == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o cliente com ID {ClienteId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _clienteService.GetByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                await _clienteService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o cliente com ID {ClienteId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
