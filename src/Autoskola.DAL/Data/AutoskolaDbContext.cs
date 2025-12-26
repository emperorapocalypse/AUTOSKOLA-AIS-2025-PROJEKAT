using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Autoskola.DAL.Models;

namespace Autoskola.DAL.Data
{
    public class AutoskolaDbContext : IdentityDbContext<ApplicationUser>
    {
        public AutoskolaDbContext(DbContextOptions<AutoskolaDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Kandidat> Kandidati { get; set; }
        public DbSet<Instruktor> Instruktori { get; set; }
        public DbSet<Vozilo> Vozila { get; set; }
        public DbSet<VoziloSlika> VoziloSlike { get; set; }
        public DbSet<Cas> Casovi { get; set; }
        public DbSet<Ispit> Ispiti { get; set; }
        public DbSet<KandidatCas> KandidatCasovi { get; set; }
        public DbSet<KandidatIspit> KandidatIspiti { get; set; }
        public DbSet<IspitVozilo> IspitVozila { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<KandidatCas>()
                .HasKey(kc => new { kc.KandidatId, kc.CasId });

            modelBuilder.Entity<KandidatCas>()
                .HasOne(kc => kc.Kandidat)
                .WithMany(k => k.KandidatCasovi)
                .HasForeignKey(kc => kc.KandidatId);

            modelBuilder.Entity<KandidatCas>()
                .HasOne(kc => kc.Cas)
                .WithMany(c => c.KandidatCasovi)
                .HasForeignKey(kc => kc.CasId);

            modelBuilder.Entity<KandidatIspit>()
                .HasKey(ki => new { ki.KandidatId, ki.IspitId });

            modelBuilder.Entity<KandidatIspit>()
                .HasOne(ki => ki.Kandidat)
                .WithMany(k => k.KandidatIspiti)
                .HasForeignKey(ki => ki.KandidatId);

            modelBuilder.Entity<KandidatIspit>()
                .HasOne(ki => ki.Ispit)
                .WithMany(i => i.KandidatIspiti)
                .HasForeignKey(ki => ki.IspitId);

            modelBuilder.Entity<IspitVozilo>()
                .HasKey(iv => new { iv.IspitId, iv.VoziloId });

            modelBuilder.Entity<IspitVozilo>()
                .HasOne(iv => iv.Ispit)
                .WithMany(i => i.IspitVozila)
                .HasForeignKey(iv => iv.IspitId);

            modelBuilder.Entity<IspitVozilo>()
                .HasOne(iv => iv.Vozilo)
                .WithMany(v => v.IspitVozila)
                .HasForeignKey(iv => iv.VoziloId);

            
            SeedRolesAndAdmin(modelBuilder);
        }

        private void SeedRolesAndAdmin(ModelBuilder modelBuilder)
        {
            
        }
    }
}