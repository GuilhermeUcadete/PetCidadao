
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly PetCidadaoContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(PetCidadaoContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        public async Task<UsuarioRespostaDto> CadastrarAsync(UsuarioCadastroDto dados)
        {
            string email = dados.Email.Trim().ToLowerInvariant();

            bool emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == email);

            if (emailExiste)
            {
                throw new InvalidOperationException("Este e-mail já está cadastrado.");
            }

            var usuario = new Usuario
            {
                Nome = dados.Nome.Trim(),
                Email = email,
                Telefone = dados.Telefone.Trim(),
                TipoUsuario = dados.TipoUsuario.Trim(),
                Endereco = dados.Endereco
            };

            usuario.SenhaHash = _passwordHasher.HashPassword(
                usuario,
                dados.Senha
            );

            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return ConverterParaDto(usuario);
        }

        public async Task<List<UsuarioRespostaDto>> ListarAsync()
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .ToListAsync();

            return usuarios.Select(ConverterParaDto).ToList();
        }

        private static UsuarioRespostaDto ConverterParaDto(Usuario usuario)
        {
            return new UsuarioRespostaDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                TipoUsuario = usuario.TipoUsuario,
                Endereco = usuario.Endereco,
                DataCadastro = usuario.DataCadastro
            };
        }
    }
}
