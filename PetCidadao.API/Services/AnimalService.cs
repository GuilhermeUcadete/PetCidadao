
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Data;
using PetCidadao.API.DTOs;
using PetCidadao.API.Interfaces;
using PetCidadao.API.Models;

namespace PetCidadao.API.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly PetCidadaoContext _context;

        public AnimalService(PetCidadaoContext context)
        {
            _context = context;
        }

        public async Task<AnimalRespostaDto> CadastrarAsync(AnimalCadastroDto dados)
        {
            bool usuarioExiste = await _context.Usuarios
                .AnyAsync(u => u.Id == dados.UsuarioId);

            if (!usuarioExiste)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            string? codigoRfid = string.IsNullOrWhiteSpace(dados.CodigoRfid)
                ? null
                : dados.CodigoRfid.Trim();

            if (codigoRfid != null)
            {
                bool rfidExiste = await _context.Animais
                    .AnyAsync(a => a.CodigoRfid == codigoRfid);

                if (rfidExiste)
                {
                    throw new InvalidOperationException(
                        "Este código RFID já está cadastrado."
                    );
                }
            }

            var animal = new Animal
            {
                Nome = dados.Nome.Trim(),
                Especie = dados.Especie.Trim(),
                Raca = dados.Raca.Trim(),
                Sexo = dados.Sexo.Trim(),
                Idade = dados.Idade,
                Cor = dados.Cor.Trim(),
                Descricao = dados.Descricao.Trim(),
                FotoUrl = dados.FotoUrl,
                CodigoRfid = codigoRfid,
                UsuarioId = dados.UsuarioId
            };

            _context.Animais.Add(animal);

            await _context.SaveChangesAsync();

            return ConverterParaDto(animal);
        }

        public async Task<List<AnimalRespostaDto>> ListarAsync()
        {
            var animais = await _context.Animais
                .AsNoTracking()
                .ToListAsync();

            return animais.Select(ConverterParaDto).ToList();
        }

        public async Task<AnimalRespostaDto?> BuscarPorIdAsync(int id)
        {
            var animal = await _context.Animais
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
            {
                return null;
            }

            return ConverterParaDto(animal);
        }

        public async Task<AnimalRespostaDto?> BuscarPorRfidAsync(string codigoRfid)
        {
            string codigo = codigoRfid.Trim();

            var animal = await _context.Animais
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.CodigoRfid == codigo);

            if (animal == null)
            {
                return null;
            }

            return ConverterParaDto(animal);
        }

        private static AnimalRespostaDto ConverterParaDto(Animal animal)
        {
            return new AnimalRespostaDto
            {
                Id = animal.Id,
                Nome = animal.Nome,
                Especie = animal.Especie,
                Raca = animal.Raca,
                Sexo = animal.Sexo,
                Idade = animal.Idade,
                Cor = animal.Cor,
                Descricao = animal.Descricao,
                FotoUrl = animal.FotoUrl,
                CodigoRfid = animal.CodigoRfid,
                UsuarioId = animal.UsuarioId,
                DataCadastro = animal.DataCadastro
            };
        }
    }
}
