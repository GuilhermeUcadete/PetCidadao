
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Models;
using PetCidadao.API.Services;

namespace PetCidadao.Tests.Services
{
    public class AdocaoServiceTests
    {
        private PetCidadaoContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<PetCidadaoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PetCidadaoContext(options);
        }

        private async Task<(int usuarioId, int animalId)> CriarDados(
            PetCidadaoContext context)
        {
            var usuario = new Usuario
            {
                Nome = "Joao Silva",
                Email = "joao@email.com",
                SenhaHash = "hash",
                Telefone = "11999999999",
                TipoUsuario = "Protetor"
            };

            context.Usuarios.Add(usuario);
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
                UsuarioId = usuario.Id
            };

            context.Animais.Add(animal);
            await context.SaveChangesAsync();

            return (usuario.Id, animal.Id);
        }

        [Fact]
        public async Task CadastrarAsync_DeveCadastrarAdocaoComSucesso()
        {
            using var context = CriarContexto();

            var ids = await CriarDados(context);
            var service = new AdocaoService(context);

            var dados = new AdocaoCadastroDto
            {
                Descricao = "Rex procura um novo lar",
                AnimalId = ids.animalId,
                UsuarioId = ids.usuarioId
            };

            var resultado = await service.CadastrarAsync(dados);

            Assert.Equal("Rex procura um novo lar", resultado.Descricao);
            Assert.Equal("Disponivel", resultado.Status);
            Assert.Equal(ids.animalId, resultado.AnimalId);
            Assert.Equal(ids.usuarioId, resultado.UsuarioId);
        }

        [Fact]
        public async Task ListarDisponiveisAsync_DeveRetornarAdocoesDisponiveis()
        {
            using var context = CriarContexto();

            var ids = await CriarDados(context);
            var service = new AdocaoService(context);

            var dados = new AdocaoCadastroDto
            {
                Descricao = "Rex procura um novo lar",
                AnimalId = ids.animalId,
                UsuarioId = ids.usuarioId
            };

            await service.CadastrarAsync(dados);

            var resultado = await service.ListarDisponiveisAsync();

            Assert.Single(resultado);
            Assert.Equal("Disponivel", resultado[0].Status);
        }

        [Fact]
        public async Task CadastrarAsync_NaoDevePermitirAdocaoDuplicada()
        {
            using var context = CriarContexto();

            var ids = await CriarDados(context);
            var service = new AdocaoService(context);

            var dados = new AdocaoCadastroDto
            {
                Descricao = "Rex procura um novo lar",
                AnimalId = ids.animalId,
                UsuarioId = ids.usuarioId
            };

            await service.CadastrarAsync(dados);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CadastrarAsync(dados)
            );
        }
    }
}
