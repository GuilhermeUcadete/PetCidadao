namespace PetCidadao.API.DTOs
{
    public class UsuarioRespostaDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string TipoUsuario { get; set; } = string.Empty;

        public string? Endereco { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}