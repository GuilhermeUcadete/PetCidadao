
namespace PetCidadao.API.Models
{
    public class SolicitacaoAdocao
    {
        public int Id { get; set; }

        public int AdocaoId { get; set; }

        public Adocao? Adocao { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }

        public string Mensagem { get; set; } = string.Empty;

        public string Status { get; set; } = "Pendente";

        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;
    }
}
