
using Microsoft.EntityFrameworkCore;
using PetCidadao.API.Models;

namespace PetCidadao.API.Data
{
    public class PetCidadaoContext : DbContext
    {
        public PetCidadaoContext(DbContextOptions<PetCidadaoContext> options)
            : base(options)
        {
        }
        public DbSet<CorrespondenciaAnimal> CorrespondenciasAnimais { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Animal> Animais { get; set; }

        public DbSet<Ocorrencia> Ocorrencias { get; set; }

        public DbSet<Adocao> Adocoes { get; set; }

        public DbSet<Denuncia> Denuncias { get; set; }

        public DbSet<SolicitacaoAdocao> SolicitacoesAdocao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CorrespondenciaAnimal>()
                .Property(c => c.PercentualCompatibilidade)
                .HasPrecision(5, 2);

            modelBuilder.Entity<CorrespondenciaAnimal>()
                .HasOne(c => c.OcorrenciaPerdida)
                .WithMany()
                .HasForeignKey(c => c.OcorrenciaPerdidaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CorrespondenciaAnimal>()
                .HasOne(c => c.OcorrenciaEncontrada)
                .WithMany()
                .HasForeignKey(c => c.OcorrenciaEncontradaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SolicitacaoAdocao>()
                .HasOne(s => s.Adocao)
                .WithMany()
                .HasForeignKey(s => s.AdocaoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SolicitacaoAdocao>()
                .HasOne(s => s.Usuario)
                .WithMany()
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Animais)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ocorrencia>()
                .HasOne(o => o.Animal)
                .WithMany()
                .HasForeignKey(o => o.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ocorrencia>()
                .HasOne(o => o.Usuario)
                .WithMany()
                .HasForeignKey(o => o.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Adocao>()
                .HasOne(a => a.Animal)
                .WithMany()
                .HasForeignKey(a => a.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Adocao>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Denuncia>()
                .HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Animal>()
                .HasIndex(a => a.CodigoRfid)
                .IsUnique()
                .HasFilter("[CodigoRfid] IS NOT NULL");
        }
    }
}
