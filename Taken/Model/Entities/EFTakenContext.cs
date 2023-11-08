using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EFTakenContext : DbContext
    {
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Rekening> Rekeningen { get; set; }

        public DbSet<Personeelslid> Personeelsleden { get; set; }

        public DbSet<Artikelgroep> Artikelgroepen { get; set; }
        public DbSet<Artikel> Artikels { get; set; }
        public DbSet<NonFoodArtikel> NonFoodArtikels { get; set; }
        public DbSet<FoodArtikel> FoodArtikels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            "Server=.\\SQLExpress;Database=EFBank;" +
            "Trusted_Connection=true;encrypt=false",    //trusted connection = windowsuser als login
            options => options.MaxBatchSize(150));      // # sql commands die in 1* naar de db gestuurd kunnen worden
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Artikel_Discriminator
            modelBuilder.Entity<Artikel>()
                        .ToTable("Artikels")
                        .HasDiscriminator<string>("ArtikelType")
                        .HasValue<FoodArtikel>("F")
                        .HasValue<NonFoodArtikel>("N");
            #endregion

            #region Personeelslid_hasdata
            modelBuilder.Entity<Personeelslid>().HasData
            (
                new Personeelslid { PersoneelsNr = 01, Voornaam = "Diane" },
                new Personeelslid { PersoneelsNr = 02, Voornaam = "Mary", ManagerNr = 1 },
                new Personeelslid { PersoneelsNr = 03, Voornaam = "Jeff", ManagerNr = 1 },
                new Personeelslid { PersoneelsNr = 04, Voornaam = "William", ManagerNr = 2 },
                new Personeelslid { PersoneelsNr = 05, Voornaam = "Gerard", ManagerNr = 2 },
                new Personeelslid { PersoneelsNr = 06, Voornaam = "Anthony", ManagerNr = 2 },
                new Personeelslid { PersoneelsNr = 07, Voornaam = "Leslie", ManagerNr = 6 },
                new Personeelslid { PersoneelsNr = 08, Voornaam = "July", ManagerNr = 6 },
                new Personeelslid { PersoneelsNr = 09, Voornaam = "Steve", ManagerNr = 6 },
                new Personeelslid { PersoneelsNr = 10, Voornaam = "Foon Yue", ManagerNr = 6 },
                new Personeelslid { PersoneelsNr = 11, Voornaam = "George", ManagerNr = 6 },
                new Personeelslid { PersoneelsNr = 12, Voornaam = "Loui", ManagerNr = 5 },
                new Personeelslid { PersoneelsNr = 13, Voornaam = "Pamela", ManagerNr = 5 },
                new Personeelslid { PersoneelsNr = 14, Voornaam = "Larry", ManagerNr = 5 },
                new Personeelslid { PersoneelsNr = 15, Voornaam = "Barry", ManagerNr = 5 },
                new Personeelslid { PersoneelsNr = 16, Voornaam = "Andy", ManagerNr = 4 },
                new Personeelslid { PersoneelsNr = 17, Voornaam = "Peter", ManagerNr = 4 },
                new Personeelslid { PersoneelsNr = 18, Voornaam = "Tom", ManagerNr = 4 },
                new Personeelslid { PersoneelsNr = 19, Voornaam = "Mami", ManagerNr = 2 },
                new Personeelslid { PersoneelsNr = 20, Voornaam = "Yoshimi", ManagerNr = 19 },
                new Personeelslid { PersoneelsNr = 21, Voornaam = "Martin", ManagerNr = 5 }
            );
            #endregion

            #region Klant_hasdata
            modelBuilder.Entity<Klant>()
            .HasData(
                new Klant
                {
                    KlantNr = 1,
                    Voornaam = "Marge"
                },
                new Klant
                {
                    KlantNr = 2,
                    Voornaam = "Homer"
                },
                new Klant
                {
                    KlantNr = 3,
                    Voornaam = "Lisa"
                },
                new Klant
                {
                    KlantNr = 4,
                    Voornaam = "Maggie"
                },
                new Klant
                {
                    KlantNr = 5,
                    Voornaam = "Simpson"
                }
            );
            #endregion

            #region Rekening_hasdata
            modelBuilder.Entity<Rekening>()
            .HasData(
                new Rekening
                {
                    RekeningNr = "123-4567890-02",
                    KlantNr = 1,
                    Saldo = 1000,
                    Soort = 'Z'
                },
                new Rekening
                {
                    RekeningNr = "234-5678901-69",
                    KlantNr = 1,
                    Saldo = 2000,
                    Soort = 'S'
                },
                new Rekening
                {
                    RekeningNr = "345-6789012-12",
                    KlantNr = 2,
                    Saldo = 1000,
                    Soort = 'S'
                }
            );
            #endregion
        }
    }
}
