namespace PetCidadao.API.DTOs
{
    public class UsuarioCadastroDto
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string TipoUsuario { get; set; } = string.Empty;

        public string? Endereco { get; set; }
    }
}