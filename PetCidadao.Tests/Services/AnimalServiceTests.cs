
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Models;
using PetCidadao.API.Services;

namespace PetCidadao.Tests.Services
{
    public class AnimalServiceTests
    {
        private PetCidadaoContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<PetCidadaoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PetCidadaoContext(options);
        }

        private async Task<int> CriarUsuario(PetCidadaoContext context)
        {
            var usuario = new Usuario
            {
                Nome = "Joao Silva",
                Email = "joao@email.com",
                SenhaHash = "hash",
                Telefone = "11999999999",
                TipoUsuario = "Cidadao"
            };

            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();

            return usuario.Id;
        }

        [Fact]
        public async Task CadastrarAsync_DeveCadastrarAnimalComSucesso()
        {
            using var context = CriarContexto();

            int usuarioId = await CriarUsuario(context);

            var service = new AnimalService(context);

            var dados = new AnimalCadastroDto
            {
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                Idade = 3,
                Cor = "Caramelo",
                Descricao = "Animal dócil",
                CodigoRfid = "PET001",
                UsuarioId = usuarioId
            };

            var resultado = await service.CadastrarAsync(dados);

            Assert.Equal("Rex", resultado.Nome);
            Assert.Equal("Cachorro", resultado.Especie);
            Assert.Equal("PET001", resultado.CodigoRfid);
            Assert.Equal(usuarioId, resultado.UsuarioId);
        }

        [Fact]
        public async Task CadastrarAsync_NaoDevePermitirRfidDuplicado()
        {
            using var context = CriarContexto();

            int usuarioId = await CriarUsuario(context);

            var service = new AnimalService(context);

            var dados = new AnimalCadastroDto
            {
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                Idade = 3,
                Cor = "Caramelo",
                Descricao = "Animal dócil",
                CodigoRfid = "PET001",
                UsuarioId = usuarioId
            };

            await service.CadastrarAsync(dados);

            var segundoAnimal = new AnimalCadastroDto
            {
                Nome = "Bob",
                Especie = "Cachorro",
                Raca = "Poodle",
                Sexo = "Macho",
                Idade = 2,
                Cor = "Branco",
                Descricao = "Animal tranquilo",
                CodigoRfid = "PET001",
                UsuarioId = usuarioId
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CadastrarAsync(segundoAnimal)
            );
        }

        [Fact]
        public async Task CadastrarAsync_DeveImpedirCadastroComUsuarioInexistente()
        {
            using var context = CriarContexto();

            var service = new AnimalService(context);

            var dados = new AnimalCadastroDto
            {
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                Idade = 3,
                Cor = "Caramelo",
                Descricao = "Animal dócil",
                UsuarioId = 999
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.CadastrarAsync(dados)
            );
        }
    }
}
