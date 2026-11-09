
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Models;
using PetCidadao.API.Services;

namespace PetCidadao.Tests.Services
{
    public class OcorrenciaServiceTests
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
        public async Task CadastrarAsync_DeveCadastrarOcorrenciaPerdida()
        {
            using var context = CriarContexto();

            int usuarioId = await CriarUsuario(context);

            var service = new OcorrenciaService(context);

            var dados = new OcorrenciaCadastroDto
            {
                Tipo = "Perdido",
                Descricao = "Cachorro desaparecido",
                Localizacao = "Sao Bernardo do Campo",
                DataOcorrencia = DateTime.UtcNow,
                UsuarioId = usuarioId
            };

            var resultado = await service.CadastrarAsync(dados);

            Assert.Equal("Perdido", resultado.Tipo);
            Assert.Equal("Ativa", resultado.Status);
            Assert.Equal(usuarioId, resultado.UsuarioId);
        }

        [Fact]
        public async Task CadastrarAsync_NaoDeveAceitarTipoInvalido()
        {
            using var context = CriarContexto();

            int usuarioId = await CriarUsuario(context);

            var service = new OcorrenciaService(context);

            var dados = new OcorrenciaCadastroDto
            {
                Tipo = "Roubo",
                Descricao = "Descricao de teste",
                Localizacao = "Sao Paulo",
                DataOcorrencia = DateTime.UtcNow,
                UsuarioId = usuarioId
            };

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CadastrarAsync(dados)
            );
        }

        [Fact]
        public async Task CadastrarAsync_NaoDeveAceitarUsuarioInexistente()
        {
            using var context = CriarContexto();

            var service = new OcorrenciaService(context);

            var dados = new OcorrenciaCadastroDto
            {
                Tipo = "Encontrado",
                Descricao = "Animal encontrado",
                Localizacao = "Sao Paulo",
                DataOcorrencia = DateTime.UtcNow,
                UsuarioId = 999
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => service.CadastrarAsync(dados)
            );
        }
    }
}
