using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DenunciaController : ControllerBase
    {
        private readonly IDenunciaService _denunciaService;

        public DenunciaController(IDenunciaService denunciaService)
        {
            _denunciaService = denunciaService;
        }

        [HttpPost]
        public async Task<ActionResult<DenunciaRespostaDto>> Cadastrar(
            DenunciaCadastroDto dados)
        {
            try
            {
                var denuncia = await _denunciaService.CadastrarAsync(dados);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = denuncia.Id },
                    denuncia
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
        public async Task<ActionResult<List<DenunciaRespostaDto>>> Listar()
        {
            var denuncias = await _denunciaService.ListarAsync();

            return Ok(denuncias);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DenunciaRespostaDto>> BuscarPorId(int id)
        {
            var denuncia = await _denunciaService.BuscarPorIdAsync(id);

            if (denuncia == null)
            {
                return NotFound(new { mensagem = "Denúncia não encontrada." });
            }

            return Ok(denuncia);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<DenunciaRespostaDto>> AtualizarStatus(
            int id,
            [FromQuery] string status)
        {
            try
            {
                var denuncia = await _denunciaService
                    .AtualizarStatusAsync(id, status);

                if (denuncia == null)
                {
                    return NotFound(new { mensagem = "Denúncia não encontrada." });
                }

                return Ok(denuncia);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}