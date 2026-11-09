using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface ISolicitacaoAdocaoService
    {
        Task<SolicitacaoAdocaoRespostaDto> CadastrarAsync(
            SolicitacaoAdocaoCadastroDto dados
        );

        Task<List<SolicitacaoAdocaoRespostaDto>> ListarPorAdocaoAsync(
            int adocaoId
        );

        Task<SolicitacaoAdocaoRespostaDto?> AtualizarStatusAsync(
            int id,
            string status
        );
    }
}