
using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface IAnimalService
    {
        Task<AnimalRespostaDto> CadastrarAsync(AnimalCadastroDto dados);

        Task<List<AnimalRespostaDto>> ListarAsync();

        Task<AnimalRespostaDto?> BuscarPorIdAsync(int id);

        Task<AnimalRespostaDto?> BuscarPorRfidAsync(string codigoRfid);
    }
}
