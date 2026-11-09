
using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioRespostaDto>> Cadastrar(
            UsuarioCadastroDto dados)
        {
            try
            {
                var usuario = await _usuarioService.CadastrarAsync(dados);

                return CreatedAtAction(
                    nameof(Listar),
                    new { id = usuario.Id },
                    usuario
                );
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioRespostaDto>>> Listar()
        {
            var usuarios = await _usuarioService.ListarAsync();

            return Ok(usuarios);
        }
    }
}
