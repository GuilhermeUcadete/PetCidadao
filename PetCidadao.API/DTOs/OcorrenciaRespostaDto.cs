
namespace PetCidadao.API.DTOs
{
    public class OcorrenciaRespostaDto
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public DateTime DataOcorrencia { get; set; }

        public string? FotoUrl { get; set; }

        public string Status { get; set; } = string.Empty;

        public int? AnimalId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
