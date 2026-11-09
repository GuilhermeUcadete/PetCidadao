using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacaoAdocaoController : ControllerBase
    {
        private readonly ISolicitacaoAdocaoService _solicitacaoService;

        public SolicitacaoAdocaoController(
            ISolicitacaoAdocaoService solicitacaoService)
        {
            _solicitacaoService = solicitacaoService;
        }

        [HttpPost]
        public async Task<ActionResult<SolicitacaoAdocaoRespostaDto>> Cadastrar(
            SolicitacaoAdocaoCadastroDto dados)
        {
            try
            {
                var solicitacao = await _solicitacaoService.CadastrarAsync(dados);

                return CreatedAtAction(
                    nameof(ListarPorAdocao),
                    new { adocaoId = solicitacao.AdocaoId },
                    solicitacao
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

        [HttpGet("adocao/{adocaoId:int}")]
        public async Task<ActionResult<List<SolicitacaoAdocaoRespostaDto>>>
            ListarPorAdocao(int adocaoId)
        {
            var solicitacoes = await _solicitacaoService
                .ListarPorAdocaoAsync(adocaoId);

            return Ok(solicitacoes);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<SolicitacaoAdocaoRespostaDto>>
            AtualizarStatus(int id, [FromQuery] string status)
        {
            try
            {
                var solicitacao = await _solicitacaoService
                    .AtualizarStatusAsync(id, status);

                if (solicitacao == null)
                {
                    return NotFound(new { mensagem = "Solicitação não encontrada." });
                }

                return Ok(solicitacao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}