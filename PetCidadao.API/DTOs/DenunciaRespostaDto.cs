namespace PetCidadao.API.DTOs
{
    public class DenunciaRespostaDto
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? FotoUrl { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}