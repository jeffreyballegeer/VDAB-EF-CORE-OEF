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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            "Server=.\\SQLExpress;Database=EFBank;" +
            "Trusted_Connection=true;encrypt=false",    //trusted connection = windowsuser als login
            options => options.MaxBatchSize(150));      // # sql commands die in 1* naar de db gestuurd kunnen worden
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
