
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.Models;
using PetCidadao.API.Services;

namespace PetCidadao.Tests.Services
{
    public class CorrespondenciaAnimalServiceTests
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
        public async Task AnalisarAsync_DeveEncontrarAnimalCompativel()
        {
            using var context = CriarContexto();

            int usuarioId = await CriarUsuario(context);

            var animalPerdido = new Animal
            {
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                Idade = 3,
                Cor = "Caramelo",
                Descricao = "Animal perdido",
                UsuarioId = usuarioId
            };

            var animalEncontrado = new Animal
            {
                Nome = "Rex",
                Especie = "Cachorro",
                Raca = "Labrador",
                Sexo = "Macho",
                Idade = 3,
                Cor = "Caramelo",
                Descricao = "Animal encontrado",
                UsuarioId = usuarioId
            };

            context.Animais.AddRange(animalPerdido, animalEncontrado);
            await context.SaveChangesAsync();

            var ocorrenciaPerdida = new Ocorrencia
            {
                Tipo = "Perdido",
                Descricao = "Rex desapareceu",
                Localizacao = "Sao Paulo",
                DataOcorrencia = DateTime.UtcNow,
                UsuarioId = usuarioId,
                AnimalId = animalPerdido.Id
            };

            var ocorrenciaEncontrada = new Ocorrencia
            {
                Tipo = "Encontrado",
                Descricao = "Cachorro encontrado",
                Localizacao = "Sao Paulo",
                DataOcorrencia = DateTime.UtcNow,
                UsuarioId = usuarioId,
                AnimalId = animalEncontrado.Id
            };

            context.Ocorrencias.AddRange(
                ocorrenciaPerdida,
                ocorrenciaEncontrada
            );

            await context.SaveChangesAsync();

            var service = new CorrespondenciaAnimalService(context);

            var resultado = await service.AnalisarAsync(
                ocorrenciaPerdida.Id
            );

            Assert.Single(resultado);
            Assert.Equal(100, resultado[0].PercentualCompatibilidade);
            Assert.Equal(
                ocorrenciaEncontrada.Id,
                resultado[0].OcorrenciaEncontradaId
            );
        }

        [Fact]
        public async Task AnalisarAsync_DeveRetornarListaVaziaSemAnimalVinculado()
        {
            using var context = CriarContexto();

            int usuarioId = await CriarUsuario(context);

            var ocorrenciaPerdida = new Ocorrencia
            {
                Tipo = "Perdido",
                Descricao = "Animal desaparecido",
                Localizacao = "Sao Paulo",
                DataOcorrencia = DateTime.UtcNow,
                UsuarioId = usuarioId,
                AnimalId = null
            };

            context.Ocorrencias.Add(ocorrenciaPerdida);
            await context.SaveChangesAsync();

            var service = new CorrespondenciaAnimalService(context);

            var resultado = await service.AnalisarAsync(
                ocorrenciaPerdida.Id
            );

            Assert.Empty(resultado);
        }

        [Fact]
        public async Task AnalisarAsync_DeveImpedirOcorrenciaPerdidaInexistente()
        {
            using var context = CriarContexto();

            var service = new CorrespondenciaAnimalService(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.AnalisarAsync(999)
            );
        }
    }
}
