using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repara.Services.Contracts;
using Repara.DTO.Solicitacao;
using Repara.Shared.Exceptions;
using Newtonsoft.Json;
using Repara.DTO.Equipamento;

namespace Repara.API.Controllers
{
    [Route("api/solicitacaos")]
    [ApiController]
    public class SolicitacaoController : ControllerBase
    {
        private readonly ISolicitacaoService _solicitacaoService;
        private readonly IEquipamentoService _equipamentoService;

        private readonly ILogger<SolicitacaoController> _logger;

        public SolicitacaoController(ISolicitacaoService solicitacaoService, ILogger<SolicitacaoController> logger, IEquipamentoService equipamentoService)
        {
            _solicitacaoService = solicitacaoService;
            _logger = logger;
            _equipamentoService = equipamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSolicitacao([FromQuery] SolicitacaoFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var solicitacaos = _solicitacaoService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    solicitacaos.TotalCount,
                    solicitacaos.PageSize,
                    solicitacaos.CurrentPage,
                    solicitacaos.TotalPages,
                    solicitacaos.HasNext,
                    solicitacaos.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(solicitacaos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de solicitacaos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolicitacaoById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var solicitacao = await _solicitacaoService.GetByIdAsync(id);
                if (solicitacao == null)
                {
                    return NotFound();
                }
                return Ok(solicitacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o solicitacao com ID {SolicitacaoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        // retorna a lista de equipamentos de uma solictacao
        [HttpGet("{id:int}/equimentos")]
        public async Task<IActionResult> GetAllEquipamento(int id, [FromQuery] EquipamentoFilterParameters filterParameters)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                filterParameters.SolicitacaoId = id;

                var solicitacaos = _equipamentoService.GetAllPaged(filterParameters);
                var metadata = new
                {
                    solicitacaos.TotalCount,
                    solicitacaos.PageSize,
                    solicitacaos.CurrentPage,
                    solicitacaos.TotalPages,
                    solicitacaos.HasNext,
                    solicitacaos.HasPrevious
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(solicitacaos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de solicitacaos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSolicitacao([FromBody] SolicitacaoCreateDTO solicitacaoCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (solicitacaoCreateDto == null)
                {
                    return BadRequest();
                }

                var createdSolicitacao = await _solicitacaoService.CreateAsync(solicitacaoCreateDto);
                if (createdSolicitacao == null)
                {
                    return BadRequest("Erro ao criar o solicitacao. Verifique os dados fornecidos e tente novamente.");
                }

                return CreatedAtAction(nameof(GetSolicitacaoById), new { id = createdSolicitacao.Id }, createdSolicitacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar um novo solicitacao.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSolicitacao(int id, [FromBody] SolicitacaoUpdateDTO solicitacaoUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (solicitacaoUpdateDto == null)
                {
                    return BadRequest();
                }

                var updatedSolicitacao = await _solicitacaoService.UpdateAsync(id, solicitacaoUpdateDto);

                if (updatedSolicitacao == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a solicitacao com ID {SolicitacaoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitacao(int id)
        {
            try
            {
                var solicitacao = await _solicitacaoService.GetByIdAsync(id);
                if (solicitacao == null)
                {
                    return NotFound();
                }

                await _solicitacaoService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir o solicitacao com ID {SolicitacaoId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro interno no servidor.");
            }
        }


    }
}
