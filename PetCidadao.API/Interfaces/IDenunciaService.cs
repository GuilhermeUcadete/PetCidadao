using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface IDenunciaService
    {
        Task<DenunciaRespostaDto> CadastrarAsync(
            DenunciaCadastroDto dados
        );

        Task<List<DenunciaRespostaDto>> ListarAsync();

        Task<DenunciaRespostaDto?> BuscarPorIdAsync(int id);

        Task<DenunciaRespostaDto?> AtualizarStatusAsync(
            int id,
            string status
        );
    }
}