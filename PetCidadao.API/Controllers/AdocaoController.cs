using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdocaoController : ControllerBase
    {
        private readonly IAdocaoService _adocaoService;

        public AdocaoController(IAdocaoService adocaoService)
        {
            _adocaoService = adocaoService;
        }

        [HttpPost]
        public async Task<ActionResult<AdocaoRespostaDto>> Cadastrar(
            AdocaoCadastroDto dados)
        {
            try
            {
                var adocao = await _adocaoService.CadastrarAsync(dados);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = adocao.Id },
                    adocao
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
        public async Task<ActionResult<List<AdocaoRespostaDto>>> Listar()
        {
            var adocoes = await _adocaoService.ListarDisponiveisAsync();

            return Ok(adocoes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdocaoRespostaDto>> BuscarPorId(int id)
        {
            var adocao = await _adocaoService.BuscarPorIdAsync(id);

            if (adocao == null)
            {
                return NotFound(new { mensagem = "Publicação não encontrada." });
            }

            return Ok(adocao);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<AdocaoRespostaDto>> AtualizarStatus(
            int id,
            [FromQuery] string status)
        {
            try
            {
                var adocao = await _adocaoService.AtualizarStatusAsync(id, status);

                if (adocao == null)
                {
                    return NotFound(new { mensagem = "Publicação não encontrada." });
                }

                return Ok(adocao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}