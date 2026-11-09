using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class DenunciaService : IDenunciaService
    {
        private readonly PetCidadaoContext _context;

        public DenunciaService(PetCidadaoContext context)
        {
            _context = context;
        }

        public async Task<DenunciaRespostaDto> CadastrarAsync(
            DenunciaCadastroDto dados)
        {
            bool usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id == dados.UsuarioId);

            if (!usuarioExiste)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            string tipo = dados.Tipo.Trim();

            if (tipo != "MausTratos" && tipo != "Abandono")
            {
                throw new InvalidOperationException(
                    "O tipo deve ser MausTratos ou Abandono."
                );
            }

            var denuncia = new Denuncia
            {
                Tipo = tipo,
                Descricao = dados.Descricao.Trim(),
                Localizacao = dados.Localizacao.Trim(),
                FotoUrl = dados.FotoUrl,
                UsuarioId = dados.UsuarioId,
                Status = "Recebida"
            };

            _context.Denuncias.Add(denuncia);

            await _context.SaveChangesAsync();

            return ConverterParaDto(denuncia);
        }

        public async Task<List<DenunciaRespostaDto>> ListarAsync()
        {
            var denuncias = await _context.Denuncias
                .AsNoTracking()
                .ToListAsync();

            return denuncias.Select(ConverterParaDto).ToList();
        }

        public async Task<DenunciaRespostaDto?> BuscarPorIdAsync(int id)
        {
            var denuncia = await _context.Denuncias
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (denuncia == null)
            {
                return null;
            }

            return ConverterParaDto(denuncia);
        }

        public async Task<DenunciaRespostaDto?> AtualizarStatusAsync(
            int id,
            string status)
        {
            var denuncia = await _context.Denuncias
                .FirstOrDefaultAsync(d => d.Id == id);

            if (denuncia == null)
            {
                return null;
            }

            string novoStatus = status.Trim();

            if (novoStatus != "Recebida" &&
                novoStatus != "EmAnalise" &&
                novoStatus != "Procedente" &&
                novoStatus != "Improcedente")
            {
                throw new InvalidOperationException(
                    "Status inválido. Utilize Recebida, EmAnalise, Procedente ou Improcedente."
                );
            }

            denuncia.Status = novoStatus;

            await _context.SaveChangesAsync();

            return ConverterParaDto(denuncia);
        }

        private static DenunciaRespostaDto ConverterParaDto(Denuncia denuncia)
        {
            return new DenunciaRespostaDto
            {
                Id = denuncia.Id,
                Tipo = denuncia.Tipo,
                Descricao = denuncia.Descricao,
                Localizacao = denuncia.Localizacao,
                Status = denuncia.Status,
                FotoUrl = denuncia.FotoUrl,
                UsuarioId = denuncia.UsuarioId,
                DataCadastro = denuncia.DataCadastro
            };
        }
    }
}