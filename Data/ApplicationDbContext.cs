using AgenceLocationVoiture.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgenceLocationVoiture.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilisateur>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Agence> Agences { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Voiture> Voitures { get; set; }
        public DbSet<FicheTechnique> FichesTechniques { get; set; }
        public DbSet<OffreLoc> OffresLocation { get; set; }
        public DbSet<DemandeLoc> DemandesLocation { get; set; }
        public DbSet<Avis> Avis { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Utilisateur>()
                .HasDiscriminator<string>("TypeUtilisateur")
                .HasValue<Utilisateur>("Utilisateur")
                .HasValue<Agence>("Agence")
                .HasValue<Client>("Client")
                .HasValue<Admin>("Admin"); 

            modelBuilder.Entity<Voiture>()
                .HasOne(v => v.Agence)
                .WithMany(a => a.Voitures)
                .HasForeignKey(v => v.AgenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voiture>()
                .HasOne(v => v.FicheTechnique)
                .WithOne(f => f.Voiture)
                .HasForeignKey<Voiture>(v => v.FicheTechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OffreLoc>()
                .HasOne(o => o.Voiture)
                .WithMany(v => v.OffresLocation)
                .HasForeignKey(o => o.VoitureId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OffreLoc>()
                .HasOne(o => o.Agence)
                .WithMany(a => a.OffresLocation)
                .HasForeignKey(o => o.AgenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DemandeLoc>()
                .HasOne(d => d.Client)
                .WithMany(c => c.DemandesLocation)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DemandeLoc>()
                .HasOne(d => d.OffreLoc)
                .WithMany(o => o.Demandes)
                .HasForeignKey(d => d.OffreLocId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Client)
                .WithMany(c => c.AvisDonnes)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Agence)
                .WithMany(ag => ag.AvisRecus)
                .HasForeignKey(a => a.AgenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Voiture>()
                .Property(v => v.Kilometrage)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OffreLoc>()
                .Property(o => o.PrixParJour)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OffreLoc>()
                .Property(o => o.PrixParSemaine)
                .HasPrecision(10, 2);

            modelBuilder.Entity<FicheTechnique>()
                .Property(f => f.Consommation)
                .HasPrecision(5, 2);

            modelBuilder.Entity<FicheTechnique>()
                .Property(f => f.EmissionCO2)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Voiture>()
                .HasIndex(v => v.Marque);

            modelBuilder.Entity<Voiture>()
                .HasIndex(v => v.EstDisponible);

            modelBuilder.Entity<OffreLoc>()
                .HasIndex(o => o.EstActive);

            modelBuilder.Entity<DemandeLoc>()
                .HasIndex(d => d.Statut);

            modelBuilder.Entity<Agence>()
                .HasIndex(a => a.NumeroSiret)
                .IsUnique();
        }
    }
}
