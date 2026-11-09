
using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OcorrenciaController : ControllerBase
    {
        private readonly IOcorrenciaService _ocorrenciaService;

        public OcorrenciaController(IOcorrenciaService ocorrenciaService)
        {
            _ocorrenciaService = ocorrenciaService;
        }

        [HttpPost]
        public async Task<ActionResult<OcorrenciaRespostaDto>> Cadastrar(
            OcorrenciaCadastroDto dados)
        {
            try
            {
                var ocorrencia = await _ocorrenciaService.CadastrarAsync(dados);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = ocorrencia.Id },
                    ocorrencia
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OcorrenciaRespostaDto>>> Listar()
        {
            var ocorrencias = await _ocorrenciaService.ListarAsync();

            return Ok(ocorrencias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OcorrenciaRespostaDto>> BuscarPorId(int id)
        {
            var ocorrencia = await _ocorrenciaService.BuscarPorIdAsync(id);

            if (ocorrencia == null)
            {
                return NotFound(new { mensagem = "Ocorrência não encontrada." });
            }

            return Ok(ocorrencia);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<OcorrenciaRespostaDto>> AtualizarStatus(
            int id,
            [FromQuery] string status)
        {
            try
            {
                var ocorrencia = await _ocorrenciaService
                    .AtualizarStatusAsync(id, status);

                if (ocorrencia == null)
                {
                    return NotFound(new { mensagem = "Ocorrência não encontrada." });
                }

                return Ok(ocorrencia);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
