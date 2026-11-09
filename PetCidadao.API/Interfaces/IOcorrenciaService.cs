
using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface IOcorrenciaService
    {
        Task<OcorrenciaRespostaDto> CadastrarAsync(
            OcorrenciaCadastroDto dados
        );

        Task<List<OcorrenciaRespostaDto>> ListarAsync();

        Task<OcorrenciaRespostaDto?> BuscarPorIdAsync(int id);

        Task<OcorrenciaRespostaDto?> AtualizarStatusAsync(
            int id,
            string status
        );
    }
}
