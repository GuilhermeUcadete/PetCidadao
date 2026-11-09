using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CorrespondenciaAnimalController : ControllerBase
    {
        private readonly ICorrespondenciaAnimalService _correspondenciaService;

        public CorrespondenciaAnimalController(
            ICorrespondenciaAnimalService correspondenciaService)
        {
            _correspondenciaService = correspondenciaService;
        }

        [HttpPost("analisar/{ocorrenciaPerdidaId:int}")]
        public async Task<ActionResult<List<CorrespondenciaAnimalRespostaDto>>>
            Analisar(int ocorrenciaPerdidaId)
        {
            try
            {
                var correspondencias = await _correspondenciaService
                    .AnalisarAsync(ocorrenciaPerdidaId);

                return Ok(correspondencias);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpGet("ocorrencia/{ocorrenciaId:int}")]
        public async Task<ActionResult<List<CorrespondenciaAnimalRespostaDto>>>
            ListarPorOcorrencia(int ocorrenciaId)
        {
            var correspondencias = await _correspondenciaService
                .ListarPorOcorrenciaAsync(ocorrenciaId);

            return Ok(correspondencias);
        }
    }
}