using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface IAdocaoService
    {
        Task<AdocaoRespostaDto> CadastrarAsync(AdocaoCadastroDto dados);

        Task<List<AdocaoRespostaDto>> ListarDisponiveisAsync();

        Task<AdocaoRespostaDto?> BuscarPorIdAsync(int id);

        Task<AdocaoRespostaDto?> AtualizarStatusAsync(int id, string status);
    }
}