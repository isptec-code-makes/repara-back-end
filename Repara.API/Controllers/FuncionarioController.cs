using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Funcionario;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;

namespace Repara.API.Controllers
{
    [Route("api/funcionario")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly ILogger<FuncionarioController> _logger;

        public FuncionarioController(IFuncionarioService funcionarioService, ILogger<FuncionarioController> logger)
        {
            _funcionarioService = funcionarioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFuncionarios([FromQuery] FuncionarioFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var funcionarios = _funcionarioService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    funcionarios.TotalCount,
                    funcionarios.PageSize,
                    funcionarios.CurrentPage,
                    funcionarios.TotalPages,
                    funcionarios.HasNext,
                    funcionarios.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(funcionarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de funcionarios.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFuncionarioById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var funcionario = await _funcionarioService.GetByIdAsync(id);
                if (funcionario == null)
                {
                    return NotFound();
                }
                return Ok(funcionario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o funcionario com ID {FuncionarioId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFuncionario([FromBody] FuncionarioCreateDTO funcionarioCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (funcionarioCreateDto == null)
                {
                    return BadRequest();
                }

                var createdFuncionario = await _funcionarioService.CreateAsync(funcionarioCreateDto);
                if (createdFuncionario == null)
                {
                    return BadRequest("Erro ao criar o funcionario. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetFuncionarioById), new { id = createdFuncionario.Id }, createdFuncionario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo funcionario.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFuncionario(int id, [FromBody] FuncionarioUpdateDTO funcionarioUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (funcionarioUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedFuncionario = await _funcionarioService.UpdateAsync(id, funcionarioUpdateDto);

                if (updatedFuncionario == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o funcionario com ID {FuncionarioId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            try
            {
                var funcionario = await _funcionarioService.GetByIdAsync(id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                await _funcionarioService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o funcionario com ID {FuncionarioId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }
    }
}
