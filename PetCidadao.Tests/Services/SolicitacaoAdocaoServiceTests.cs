
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Models;
using PetCidadao.API.Services;

namespace PetCidadao.Tests.Services
{
    public class SolicitacaoAdocaoServiceTests
    {
        private PetCidadaoContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<PetCidadaoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PetCidadaoContext(options);
        }

        private async Task<(int usuarioId, int animalId, int adocaoId)> CriarDados(
            PetCidadaoContext context)
        {
            var protetor = new Usuario
            {
                Nome = "Joao Silva",
                Email = "joao@email.com",
                SenhaHash = "hash",
                Telefone = "11999999999",
                TipoUsuario = "Protetor"
            };

            var interessado = new Usuario
            {
                Nome = "Maria Silva",
                Email = "maria@email.com",
                SenhaHash = "hash",
                Telefone = "11888888888",
                TipoUsuario = "Cidadao"
            };

            context.Usuarios.AddRange(protetor, interessado);
            await context.SaveChangesAsync();

            var animal = new Animal
            {
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                Idade = 3,
                Cor = "Caramelo",
                Descricao = "Animal docil",
                UsuarioId = protetor.Id
            };

            context.Animais.Add(animal);
            await context.SaveChangesAsync();

            var adocao = new Adocao
            {
                Descricao = "Rex procura um novo lar",
                Status = "Disponivel",
                AnimalId = animal.Id,
                UsuarioId = protetor.Id
            };

            context.Adocoes.Add(adocao);
            await context.SaveChangesAsync();

            return (interessado.Id, animal.Id, adocao.Id);
        }

        [Fact]
        public async Task CadastrarAsync_DeveCadastrarSolicitacaoComSucesso()
        {
            using var context = CriarContexto();

            var ids = await CriarDados(context);
            var service = new SolicitacaoAdocaoService(context);

            var dados = new SolicitacaoAdocaoCadastroDto
            {
                AdocaoId = ids.adocaoId,
                UsuarioId = ids.usuarioId,
                Mensagem = "Gostaria de adotar o Rex."
            };

            var resultado = await service.CadastrarAsync(dados);

            Assert.Equal(ids.adocaoId, resultado.AdocaoId);
            Assert.Equal(ids.usuarioId, resultado.UsuarioId);
            Assert.Equal("Pendente", resultado.Status);
        }

        [Fact]
        public async Task CadastrarAsync_NaoDevePermitirSolicitacaoParaPropriaAdocao()
        {
            using var context = CriarContexto();

            var ids = await CriarDados(context);
            var service = new SolicitacaoAdocaoService(context);

            var adocao = await context.Adocoes.FirstAsync(
                a => a.Id == ids.adocaoId
            );

            var dados = new SolicitacaoAdocaoCadastroDto
            {
                AdocaoId = ids.adocaoId,
                UsuarioId = adocao.UsuarioId,
                Mensagem = "Quero adotar meu próprio animal."
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CadastrarAsync(dados)
            );
        }

        [Fact]
        public async Task AtualizarStatusAsync_AoAprovarDeveMarcarAdocaoComoAdotada()
        {
            using var context = CriarContexto();

            var ids = await CriarDados(context);
            var service = new SolicitacaoAdocaoService(context);

            var dados = new SolicitacaoAdocaoCadastroDto
            {
                AdocaoId = ids.adocaoId,
                UsuarioId = ids.usuarioId,
                Mensagem = "Tenho interesse na adoção."
            };

            var solicitacao = await service.CadastrarAsync(dados);

            var resultado = await service.AtualizarStatusAsync(
                solicitacao.Id,
                "Aprovada"
            );

            var adocaoAtualizada = await context.Adocoes
                .FirstAsync(a => a.Id == ids.adocaoId);

            Assert.NotNull(resultado);
            Assert.Equal("Aprovada", resultado.Status);
            Assert.Equal("Adotado", adocaoAtualizada.Status);
        }
    }
}
