using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioRespostaDto> CadastrarAsync(UsuarioCadastroDto dados);

        Task<List<UsuarioRespostaDto>> ListarAsync();
    }
}