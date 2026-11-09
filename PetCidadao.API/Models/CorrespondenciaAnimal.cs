namespace PetCidadao.API.Models
{
    public class CorrespondenciaAnimal
    {
        public int Id { get; set; }

        public int OcorrenciaPerdidaId { get; set; }

        public Ocorrencia? OcorrenciaPerdida { get; set; }

        public int OcorrenciaEncontradaId { get; set; }

        public Ocorrencia? OcorrenciaEncontrada { get; set; }

        public decimal PercentualCompatibilidade { get; set; }

        public string Justificativa { get; set; } = string.Empty;

        public string Status { get; set; } = "Pendente";

        public DateTime DataAnalise { get; set; } = DateTime.UtcNow;
    }
}