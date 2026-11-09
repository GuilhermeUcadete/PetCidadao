using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class SolicitacaoAdocaoService : ISolicitacaoAdocaoService
    {
        private readonly PetCidadaoContext _context;

        public SolicitacaoAdocaoService(PetCidadaoContext context)
        {
            _context = context;
        }

        public async Task<SolicitacaoAdocaoRespostaDto> CadastrarAsync(
            SolicitacaoAdocaoCadastroDto dados)
        {
            var adocao = await _context.Adocoes
                .FirstOrDefaultAsync(a => a.Id == dados.AdocaoId);

            if (adocao == null)
            {
                throw new KeyNotFoundException("Publicação de adoção não encontrada.");
            }

            if (adocao.Status != "Disponivel")
            {
                throw new InvalidOperationException(
                    "Este animal não está mais disponível para adoção."
                );
            }

            bool usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id == dados.UsuarioId);

            if (!usuarioExiste)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            if (adocao.UsuarioId == dados.UsuarioId)
            {
                throw new InvalidOperationException(
                    "Você não pode solicitar a adoção do seu próprio animal."
                );
            }

            bool solicitacaoExiste = await _context.SolicitacoesAdocao
                .AnyAsync(s => s.AdocaoId == dados.AdocaoId &&
                               s.UsuarioId == dados.UsuarioId &&
                               s.Status == "Pendente");

            if (solicitacaoExiste)
            {
                throw new InvalidOperationException(
                    "Você já possui uma solicitação pendente para este animal."
                );
            }

            var solicitacao = new SolicitacaoAdocao
            {
                AdocaoId = dados.AdocaoId,
                UsuarioId = dados.UsuarioId,
                Mensagem = dados.Mensagem.Trim(),
                Status = "Pendente"
            };

            _context.SolicitacoesAdocao.Add(solicitacao);

            await _context.SaveChangesAsync();

            return ConverterParaDto(solicitacao);
        }

        public async Task<List<SolicitacaoAdocaoRespostaDto>> ListarPorAdocaoAsync(
            int adocaoId)
        {
            var solicitacoes = await _context.SolicitacoesAdocao
                .AsNoTracking()
                .Where(s => s.AdocaoId == adocaoId)
                .ToListAsync();

            return solicitacoes.Select(ConverterParaDto).ToList();
        }

        public async Task<SolicitacaoAdocaoRespostaDto?> AtualizarStatusAsync(
            int id,
            string status)
        {
            var solicitacao = await _context.SolicitacoesAdocao
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitacao == null)
            {
                return null;
            }

            string novoStatus = status.Trim();

            if (novoStatus != "Pendente" &&
                novoStatus != "Aprovada" &&
                novoStatus != "Recusada" &&
                novoStatus != "Cancelada")
            {
                throw new InvalidOperationException(
                    "Status inválido. Utilize Pendente, Aprovada, Recusada ou Cancelada."
                );
            }

            if (novoStatus == "Aprovada")
            {
                var adocao = await _context.Adocoes
                    .FirstOrDefaultAsync(a => a.Id == solicitacao.AdocaoId);

                if (adocao == null || adocao.Status != "Disponivel")
                {
                    throw new InvalidOperationException(
                        "Este animal não está mais disponível para adoção."
                    );
                }

                adocao.Status = "Adotado";
            }

            solicitacao.Status = novoStatus;

            await _context.SaveChangesAsync();

            return ConverterParaDto(solicitacao);
        }

        private static SolicitacaoAdocaoRespostaDto ConverterParaDto(
            SolicitacaoAdocao solicitacao)
        {
            return new SolicitacaoAdocaoRespostaDto
            {
                Id = solicitacao.Id,
                AdocaoId = solicitacao.AdocaoId,
                UsuarioId = solicitacao.UsuarioId,
                Mensagem = solicitacao.Mensagem,
                Status = solicitacao.Status,
                DataSolicitacao = solicitacao.DataSolicitacao
            };
        }
    }
}