using AnagraficaAPI.Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Poseidon.AuthorizationManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TreePlat.Domain.Constants;
using TreePlat.Domain.Enumerations;

namespace AnagraficaAPI.Entity.Context
{
    public class AnagraficaContext : DbContext
    {
        public virtual DbSet<Cliente> Clienti { get; set; }
        public virtual DbSet<Indirizzo> Indirizzi { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<RelClientiTags> RelClientiTags { get; set; }
        public virtual DbSet<DocumentoCliente> DocumentoCliente { get; set; }
        public virtual DbSet<Documento> Documenti { get; set; }

        private readonly IAuthorizationManager _authorizationManager;

#pragma warning disable IDE0060 // Remove unused parameter
        public int LevenshteinMethod(string field, string search, int max) => throw new NotSupportedException();
#pragma warning restore IDE0060 // Remove unused parameter

        public AnagraficaContext() : base() { }

        public AnagraficaContext(DbContextOptions<AnagraficaContext> options, IAuthorizationManager authorizationManager) : base(options)
        {
            _authorizationManager = authorizationManager;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDbFunction(typeof(AnagraficaContext).GetMethod(nameof(LevenshteinMethod), new[] { typeof(string), typeof(string), typeof(int) }))
                .HasName("Levenshtein");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("IDCliente");
                entity.Property(e => e.Created).HasColumnName("DataInserimento").HasColumnType("datetime");
                entity.Property(e => e.Modified).HasColumnName("DataModifica").HasColumnType("datetime").IsRequired(false);
                entity.HasOne(e => e.Indirizzo).WithOne(e => e.Cliente).HasForeignKey<Indirizzo>(b => b.Id);
                entity.HasMany(d => d.DocumentoCliente).WithOne(p => p.Cliente).HasForeignKey(d => d.IdCliente);
                entity.HasQueryFilter(x => x.IdAgenzia == _authorizationManager.GetIdAgenziaFilter(EPrivilegioPoseidon.Cliente_Visualizzazione, null));
            });

            modelBuilder.Entity<Indirizzo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("IDIndirizzo");
                entity.Property(e => e.Created).HasColumnName("DataInserimento").HasColumnType("datetime");
                entity.Property(e => e.Modified).HasColumnName("DataModifica").HasColumnType("datetime").IsRequired(false);
                entity.HasOne(e => e.Cliente).WithOne(e => e.Indirizzo).HasForeignKey<Cliente>(b => b.IdIndirizzo);
                entity.HasQueryFilter(x => x.Cliente.IdAgenzia == _authorizationManager.GetIdAgenziaFilter(EPrivilegioPoseidon.Cliente_Visualizzazione, null));
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("IDTag");
                entity.Property(e => e.Created).HasColumnName("DataInserimento").HasColumnType("datetime");
                entity.Property(e => e.Modified).HasColumnName("DataModifica").HasColumnType("datetime").IsRequired(false);
                entity.HasQueryFilter(x => x.IdAgenzia == _authorizationManager.GetIdAgenziaFilter(null, null));
            });

            modelBuilder.Entity<RelClientiTags>(entity =>
            {
                entity.HasKey(e => new { e.IdCliente, e.IdTag });
                entity.Property(e => e.Created).HasColumnName("DataInserimento").HasColumnType("datetime");
                entity.HasOne(d => d.Cliente).WithMany(p => p.RelClientiTags).HasForeignKey(d => d.IdCliente);
                entity.HasOne(d => d.Tag).WithMany(p => p.RelClientiTags).HasForeignKey(d => d.IdTag);
                entity.HasQueryFilter(x => x.Cliente.IdAgenzia == _authorizationManager.GetIdAgenziaFilter(EPrivilegioPoseidon.Cliente_Visualizzazione, null));
            });

            modelBuilder.Entity<DocumentoCliente>(entity =>
            {
                entity.HasOne(d => d.Cliente).WithMany(p => p.DocumentoCliente).HasForeignKey(d => d.IdCliente);
                entity.HasOne(d => d.Documento).WithOne(p => p.DocumentoCliente).HasForeignKey<DocumentoCliente>(d => d.IdDocumento);
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.Property(e => e.Created).HasColumnName("DataInserimento").HasColumnType("datetime").IsRequired(false);
                entity.HasOne(d => d.DocumentoCliente).WithOne(p => p.Documento);
            });
        }

        public virtual void SetRowVersionValue(BaseEntity entity, byte[] rowVersion)
        {
            var entryProp = Entry(entity).Property(u => u.RowVersion);
            entryProp.OriginalValue = rowVersion;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            Audit();
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return ApiErrorCode.DbUpdateConcurrencyException;
            }
        }

        private void Audit()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker?.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
    }
}
