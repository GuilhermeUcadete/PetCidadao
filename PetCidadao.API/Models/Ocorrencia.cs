
namespace PetCidadao.API.Models
{
    public class Ocorrencia
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public DateTime DataOcorrencia { get; set; }

        public string? FotoUrl { get; set; }

        public string Status { get; set; } = "Ativa";

        public int? AnimalId { get; set; }

        public Animal? Animal { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
