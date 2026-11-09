namespace PetCidadao.API.DTOs
{
    public class SolicitacaoAdocaoRespostaDto
    {
        public int Id { get; set; }

        public int AdocaoId { get; set; }

        public int UsuarioId { get; set; }

        public string Mensagem { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime DataSolicitacao { get; set; }
    }
}