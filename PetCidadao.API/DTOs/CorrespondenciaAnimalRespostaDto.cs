namespace PetCidadao.API.DTOs
{
    public class CorrespondenciaAnimalRespostaDto
    {
        public int Id { get; set; }

        public int OcorrenciaPerdidaId { get; set; }

        public int OcorrenciaEncontradaId { get; set; }

        public decimal PercentualCompatibilidade { get; set; }

        public string Justificativa { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime DataAnalise { get; set; }
    }
}