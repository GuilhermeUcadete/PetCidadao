
using Microsoft.AspNetCore.Mvc;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;

namespace PetCidadao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpPost]
        public async Task<ActionResult<AnimalRespostaDto>> Cadastrar(
            AnimalCadastroDto dados)
        {
            try
            {
                var animal = await _animalService.CadastrarAsync(dados);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = animal.Id },
                    animal
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<AnimalRespostaDto>>> Listar()
        {
            var animais = await _animalService.ListarAsync();

            return Ok(animais);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AnimalRespostaDto>> BuscarPorId(int id)
        {
            var animal = await _animalService.BuscarPorIdAsync(id);

            if (animal == null)
            {
                return NotFound(new { mensagem = "Animal não encontrado." });
            }

            return Ok(animal);
        }

        [HttpGet("rfid/{codigoRfid}")]
        public async Task<ActionResult<AnimalRespostaDto>> BuscarPorRfid(
            string codigoRfid)
        {
            var animal = await _animalService.BuscarPorRfidAsync(codigoRfid);

            if (animal == null)
            {
                return NotFound(new { mensagem = "Animal não encontrado." });
            }

            return Ok(animal);
        }
    }
}
