
namespace PetCidadao.API.DTOs
{
    public class AnimalRespostaDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Especie { get; set; } = string.Empty;

        public string Raca { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public int Idade { get; set; }

        public string Cor { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public string? FotoUrl { get; set; }

        public string? CodigoRfid { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
