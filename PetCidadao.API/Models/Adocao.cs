
namespace PetCidadao.API.Models
{
    public class Adocao
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public string Status { get; set; } = "Disponivel";

        public int AnimalId { get; set; }

        public Animal? Animal { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
