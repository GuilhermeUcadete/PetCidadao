
namespace PetCidadao.API.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string SenhaHash { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string TipoUsuario { get; set; } = string.Empty;

        public string? Endereco { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public ICollection<Animal> Animais { get; set; } = new List<Animal>();
    }
}
