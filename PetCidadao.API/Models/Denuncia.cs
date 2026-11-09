
namespace PetCidadao.API.Models
{
    public class Denuncia
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string Localizacao { get; set; } = string.Empty;

        public string Status { get; set; } = "Recebida";

        public string? FotoUrl { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
