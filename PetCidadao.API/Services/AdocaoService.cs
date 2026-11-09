using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class AdocaoService : IAdocaoService
    {
        private readonly PetCidadaoContext _context;

        public AdocaoService(PetCidadaoContext context)
        {
            _context = context;
        }

        public async Task<AdocaoRespostaDto> CadastrarAsync(AdocaoCadastroDto dados)
        {
            bool animalExiste = await _context.Animais
                .AnyAsync(a => a.Id == dados.AnimalId);

            if (!animalExiste)
            {
                throw new KeyNotFoundException("Animal não encontrado.");
            }

            bool usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id == dados.UsuarioId);

            if (!usuarioExiste)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            bool adocaoExiste = await _context.Adocoes
                .AnyAsync(a => a.AnimalId == dados.AnimalId &&
                               a.Status == "Disponivel");

            if (adocaoExiste)
            {
                throw new InvalidOperationException(
                    "Este animal já possui uma publicação de adoção disponível."
                );
            }

            var adocao = new Adocao
            {
                Descricao = dados.Descricao.Trim(),
                AnimalId = dados.AnimalId,
                UsuarioId = dados.UsuarioId,
                Status = "Disponivel"
            };

            _context.Adocoes.Add(adocao);

            await _context.SaveChangesAsync();

            return ConverterParaDto(adocao);
        }

        public async Task<List<AdocaoRespostaDto>> ListarDisponiveisAsync()
        {
            var adocoes = await _context.Adocoes
                .AsNoTracking()
                .Where(a => a.Status == "Disponivel")
                .ToListAsync();

            return adocoes.Select(ConverterParaDto).ToList();
        }

        public async Task<AdocaoRespostaDto?> BuscarPorIdAsync(int id)
        {
            var adocao = await _context.Adocoes
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adocao == null)
            {
                return null;
            }

            return ConverterParaDto(adocao);
        }

        public async Task<AdocaoRespostaDto?> AtualizarStatusAsync(
            int id,
            string status)
        {
            var adocao = await _context.Adocoes
                .FirstOrDefaultAsync(a => a.Id == id);

            if (adocao == null)
            {
                return null;
            }

            string novoStatus = status.Trim();

            if (novoStatus != "Disponivel" &&
                novoStatus != "EmProcesso" &&
                novoStatus != "Adotado" &&
                novoStatus != "Cancelado")
            {
                throw new InvalidOperationException(
                    "Status inválido. Utilize Disponivel, EmProcesso, Adotado ou Cancelado."
                );
            }

            adocao.Status = novoStatus;

            await _context.SaveChangesAsync();

            return ConverterParaDto(adocao);
        }

        private static AdocaoRespostaDto ConverterParaDto(Adocao adocao)
        {
            return new AdocaoRespostaDto
            {
                Id = adocao.Id,
                Descricao = adocao.Descricao,
                Status = adocao.Status,
                AnimalId = adocao.AnimalId,
                UsuarioId = adocao.UsuarioId,
                DataCadastro = adocao.DataCadastro
            };
        }
    }
}