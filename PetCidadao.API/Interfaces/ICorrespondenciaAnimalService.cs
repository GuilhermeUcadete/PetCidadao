using PetCidadao.API.DTOs;

namespace PetCidadao.API.Interfaces
{
    public interface ICorrespondenciaAnimalService
    {
        Task<List<CorrespondenciaAnimalRespostaDto>> AnalisarAsync(
            int ocorrenciaPerdidaId
        );

        Task<List<CorrespondenciaAnimalRespostaDto>> ListarPorOcorrenciaAsync(
            int ocorrenciaId
        );
    }
}