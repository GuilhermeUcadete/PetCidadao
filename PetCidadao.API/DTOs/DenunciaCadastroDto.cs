namespace PetCidadao.API.DTOs
{
    public class DenunciaCadastroDto
    {
        public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public string? FotoUrl { get; set; }

        public int UsuarioId { get; set; }
    }
}