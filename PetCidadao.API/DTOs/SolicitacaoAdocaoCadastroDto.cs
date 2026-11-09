namespace PetCidadao.API.DTOs
{
    public class SolicitacaoAdocaoCadastroDto
    {
        public int AdocaoId { get; set; }

        public int UsuarioId { get; set; }

        public string Mensagem { get; set; } = string.Empty;
    }
}