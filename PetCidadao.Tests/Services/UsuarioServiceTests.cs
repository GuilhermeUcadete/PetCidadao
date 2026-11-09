
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Services;

namespace PetCidadao.Tests.Services
{
    public class UsuarioServiceTests
    {
        private PetCidadaoContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<PetCidadaoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PetCidadaoContext(options);
        }

        [Fact]
        public async Task CadastrarAsync_DeveCadastrarUsuarioComSucesso()
        {
            using var context = CriarContexto();

            var service = new UsuarioService(context);

            var dados = new UsuarioCadastroDto
            {
                Nome = "Joao Silva",
                Email = "joao@email.com",
                Senha = "Senha123",
                Telefone = "11999999999",
                TipoUsuario = "Cidadao",
                Endereco = "Sao Bernardo do Campo"
            };

            var resultado = await service.CadastrarAsync(dados);

            Assert.Equal("Joao Silva", resultado.Nome);
            Assert.Equal("joao@email.com", resultado.Email);
            Assert.Equal(1, resultado.Id);

            var usuarioSalvo = await context.Usuarios.FirstAsync();

            Assert.NotEqual("Senha123", usuarioSalvo.SenhaHash);
            Assert.NotEmpty(usuarioSalvo.SenhaHash);
        }

        [Fact]
        public async Task CadastrarAsync_NaoDevePermitirEmailDuplicado()
        {
            using var context = CriarContexto();

            var service = new UsuarioService(context);

            var dados = new UsuarioCadastroDto
            {
                Nome = "Joao Silva",
                Email = "joao@email.com",
                Senha = "Senha123",
                Telefone = "11999999999",
                TipoUsuario = "Cidadao"
            };

            await service.CadastrarAsync(dados);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CadastrarAsync(dados)
            );
        }
    }
}
