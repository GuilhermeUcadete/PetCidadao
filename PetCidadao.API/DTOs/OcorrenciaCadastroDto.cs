
namespace PetCidadao.API.DTOs
{
    public class OcorrenciaCadastroDto
    {
        public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public DateTime DataOcorrencia { get; set; }

        public string? FotoUrl { get; set; }

        public int? AnimalId { get; set; }

        public int UsuarioId { get; set; }
    }
}
