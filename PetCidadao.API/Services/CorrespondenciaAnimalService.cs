
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class CorrespondenciaAnimalService : ICorrespondenciaAnimalService
    {
        private readonly PetCidadaoContext _context;

        public CorrespondenciaAnimalService(PetCidadaoContext context)
        {
            _context = context;
        }

        public async Task<List<CorrespondenciaAnimalRespostaDto>> AnalisarAsync(
            int ocorrenciaPerdidaId)
        {
            var ocorrenciaPerdida = await _context.Ocorrencias
                .FirstOrDefaultAsync(o =>
                    o.Id == ocorrenciaPerdidaId &&
                    o.Tipo == "Perdido");

            if (ocorrenciaPerdida == null)
            {
                throw new KeyNotFoundException(
                    "Ocorrência de animal perdido não encontrada."
                );
            }

            var ocorrenciasEncontradas = await _context.Ocorrencias
                .Where(o =>
                    o.Tipo == "Encontrado" &&
                    o.Status == "Ativa")
                .ToListAsync();

            var animalPerdido = ocorrenciaPerdida.AnimalId.HasValue
                ? await _context.Animais.FirstOrDefaultAsync(
                    a => a.Id == ocorrenciaPerdida.AnimalId.Value)
                : null;

            if (animalPerdido == null)
            {
                return new List<CorrespondenciaAnimalRespostaDto>();
            }

            foreach (var encontrada in ocorrenciasEncontradas)
            {
                if (!encontrada.AnimalId.HasValue)
                {
                    continue;
                }

                var animalEncontrado = await _context.Animais
                    .FirstOrDefaultAsync(a => a.Id == encontrada.AnimalId.Value);

                if (animalEncontrado == null)
                {
                    continue;
                }

                decimal percentual = CalcularCompatibilidade(
                    animalPerdido,
                    animalEncontrado
                );

                if (percentual < 50)
                {
                    continue;
                }

                var correspondenciaExistente = await _context.CorrespondenciasAnimais
                    .FirstOrDefaultAsync(c =>
                        c.OcorrenciaPerdidaId == ocorrenciaPerdidaId &&
                        c.OcorrenciaEncontradaId == encontrada.Id);

                if (correspondenciaExistente == null)
                {
                    var correspondencia = new CorrespondenciaAnimal
                    {
                        OcorrenciaPerdidaId = ocorrenciaPerdidaId,
                        OcorrenciaEncontradaId = encontrada.Id,
                        PercentualCompatibilidade = percentual,
                        Justificativa = "Compatibilidade calculada com base na espécie, raça e cor.",
                        Status = "Pendente"
                    };

                    _context.CorrespondenciasAnimais.Add(correspondencia);
                }
                else
                {
                    correspondenciaExistente.PercentualCompatibilidade = percentual;
                }
            }

            await _context.SaveChangesAsync();

            var resultados = await _context.CorrespondenciasAnimais
                .AsNoTracking()
                .Where(c => c.OcorrenciaPerdidaId == ocorrenciaPerdidaId)
                .ToListAsync();

            return resultados.Select(ConverterParaDto).ToList();
        }

        public async Task<List<CorrespondenciaAnimalRespostaDto>> ListarPorOcorrenciaAsync(
            int ocorrenciaId)
        {
            var correspondencias = await _context.CorrespondenciasAnimais
                .AsNoTracking()
                .Where(c =>
                    c.OcorrenciaPerdidaId == ocorrenciaId ||
                    c.OcorrenciaEncontradaId == ocorrenciaId)
                .ToListAsync();

            return correspondencias.Select(ConverterParaDto).ToList();
        }

        private static decimal CalcularCompatibilidade(
            Animal animalPerdido,
            Animal animalEncontrado)
        {
            decimal percentual = 0;

            if (animalPerdido.Especie.Equals(
                animalEncontrado.Especie,
                StringComparison.OrdinalIgnoreCase))
            {
                percentual += 40;
            }

            if (animalPerdido.Raca.Equals(
                animalEncontrado.Raca,
                StringComparison.OrdinalIgnoreCase))
            {
                percentual += 30;
            }

            if (animalPerdido.Cor.Equals(
                animalEncontrado.Cor,
                StringComparison.OrdinalIgnoreCase))
            {
                percentual += 30;
            }

            return percentual;
        }

        private static CorrespondenciaAnimalRespostaDto ConverterParaDto(
            CorrespondenciaAnimal correspondencia)
        {
            return new CorrespondenciaAnimalRespostaDto
            {
                Id = correspondencia.Id,
                OcorrenciaPerdidaId = correspondencia.OcorrenciaPerdidaId,
                OcorrenciaEncontradaId = correspondencia.OcorrenciaEncontradaId,
                PercentualCompatibilidade = correspondencia.PercentualCompatibilidade,
                Justificativa = correspondencia.Justificativa,
                Status = correspondencia.Status,
                DataAnalise = correspondencia.DataAnalise
            };
        }
    }
}
