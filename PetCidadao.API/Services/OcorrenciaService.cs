
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class OcorrenciaService : IOcorrenciaService
    {
        private readonly PetCidadaoContext _context;

        public OcorrenciaService(PetCidadaoContext context)
        {
            _context = context;
        }

        public async Task<OcorrenciaRespostaDto> CadastrarAsync(
            OcorrenciaCadastroDto dados)
        {
            bool usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id == dados.UsuarioId);

            if (!usuarioExiste)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            if (dados.AnimalId.HasValue)
            {
                bool animalExiste = await _context.Animais
                    .AnyAsync(a => a.Id == dados.AnimalId.Value);

                if (!animalExiste)
                {
                    throw new KeyNotFoundException("Animal não encontrado.");
                }
            }

            string tipo = dados.Tipo.Trim();

            if (tipo != "Perdido" && tipo != "Encontrado")
            {
                throw new InvalidOperationException(
                    "O tipo deve ser Perdido ou Encontrado."
                );
            }

            var ocorrencia = new Ocorrencia
            {
                Tipo = tipo,
                Descricao = dados.Descricao.Trim(),
                Localizacao = dados.Localizacao.Trim(),
                DataOcorrencia = dados.DataOcorrencia,
                FotoUrl = dados.FotoUrl,
                AnimalId = dados.AnimalId,
                UsuarioId = dados.UsuarioId,
                Status = "Ativa"
            };

            _context.Ocorrencias.Add(ocorrencia);

            await _context.SaveChangesAsync();

            return ConverterParaDto(ocorrencia);
        }

        public async Task<List<OcorrenciaRespostaDto>> ListarAsync()
        {
            var ocorrencias = await _context.Ocorrencias
                .AsNoTracking()
                .ToListAsync();

            return ocorrencias.Select(ConverterParaDto).ToList();
        }

        public async Task<OcorrenciaRespostaDto?> BuscarPorIdAsync(int id)
        {
            var ocorrencia = await _context.Ocorrencias
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ocorrencia == null)
            {
                return null;
            }

            return ConverterParaDto(ocorrencia);
        }

        public async Task<OcorrenciaRespostaDto?> AtualizarStatusAsync(
            int id,
            string status)
        {
            var ocorrencia = await _context.Ocorrencias
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ocorrencia == null)
            {
                return null;
            }

            string novoStatus = status.Trim();

            if (novoStatus != "Ativa" &&
                novoStatus != "Resolvida" &&
                novoStatus != "Cancelada")
            {
                throw new InvalidOperationException(
                    "Status inválido. Utilize Ativa, Resolvida ou Cancelada."
                );
            }

            ocorrencia.Status = novoStatus;

            await _context.SaveChangesAsync();

            return ConverterParaDto(ocorrencia);
        }

        private static OcorrenciaRespostaDto ConverterParaDto(
            Ocorrencia ocorrencia)
        {
            return new OcorrenciaRespostaDto
            {
                Id = ocorrencia.Id,
                Tipo = ocorrencia.Tipo,
                Descricao = ocorrencia.Descricao,
                Localizacao = ocorrencia.Localizacao,
                DataOcorrencia = ocorrencia.DataOcorrencia,
                FotoUrl = ocorrencia.FotoUrl,
                Status = ocorrencia.Status,
                AnimalId = ocorrencia.AnimalId,
                UsuarioId = ocorrencia.UsuarioId,
                DataCadastro = ocorrencia.DataCadastro
            };
        }
    }
}
