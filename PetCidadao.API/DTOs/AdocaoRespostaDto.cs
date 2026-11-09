
namespace PetCidadao.API.DTOs
{
    public class AdocaoRespostaDto
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int AnimalId { get; set; }

        public int UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
