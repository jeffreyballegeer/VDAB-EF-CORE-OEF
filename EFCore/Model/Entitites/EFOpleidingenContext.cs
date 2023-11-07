using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class EFOpleidingenContext : DbContext
    {
        public static IConfigurationRoot configuration;
        public DbSet<Campus> Campussen { get; set; }
        public DbSet<Docent> Docenten { get; set; }
        public DbSet<Land> Landen { get; set; }

        //TPH : Table Per Hierarchy
        public DbSet<TPHCursus> TPHCursussen { get; set; }
        public DbSet<TPHZelfstudieCursus> TPHZelfstudieCursussen { get; set; }
        public DbSet<TPHKlassikaleCursus> TPHKlassikaleCursussen { get; set; }

        //TPT : Table Per Type
        public DbSet<TPTCursus> TPTCursussen { get; set; }
        public DbSet<TPTZelfstudieCursus> TPTZelfstudieCursussen { get; set; }
        public DbSet<TPTKlassikaleCursus> TPTKlassikaleCursussen { get; set; }

        //TPC : Table Per Concrete class
        public DbSet<TPCCursus> TPCCursussen { get; set; }
        public DbSet<TPCZelfstudieCursus> TPCZelfstudieCursussen { get; set; }
        public DbSet<TPCKlassikaleCursus> TPCKlassikaleCursussen { get; set; }

        public DbSet<Boek> Boeken { get; set; }
        public DbSet<Cursus> Cursussen { get; set; }

        public DbSet<Activiteit> Activiteiten { get; set; }
        public DbSet<DocentActiviteit> DocentenActiviteiten { get; set; }

        public DbSet<Werknemer> Werknemers { get; set; }

        // /* CONNECTIONSTRING HARDCODED : */
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //    "Server=.\\SQLExpress;Database=EFOpleidingen;" +
        //    "Trusted_Connection=true;encrypt=false",    //trusted connection = windowsgebruiker als login
        //    options => options.MaxBatchSize(150));      // # sql commands die in 1* naar de db gestuurd kunnen worden
        //}

        /* Connectionstring in configfile */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName) // locatie van config bestand
            .AddJsonFile("appsettings.json", false).Build();        // naam van config bestand
            var connectionString =
            configuration.GetConnectionString("EFOpleidingen");     // haal connectiestring uit configfile
            if (connectionString != null)
                optionsBuilder
                    .UseSqlServer(connectionString, options => options.MaxBatchSize(150));
            //optionsBuilder.LogTo(Console.WriteLine);              // log sql query to console
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Docent
            modelBuilder.Entity<Docent>()
                        .HasOne(d => d.Land)
                        .WithMany(l => l.Docenten)
                        .OnDelete(DeleteBehavior.SetNull);
            #endregion

            #region Adres_owned_type
            //class "Adres" should not be an entity but consists of "owned types"
            modelBuilder.Entity<Campus>().OwnsOne(s => s.Adres);
            modelBuilder.Entity<Campus>().OwnsOne(s => s.Adres)
                                            .Property(b => b.Straat)
                                            .HasColumnName("Straat");
            modelBuilder.Entity<Campus>().OwnsOne(s => s.Adres)
                                            .Property(b => b.Huisnummer)
                                            .HasColumnName("HuisNr");
            modelBuilder.Entity<Campus>().OwnsOne(s => s.Adres)
                                            .Property(b => b.Postcode)
                                            .HasColumnName("PostCd");
            modelBuilder.Entity<Campus>().OwnsOne(s => s.Adres)
                                            .Property(b => b.Gemeente)
                                            .HasColumnName("Gemeente");

            modelBuilder.Entity<Docent>().OwnsOne(s => s.ThuisAdres);
            modelBuilder.Entity<Docent>().OwnsOne(s => s.ThuisAdres)
                                            .Property(b => b.Gemeente)
                                            .HasColumnName("GemeenteThuis");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.ThuisAdres)
                                            .Property(b => b.Huisnummer)
                                            .HasColumnName("HuisNrThuis");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.ThuisAdres)
                                            .Property(b => b.Postcode)
                                            .HasColumnName("PostCdThuis");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.ThuisAdres)
                                            .Property(b => b.Straat)
                                            .HasColumnName("StraatThuis");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.VerblijfsAdres);
            modelBuilder.Entity<Docent>().OwnsOne(s => s.VerblijfsAdres)
                                            .Property(b => b.Gemeente)
                                            .HasColumnName("GemeenteVerblijf");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.VerblijfsAdres)
                                            .Property(b => b.Huisnummer)
                                            .HasColumnName("HuisNrVerblijf");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.VerblijfsAdres)
                                            .Property(b => b.Postcode)
                                            .HasColumnName("PostCdVerblijf");
            modelBuilder.Entity<Docent>().OwnsOne(s => s.VerblijfsAdres)
                                            .Property(b => b.Straat)
                                            .HasColumnName("StraatVerblijf");
            #endregion

            #region TPHCursus_Discriminator_Field
            modelBuilder.Entity<TPHCursus>()
                        .ToTable("TPHCursussen")
                        .HasDiscriminator<string>("CursusType")
                        .HasValue<TPHKlassikaleCursus>("K")
                        .HasValue<TPHZelfstudieCursus>("Z");
            #endregion

            #region TPT_Mapping
            modelBuilder.Entity<TPTCursus>().UseTptMappingStrategy();
            //optional 
            modelBuilder.Entity<TPTCursus>().ToTable("TPT");
            modelBuilder.Entity<TPTZelfstudieCursus>().ToTable("TPTZelfstudie");
            modelBuilder.Entity<TPTKlassikaleCursus>().ToTable("TPTKlassikaal");
            #endregion

            #region TPC_Mapping
            modelBuilder.Entity<TPCCursus>().UseTpcMappingStrategy();
            //optional
            modelBuilder.Entity<TPCZelfstudieCursus>().ToTable("TPCZelfstudie");
            modelBuilder.Entity<TPCKlassikaleCursus>().ToTable("TPCKlassikaal");
            #endregion

            #region Cursus_Boek
            modelBuilder.Entity<Cursus>().Property(b => b.Naam) // (1) 
                        .IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Boek>().Property(b => b.Titel)
            .HasMaxLength(150);
            modelBuilder.Entity<Boek>().Property(b => b.IsbnNr)
            .IsRequired().HasMaxLength(13);
            modelBuilder.Entity<Boek>(
            b => { b.HasIndex(e => e.IsbnNr).IsUnique(); }); // (2) 
            #endregion

            #region DocentAktiviteit
            //we define the relation between docent & aktiviteit using fluent-api:
            modelBuilder.Entity<DocentActiviteit>()
                        .HasKey(c => new { c.DocentId, c.ActiviteitId });
            modelBuilder.Entity<DocentActiviteit>()
                        .HasOne(x => x.Docent)
                        .WithMany(y => y.DocentenActiviteiten)
                        .HasForeignKey(x => x.DocentId);
            modelBuilder.Entity<DocentActiviteit>()
                        .HasOne(x => x.Activiteit)
                        .WithMany(y => y.DocentenActiviteiten)
                        .HasForeignKey(x => x.ActiviteitId);
            #endregion

            #region Boek_HasData
            modelBuilder.Entity<Boek>().HasData(
            new Boek
            {
                BoekNr = 1,
                IsbnNr = "0-0705918-0-6",
                Titel = "C++ : For Scientists and Engineers"
            },
            new Boek
            {
                BoekNr = 2,
                IsbnNr = "0-0788212-3-1",
                Titel = "C++ : The Complete Reference"
            },
            new Boek
            {
                BoekNr = 3,
                IsbnNr = "1-5659211-6-X",
                Titel = "C++ : The Core Language"
            },
            new Boek
            {
                BoekNr = 4,
                IsbnNr = "0-4448771-8-5",
                Titel = "Relational Database Systems"
            },
            new Boek
            {
                BoekNr = 5,
                IsbnNr = "1-5595851-1-0",
                Titel = "Access from the Ground Up"
            },
            new Boek
            {
                BoekNr = 6,
                IsbnNr = "0-0788212-2-3",
                Titel = "Oracle : A Beginner''s Guide"
            },
            new Boek
            {
                BoekNr = 7,
                IsbnNr = "0-0788209-7-9",
                Titel = "Oracle : The Complete Reference"
            });
            #endregion

            #region Cursus_HasData
            modelBuilder.Entity<Cursus>().HasData(
                new Cursus { CursusNr = 1, Naam = "C++" },
                new Cursus { CursusNr = 2, Naam = "Access" },
                new Cursus { CursusNr = 3, Naam = "Oracle" });
            #endregion

            #region Boek_Cursus_HasData
            modelBuilder.Entity<Boek>()
                        .HasMany(p => p.Cursussen)
                        .WithMany(p => p.Boeken)
                        .UsingEntity(j => j.HasData(
                            new { CursussenCursusNr = 1, BoekenBoekNr = 1 },
                            new { CursussenCursusNr = 1, BoekenBoekNr = 2 },
                            new { CursussenCursusNr = 1, BoekenBoekNr = 3 },
                            new { CursussenCursusNr = 2, BoekenBoekNr = 4 },
                            new { CursussenCursusNr = 2, BoekenBoekNr = 5 },
                            new { CursussenCursusNr = 3, BoekenBoekNr = 4 },
                            new { CursussenCursusNr = 3, BoekenBoekNr = 6 },
                            new { CursussenCursusNr = 3, BoekenBoekNr = 7 }
                        ));
            #endregion

            #region Werknemer
            modelBuilder.Entity<Werknemer>()
                        .HasOne(x => x.Overste)
                        .WithMany(y => y.Ondergeschikten)
                        .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region Werknemer_HasData
            //some employees
            var werknemers = new List<Werknemer>()
            {
                new Werknemer { WerknemerId = 03, Voornaam = "Agustin", Familienaam = "Calleri" },
                new Werknemer { WerknemerId = 04, Voornaam = "Ai", Familienaam = "Sugiyama" },
                new Werknemer { WerknemerId = 05, Voornaam = "Akgul", Familienaam = "Amanmuradova" },
                new Werknemer { WerknemerId = 06, Voornaam = "Albert", Familienaam = "Montanes" },
                new Werknemer { WerknemerId = 07, Voornaam = "Alberto", Familienaam = "Martin" },
                new Werknemer { WerknemerId = 08, Voornaam = "Aleksandra", Familienaam = "Wozniak" },
                new Werknemer { WerknemerId = 09, Voornaam = "Alisa", Familienaam = "Kleybanova" },
                new Werknemer { WerknemerId = 10, Voornaam = "Alize", Familienaam = "Cornet" },
                new Werknemer { WerknemerId = 11, Voornaam = "Alla", Familienaam = "Kudryavtseva" },
                new Werknemer { WerknemerId = 12, Voornaam = "Alona", Familienaam = "Bondarenko" },
                new Werknemer { WerknemerId = 13, Voornaam = "Amelie", Familienaam = "Mauresmo" },
                new Werknemer { WerknemerId = 14, Voornaam = "Ana", Familienaam = "Ivanovic" },
                new Werknemer { WerknemerId = 15, Voornaam = "Anabel", Familienaam = "Medina Garrigues" },
                new Werknemer { WerknemerId = 16, Voornaam = "Anastasia", Familienaam = "Pavlyuchenkova" },
                new Werknemer { WerknemerId = 17, Voornaam = "Anastasiya", Familienaam = "Yakimova" },
                new Werknemer { WerknemerId = 18, Voornaam = "Andreas", Familienaam = "Beck" },
                new Werknemer { WerknemerId = 19, Voornaam = "Andreas", Familienaam = "Seppi" },
                new Werknemer { WerknemerId = 20, Voornaam = "Andy", Familienaam = "Murray" },
                new Werknemer { WerknemerId = 21, Voornaam = "Andy", Familienaam = "Roddick" },
                new Werknemer { WerknemerId = 22, Voornaam = "Anna", Familienaam = "Chakvetadze" },
                new Werknemer { WerknemerId = 23, Voornaam = "Anna-Lena", Familienaam = "Groenefeld" },
                new Werknemer { WerknemerId = 24, Voornaam = "Anne", Familienaam = "Keothavong" },
                new Werknemer { WerknemerId = 25, Voornaam = "Aravane", Familienaam = "Rezai" },
                new Werknemer { WerknemerId = 26, Voornaam = "Arnaud", Familienaam = "Clement" },
                new Werknemer { WerknemerId = 27, Voornaam = "Ayumi", Familienaam = "Morita" },
                new Werknemer { WerknemerId = 28, Voornaam = "Barbora", Familienaam = "Zahlavova Strycova" },
                new Werknemer { WerknemerId = 29, Voornaam = "Bethanie", Familienaam = "Mattek-Sands" },
                new Werknemer { WerknemerId = 30, Voornaam = "Bjorn", Familienaam = "Phau" },
                new Werknemer { WerknemerId = 31, Voornaam = "Bobby", Familienaam = "Reynolds" },
                new Werknemer { WerknemerId = 32, Voornaam = "Brian", Familienaam = "Dabul" },
                new Werknemer { WerknemerId = 33, Voornaam = "Camille", Familienaam = "Pin" },
                new Werknemer { WerknemerId = 34, Voornaam = "Carla", Familienaam = "Suarez Navarro" },
                new Werknemer { WerknemerId = 35, Voornaam = "Carlos", Familienaam = "Moya" },
                new Werknemer { WerknemerId = 36, Voornaam = "Caroline", Familienaam = "Wozniacki" },
                new Werknemer { WerknemerId = 37, Voornaam = "Casey", Familienaam = "Dellacqua" },
                new Werknemer { WerknemerId = 38, Voornaam = "Christophe", Familienaam = "Rochus" },
                new Werknemer { WerknemerId = 39, Voornaam = "Daniel", Familienaam = "Gimeno" },
                new Werknemer { WerknemerId = 40, Voornaam = "Daniela", Familienaam = "Hantuchova" },
                new Werknemer { WerknemerId = 41, Voornaam = "David", Familienaam = "Ferrer" },
                new Werknemer { WerknemerId = 42, Voornaam = "David", Familienaam = "Nalbandian" },
                new Werknemer { WerknemerId = 43, Voornaam = "Denis", Familienaam = "Gremelmayr" },
                new Werknemer { WerknemerId = 44, Voornaam = "Diego", Familienaam = "Junqueira" },
                new Werknemer { WerknemerId = 45, Voornaam = "Dinara", Familienaam = "Safina" },
                new Werknemer { WerknemerId = 46, Voornaam = "Dmitry", Familienaam = "Tursunov" },
                new Werknemer { WerknemerId = 47, Voornaam = "Dominika", Familienaam = "Cibulkova" },
                new Werknemer { WerknemerId = 48, Voornaam = "Dudi", Familienaam = "Sela" },
                new Werknemer { WerknemerId = 49, Voornaam = "Edina", Familienaam = "Gallovits" },
                new Werknemer { WerknemerId = 50, Voornaam = "Eduardo", Familienaam = "Schwank" },
                new Werknemer { WerknemerId = 51, Voornaam = "Ekaterina", Familienaam = "Makarova" },
                new Werknemer { WerknemerId = 52, Voornaam = "Elena", Familienaam = "Dementieva" },
                new Werknemer { WerknemerId = 53, Voornaam = "Elena", Familienaam = "Vesnina" },
                new Werknemer { WerknemerId = 54, Voornaam = "Ernests", Familienaam = "Gulbis" },
                new Werknemer { WerknemerId = 55, Voornaam = "Evgueni", Familienaam = "Korolev" },
                new Werknemer { WerknemerId = 56, Voornaam = "Fabrice", Familienaam = "Santoro" },
                new Werknemer { WerknemerId = 57, Voornaam = "Feliciano", Familienaam = "Lopez" },
                new Werknemer { WerknemerId = 58, Voornaam = "Fernando", Familienaam = "Gonzalez" },
                new Werknemer { WerknemerId = 59, Voornaam = "Fernando", Familienaam = "Verdasco" },
                new Werknemer { WerknemerId = 60, Voornaam = "Flavia", Familienaam = "Pennetta" },
                new Werknemer { WerknemerId = 61, Voornaam = "Florent", Familienaam = "Serra" },
                new Werknemer { WerknemerId = 62, Voornaam = "Francesca", Familienaam = "Schiavone" },
                new Werknemer { WerknemerId = 63, Voornaam = "Frederico", Familienaam = "Gil" },
                new Werknemer { WerknemerId = 64, Voornaam = "Gael", Familienaam = "Monfils" },
                new Werknemer { WerknemerId = 65, Voornaam = "Galina", Familienaam = "Voskoboeva" },
                new Werknemer { WerknemerId = 66, Voornaam = "Gilles", Familienaam = "Muller" },
                new Werknemer { WerknemerId = 67, Voornaam = "Gilles", Familienaam = "Simon" },
                new Werknemer { WerknemerId = 68, Voornaam = "Gisela", Familienaam = "Dulko" },
                new Werknemer { WerknemerId = 69, Voornaam = "Guillermo", Familienaam = "Canas" },
                new Werknemer { WerknemerId = 70, Voornaam = "Guillermo", Familienaam = "Garcia-Lopez" },
                new Werknemer { WerknemerId = 71, Voornaam = "Igor", Familienaam = "Andreev" },
                new Werknemer { WerknemerId = 72, Voornaam = "Igor", Familienaam = "Kunitsyn" },
                new Werknemer { WerknemerId = 73, Voornaam = "Ivan", Familienaam = "Ljubicic" },
                new Werknemer { WerknemerId = 74, Voornaam = "Ivan", Familienaam = "Navarro-Pastor" },
                new Werknemer { WerknemerId = 75, Voornaam = "Iveta", Familienaam = "Benesova" },
                new Werknemer { WerknemerId = 76, Voornaam = "Ivo", Familienaam = "Karlovic" },
                new Werknemer { WerknemerId = 77, Voornaam = "James", Familienaam = "Blake" },
                new Werknemer { WerknemerId = 78, Voornaam = "Jan", Familienaam = "Hernych" },
                new Werknemer { WerknemerId = 79, Voornaam = "Janko", Familienaam = "Tipsarevic" },
                new Werknemer { WerknemerId = 80, Voornaam = "Jarkko", Familienaam = "Nieminen" },
                new Werknemer { WerknemerId = 81, Voornaam = "Jarmila", Familienaam = "Groth" },
                new Werknemer { WerknemerId = 82, Voornaam = "Jelena", Familienaam = "Dokic" },
                new Werknemer { WerknemerId = 83, Voornaam = "Jelena", Familienaam = "Jankovic" },
                new Werknemer { WerknemerId = 84, Voornaam = "Jeremy", Familienaam = "Chardy" },
                new Werknemer { WerknemerId = 85, Voornaam = "Jie", Familienaam = "Zheng" },
                new Werknemer { WerknemerId = 86, Voornaam = "Jose", Familienaam = "Acasuso" },
                new Werknemer { WerknemerId = 87, Voornaam = "Jo-Wilfried", Familienaam = "Tsonga" },
                new Werknemer { WerknemerId = 88, Voornaam = "Juan", Familienaam = "Carlos Ferrero" },
                new Werknemer { WerknemerId = 89, Voornaam = "Juan", Familienaam = "Martin Del Potro" },
                new Werknemer { WerknemerId = 90, Voornaam = "Juan", Familienaam = "Monaco" },
                new Werknemer { WerknemerId = 91, Voornaam = "Julie", Familienaam = "Coin" },
                new Werknemer { WerknemerId = 92, Voornaam = "Julien", Familienaam = "Benneteau" },
                new Werknemer { WerknemerId = 93, Voornaam = "Jurgen", Familienaam = "Melzer" },
                new Werknemer { WerknemerId = 94, Voornaam = "Kaia", Familienaam = "Kanepi" },
                new Werknemer { WerknemerId = 95, Voornaam = "Katarina", Familienaam = "Srebotnik" },
                new Werknemer { WerknemerId = 96, Voornaam = "Kateryna", Familienaam = "Bondarenko" },
                new Werknemer { WerknemerId = 97, Voornaam = "Kei", Familienaam = "Nishikori" },
                new Werknemer { WerknemerId = 98, Voornaam = "Kirsten", Familienaam = "Flipkens" },
                new Werknemer { WerknemerId = 99, Voornaam = "Klara", Familienaam = "Zakopalova" },
                new Werknemer { WerknemerId = 100, Voornaam = "Kristina", Familienaam = "Barrois" },
                new Werknemer { WerknemerId = 101, Voornaam = "Kristof", Familienaam = "Vliegen" },
                new Werknemer { WerknemerId = 102, Voornaam = "Leonardo", Familienaam = "Mayer" },
                new Werknemer { WerknemerId = 103, Voornaam = "Lleyton", Familienaam = "Hewitt" },
                new Werknemer { WerknemerId = 104, Voornaam = "Lourdes", Familienaam = "Dominguez Lino" },
                new Werknemer { WerknemerId = 105, Voornaam = "Lucie", Familienaam = "Hradecka" },
                new Werknemer { WerknemerId = 106, Voornaam = "Lucie", Familienaam = "Safarova" },
                new Werknemer { WerknemerId = 107, Voornaam = "Magdalena", Familienaam = "Rybarikova" },
                new Werknemer { WerknemerId = 108, Voornaam = "Marat", Familienaam = "Safin" },
                new Werknemer { WerknemerId = 109, Voornaam = "Marc", Familienaam = "Gicquel" },
                new Werknemer { WerknemerId = 110, Voornaam = "Marcel", Familienaam = "Granollers" },
                new Werknemer { WerknemerId = 111, Voornaam = "Marcos", Familienaam = "Baghdatis" },
                new Werknemer { WerknemerId = 112, Voornaam = "Mardy", Familienaam = "Fish" },
                new Werknemer { WerknemerId = 113, Voornaam = "Maret", Familienaam = "Ani" },
                new Werknemer { WerknemerId = 114, Voornaam = "Maria", Familienaam = "Jose Martinez Sanchez" },
                new Werknemer { WerknemerId = 115, Voornaam = "Maria", Familienaam = "Kirilenko" },
                new Werknemer { WerknemerId = 116, Voornaam = "Maria", Familienaam = "Sharapova" },
                new Werknemer { WerknemerId = 117, Voornaam = "Marin", Familienaam = "Cilic" },
                new Werknemer { WerknemerId = 118, Voornaam = "Marina", Familienaam = "Erakovic" },
                new Werknemer { WerknemerId = 119, Voornaam = "Mario", Familienaam = "Ancic" },
                new Werknemer { WerknemerId = 120, Voornaam = "Marion", Familienaam = "Bartoli" },
                new Werknemer { WerknemerId = 121, Voornaam = "Mariya", Familienaam = "Koryttseva" },
                new Werknemer { WerknemerId = 122, Voornaam = "Martin", Familienaam = "Vassallo-Arguello" },
                new Werknemer { WerknemerId = 123, Voornaam = "Mathilde", Familienaam = "Johansson" },
                new Werknemer { WerknemerId = 124, Voornaam = "Maximo", Familienaam = "Gonzalez" },
                new Werknemer { WerknemerId = 125, Voornaam = "Melinda", Familienaam = "Czink" },
                new Werknemer { WerknemerId = 126, Voornaam = "Michael", Familienaam = "Llodra" },
                new Werknemer { WerknemerId = 127, Voornaam = "Michael", Familienaam = "Zverev" },
                new Werknemer { WerknemerId = 128, Voornaam = "Mikhail", Familienaam = "Youzhny" },
                new Werknemer { WerknemerId = 129, Voornaam = "Monica", Familienaam = "Niculescu" },
                new Werknemer { WerknemerId = 130, Voornaam = "Na", Familienaam = "Li" },
                new Werknemer { WerknemerId = 131, Voornaam = "Nadia", Familienaam = "Petrova" },
                new Werknemer { WerknemerId = 132, Voornaam = "Nathalie", Familienaam = "Dechy" },
                new Werknemer { WerknemerId = 133, Voornaam = "Nicolas", Familienaam = "Almagro" },
                new Werknemer { WerknemerId = 134, Voornaam = "Nicolas", Familienaam = "Devilder" },
                new Werknemer { WerknemerId = 135, Voornaam = "Nicolas", Familienaam = "Kiefer" },
                new Werknemer { WerknemerId = 136, Voornaam = "Nicolas", Familienaam = "Massu" },
                new Werknemer { WerknemerId = 137, Voornaam = "Nicole", Familienaam = "Vaidisova" },
                new Werknemer { WerknemerId = 138, Voornaam = "Nikolay", Familienaam = "Davydenko" },
                new Werknemer { WerknemerId = 139, Voornaam = "Novak", Familienaam = "Djokovic" },
                new Werknemer { WerknemerId = 140, Voornaam = "Olga", Familienaam = "Govortsova" },
                new Werknemer { WerknemerId = 141, Voornaam = "Oscar", Familienaam = "Hernandez" },
                new Werknemer { WerknemerId = 142, Voornaam = "Pablo", Familienaam = "Andujar" },
                new Werknemer { WerknemerId = 143, Voornaam = "Patricia", Familienaam = "Mayr" },
                new Werknemer { WerknemerId = 144, Voornaam = "Patty", Familienaam = "Schnyder" },
                new Werknemer { WerknemerId = 145, Voornaam = "Paul", Familienaam = "Capdeville" },
                new Werknemer { WerknemerId = 146, Voornaam = "Paul-Henri", Familienaam = "Mathieu" },
                new Werknemer { WerknemerId = 147, Voornaam = "Pauline", Familienaam = "Parmentier" },
                new Werknemer { WerknemerId = 148, Voornaam = "Petra", Familienaam = "Cetkovska" },
                new Werknemer { WerknemerId = 149, Voornaam = "Petra", Familienaam = "Kvitova" },
                new Werknemer { WerknemerId = 150, Voornaam = "Philipp", Familienaam = "Kohlschreiber" },
                new Werknemer { WerknemerId = 151, Voornaam = "Philipp", Familienaam = "Petzschner" },
                new Werknemer { WerknemerId = 152, Voornaam = "Potito", Familienaam = "Starace" },
                new Werknemer { WerknemerId = 153, Voornaam = "Radek", Familienaam = "Stepanek" },
                new Werknemer { WerknemerId = 154, Voornaam = "Rafael", Familienaam = "Nadal" },
                new Werknemer { WerknemerId = 155, Voornaam = "Rainer", Familienaam = "Schuettler" },
                new Werknemer { WerknemerId = 156, Voornaam = "Richard", Familienaam = "Gasquet" },
                new Werknemer { WerknemerId = 157, Voornaam = "Robby", Familienaam = "Ginepri" },
                new Werknemer { WerknemerId = 158, Voornaam = "Robert", Familienaam = "Kendrick" },
                new Werknemer { WerknemerId = 159, Voornaam = "Roberta", Familienaam = "Vinci" },
                new Werknemer { WerknemerId = 160, Voornaam = "Robin", Familienaam = "Soderling" },
                new Werknemer { WerknemerId = 161, Voornaam = "Roger", Familienaam = "Federer" },
                new Werknemer { WerknemerId = 162, Voornaam = "Rossana", Familienaam = "De Los Rios" },
                new Werknemer { WerknemerId = 163, Voornaam = "Sabine", Familienaam = "Lisicki" },
                new Werknemer { WerknemerId = 164, Voornaam = "Samantha", Familienaam = "Stosur" },
                new Werknemer { WerknemerId = 165, Voornaam = "Samuel", Familienaam = "Querrey" },
                new Werknemer { WerknemerId = 166, Voornaam = "Sania", Familienaam = "Mirza" },
                new Werknemer { WerknemerId = 167, Voornaam = "Sara", Familienaam = "Errani" },
                new Werknemer { WerknemerId = 168, Voornaam = "Serena", Familienaam = "Williams" },
                new Werknemer { WerknemerId = 169, Voornaam = "Severine", Familienaam = "Bremond" },
                new Werknemer { WerknemerId = 170, Voornaam = "Shahar", Familienaam = "Peer" },
                new Werknemer { WerknemerId = 171, Voornaam = "Shuai", Familienaam = "Peng" },
                new Werknemer { WerknemerId = 172, Voornaam = "Simone", Familienaam = "Bolelli" },
                new Werknemer { WerknemerId = 173, Voornaam = "Sofia", Familienaam = "Arvidsson" },
                new Werknemer { WerknemerId = 174, Voornaam = "Sorana", Familienaam = "Cirstea" },
                new Werknemer { WerknemerId = 175, Voornaam = "Stanislas", Familienaam = "Wawrinka" },
                new Werknemer { WerknemerId = 176, Voornaam = "Stephanie", Familienaam = "Cohen-Aloro" },
                new Werknemer { WerknemerId = 177, Voornaam = "Svetlana", Familienaam = "Kuznetsova" },
                new Werknemer { WerknemerId = 178, Voornaam = "Sybille", Familienaam = "Bammer" },
                new Werknemer { WerknemerId = 179, Voornaam = "Tamarine", Familienaam = "Tanasugarn" },
                new Werknemer { WerknemerId = 180, Voornaam = "Tamira", Familienaam = "Paszek" },
                new Werknemer { WerknemerId = 181, Voornaam = "Tathiana", Familienaam = "Garbin" },
                new Werknemer { WerknemerId = 182, Voornaam = "Teimuraz", Familienaam = "Gabashvili" },
                new Werknemer { WerknemerId = 183, Voornaam = "Thomaz", Familienaam = "Bellucci" },
                new Werknemer { WerknemerId = 184, Voornaam = "Timea", Familienaam = "Bacsinszky" },
                new Werknemer { WerknemerId = 185, Voornaam = "Tomas", Familienaam = "Berdych" },
                new Werknemer { WerknemerId = 186, Voornaam = "Tommy", Familienaam = "Haas" },
                new Werknemer { WerknemerId = 187, Voornaam = "Tommy", Familienaam = "Robredo" },
                new Werknemer { WerknemerId = 188, Voornaam = "Tsvetana", Familienaam = "Pironkova" },
                new Werknemer { WerknemerId = 189, Voornaam = "Vania", Familienaam = "King" },
                new Werknemer { WerknemerId = 190, Voornaam = "Venus", Familienaam = "Williams" },
                new Werknemer { WerknemerId = 191, Voornaam = "Vera", Familienaam = "Dushevina" },
                new Werknemer { WerknemerId = 192, Voornaam = "Vera", Familienaam = "Zvonareva" },
                new Werknemer { WerknemerId = 193, Voornaam = "Victor", Familienaam = "Hanescu" },
                new Werknemer { WerknemerId = 194, Voornaam = "Victoria", Familienaam = "Azarenka" },
                new Werknemer { WerknemerId = 195, Voornaam = "Viktor", Familienaam = "Troicki" },
                new Werknemer { WerknemerId = 196, Voornaam = "Virginie", Familienaam = "Razzano" },
                new Werknemer { WerknemerId = 197, Voornaam = "Wayne", Familienaam = "Odesnik" },
                new Werknemer { WerknemerId = 198, Voornaam = "Yanina", Familienaam = "Wickmayer" },
                new Werknemer { WerknemerId = 199, Voornaam = "Yen-Hsun", Familienaam = "Lu" },
                new Werknemer { WerknemerId = 200, Voornaam = "Yung-Jan", Familienaam = "Chan" }
            };

            //pseudo random generate an 'overste' for two-thirds of the employees
            foreach (var werknemer in werknemers)
            {
                if (werknemer.WerknemerId % 3 != 0)
                {
                    werknemer.OversteId = (1 + ((werknemer.Voornaam.Length * werknemer.Familienaam.Length) % werknemer.WerknemerId));
                }
            }

            //fix those who became 'overste' of theirself by setting the value 'OversteId' to null
            foreach (var werknemer in werknemers)
            {
                if (werknemer.OversteId == werknemer.WerknemerId)
                {
                    werknemer.OversteId = null;
                }
            }

            //add it
            modelBuilder.Entity<Werknemer>().HasData(werknemers);
            #endregion

            #region Campussen_hasdata
            modelBuilder.Entity<Campus>().HasData(
                new Campus
                {
                    CampusId = 1,
                    Naam = "Andros",
                },
                new Campus
                {
                    CampusId = 2,
                    Naam = "Delos",
                },
                new Campus
                {
                    CampusId = 3,
                    Naam = "Gavdos",
                },
                new Campus
                {
                    CampusId = 4,
                    Naam = "Hydra",
                },
                new Campus
                {
                    CampusId = 5,
                    Naam = "Ikaria",
                },
                new Campus
                {
                    CampusId = 6,
                    Naam = "Oinouses",
                }
            );

            modelBuilder.Entity<Land>().HasData(
                new Land { LandCode = "BE", Naam = "België" },
                new Land { LandCode = "NL", Naam = "Nederland" },
                new Land { LandCode = "DE", Naam = "Duitsland" },
                new Land { LandCode = "FR", Naam = "Frankrijk" },
                new Land { LandCode = "IT", Naam = "Italië" },
                new Land { LandCode = "LU", Naam = "Luxemburg" }
            );
            #endregion

            #region Docent_hasdata
            modelBuilder.Entity<Docent>()
                .HasOne(d => d.Land)
                .WithMany(l => l.Docenten)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Docent>()
                .HasData(
                new Docent
                {
                    DocentId = 001,
                    Voornaam = "Willy",
                    Familienaam =
                    "Abbeloos",
                    Wedde = 1400m,
                    HeeftRijbewijs = new Nullable<bool>(),
                    InDienst = new DateTime(2019, 1, 1),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 002,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Abelshausen",
                    Wedde = 1800m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 2),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 003,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Achten",
                    Wedde = 1300m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 3),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 004,
                    Voornaam = "François",
                    Familienaam =
                    "Adam",
                    Wedde = 1700m,
                    HeeftRijbewijs = new Nullable<bool>(),
                    InDienst = new DateTime(2019, 1, 4),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 005,
                    Voornaam = "Jan",
                    Familienaam =
                    "Adriaensens",
                    Wedde = 2100m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 5),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 006,
                    Voornaam = "René",
                    Familienaam =
                    "Adriaensens",
                    Wedde = 1600m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 6),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 007,
                    Voornaam = "Frans",
                    Familienaam =
                    "Aerenhouts",
                    Wedde = 1300m,
                    HeeftRijbewijs = new Nullable<bool>(),
                    InDienst = new DateTime(2019, 1, 7),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 008,
                    Voornaam = "Emile",
                    Familienaam =
                    "Aerts",
                    Wedde = 1700m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 8),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 009,
                    Voornaam = "Jean",
                    Familienaam =
                    "Aerts",
                    Wedde = 1200m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 9),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 010,
                    Voornaam = "Mario",
                    Familienaam =
                    "Aerts",
                    Wedde = 1600m,
                    HeeftRijbewijs = new Nullable<bool>(),
                    InDienst = new DateTime(2019, 1, 10),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 011,
                    Voornaam = "Paul",
                    Familienaam =
                    "Aerts",
                    Wedde = 2000m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 11),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 012,
                    Voornaam = "Stefan",
                    Familienaam =
                    "Aerts",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 12),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 013,
                    Voornaam = "François",
                    Familienaam =
                    "Alexander",
                    Wedde = 1900m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 13),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 014,
                    Voornaam = "Henri",
                    Familienaam =
                    "Allard",
                    Wedde = 1600m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 14),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 015,
                    Voornaam = "Albert",
                    Familienaam =
                    "Anciaux",
                    Wedde = 1100m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 15),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 016,
                    Voornaam = "Urbain",
                    Familienaam =
                    "Anseeuw",
                    Wedde = 1500m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 16),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 017,
                    Voornaam = "Etienne",
                    Familienaam =
                    "Antheunis",
                    Wedde = 1900m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 17),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 018,
                    Voornaam = "Jacques",
                    Familienaam =
                    "Arlet",
                    Wedde = 1400m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 18),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 019,
                    Voornaam = "Wim",
                    Familienaam =
                    "Arras",
                    Wedde = 1800m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 19),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 020,
                    Voornaam = "Roger",
                    Familienaam =
                    "Baens",
                    Wedde = 2200m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 20),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 021,
                    Voornaam = "Dirk",
                    Familienaam =
                    "Baert",
                    Wedde = 1000m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 21),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 022,
                    Voornaam = "Hubert",
                    Familienaam =
                    "Baert",
                    Wedde = 1400m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 22),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 023,
                    Voornaam = "Jean-Pierre",
                    Familienaam =
                    "Baert",
                    Wedde = 1800m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 23),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 024,
                    Voornaam = "Armand",
                    Familienaam =
                    "Baeyens",
                    Wedde = 1300m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 24),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 025,
                    Voornaam = "Jan",
                    Familienaam =
                    "Baeyens",
                    Wedde = 1700m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 25),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 026,
                    Voornaam = "Roger",
                    Familienaam =
                    "Baguet",
                    Wedde = 2100m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 26),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 027,
                    Voornaam = "Serge",
                    Familienaam =
                    "Baguet",
                    Wedde = 1600m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 27),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 028,
                    Voornaam = "Gérard",
                    Familienaam =
                    "Balducq",
                    Wedde = 1300m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 28),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 029,
                    Voornaam = "Koen",
                    Familienaam =
                    "Barbé",
                    Wedde = 1700m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 1, 29),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 030,
                    Voornaam = "Georges",
                    Familienaam =
                    "Barras",
                    Wedde = 1200m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 1, 30),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 031,
                    Voornaam = "Auguste",
                    Familienaam =
                    "Baumans",
                    Wedde = 1600m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 1, 31),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 032,
                    Voornaam = "Arsène",
                    Familienaam =
                    "Bauwens",
                    Wedde = 2000m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 1),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 033,
                    Voornaam = "Henri",
                    Familienaam =
                    "Bauwens",
                    Wedde = 1500m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 2),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 034,
                    Voornaam = "Jules",
                    Familienaam =
                    "Bayens",
                    Wedde = 1900m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 3),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 035,
                    Voornaam = "Albert",
                    Familienaam =
                    "Beckaert",
                    Wedde = 1600m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 4),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 036,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Beckaert",
                    Wedde = 1100m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 5),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 037,
                    Voornaam = "Koen",
                    Familienaam =
                    "Beeckman",
                    Wedde = 1500m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 6),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 038,
                    Voornaam = "Kamiel",
                    Familienaam =
                    "Beeckman",
                    Wedde = 1900m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 7),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 039,
                    Voornaam = "Theophile",
                    Familienaam =
                    "Beeckman",
                    Wedde = 1400m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 8),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 040,
                    Voornaam = "Benoni",
                    Familienaam =
                    "Beheyt",
                    Wedde = 1800m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 9),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 041,
                    Voornaam = "Albert",
                    Familienaam =
                    "Beirnaert",
                    Wedde = 2200m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 10),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 042,
                    Voornaam = "Jean",
                    Familienaam =
                    "Belvaux",
                    Wedde = 1000m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 11),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 043,
                    Voornaam = "Adelin",
                    Familienaam =
                    "Benoit",
                    Wedde = 1400m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 12),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 044,
                    Voornaam = "Auguste",
                    Familienaam =
                    "Benoit",
                    Wedde = 1800m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 13),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 045,
                    Voornaam = "Jef",
                    Familienaam =
                    "Berben",
                    Wedde = 1300m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 14),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 046,
                    Voornaam = "Jean-Pierre",
                    Familienaam =
                    "Berckmans",
                    Wedde = 1700m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 15),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 047,
                    Voornaam = "Albert",
                    Familienaam =
                    "Berton",
                    Wedde = 2100m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 16),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 048,
                    Voornaam = "Frans",
                    Familienaam =
                    "Beths",
                    Wedde = 1600m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 17),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 049,
                    Voornaam = "René",
                    Familienaam =
                    "Beyens",
                    Wedde = 1300m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 18),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 050,
                    Voornaam = "Herman",
                    Familienaam =
                    "Beyssens",
                    Wedde = 1700m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 19),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 051,
                    Voornaam = "Albert",
                    Familienaam =
                    "Billiet",
                    Wedde = 1200m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 20),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 052,
                    Voornaam = "Hector",
                    Familienaam =
                    "Billiet",
                    Wedde = 1600m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 21),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 053,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Blavier",
                    Wedde = 2000m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 22),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 054,
                    Voornaam = "Roger",
                    Familienaam =
                    "Blockx",
                    Wedde = 1500m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 23),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 055,
                    Voornaam = "Maurice",
                    Familienaam =
                    "Blomme",
                    Wedde = 1900m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 24),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 056,
                    Voornaam = "Willy",
                    Familienaam =
                    "Bocklandt",
                    Wedde = 1600m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 25),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 057,
                    Voornaam = "Emile",
                    Familienaam =
                    "Bodart",
                    Wedde = 1100m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 2, 26),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 058,
                    Voornaam = "Alfons",
                    Familienaam =
                    "Boekaerts",
                    Wedde = 1500m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 2, 27),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 059,
                    Voornaam = "Cesar",
                    Familienaam =
                    "Bogaert",
                    Wedde = 1900m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 2, 28),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 060,
                    Voornaam = "Jan",
                    Familienaam =
                    "Bogaert",
                    Wedde = 1400m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 1),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 061,
                    Voornaam = "Jean",
                    Familienaam =
                    "Bogaerts",
                    Wedde = 1800m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 2),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 062,
                    Voornaam = "Frans",
                    Familienaam =
                    "Bonduel",
                    Wedde = 2200m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 3),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 063,
                    Voornaam = "Tom",
                    Familienaam =
                    "Boonen",
                    Wedde = 1000m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 4),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 064,
                    Voornaam = "Jozef",
                    Familienaam =
                    "Boons",
                    Wedde = 1400m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 5),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 065,
                    Voornaam = "Gabriel",
                    Familienaam =
                    "Borra",
                    Wedde = 1800m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 6),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 066,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Bosmans",
                    Wedde = 1300m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 7),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 067,
                    Voornaam = "Walter",
                    Familienaam =
                    "Boucquet",
                    Wedde = 1700m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 8),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 068,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Boumon",
                    Wedde = 2100m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 9),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 069,
                    Voornaam = "Ferdinand",
                    Familienaam =
                    "Bracke",
                    Wedde = 1600m,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 10),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 070,
                    Voornaam = "Adolphe",
                    Familienaam =
                    "Braeckeveldt",
                    Wedde = 1300m,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 11),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 071,
                    Voornaam = "Omer",
                    Familienaam =
                    "Braekevelt",
                    Wedde = 1700m,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 12),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 072,
                    Voornaam = "Frans",
                    Familienaam =
                    "Brands",
                    Wedde = 1200,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 13),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 073,
                    Voornaam = "Jean",
                    Familienaam =
                    "Brankart",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 14),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 074,
                    Voornaam = "Emile",
                    Familienaam =
                    "Brichard",
                    Wedde = 2000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 15),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 075,
                    Voornaam = "Georges",
                    Familienaam =
                    "Brosteaux",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 16),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 076,
                    Voornaam = "Emile",
                    Familienaam =
                    "Bruneau",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 17),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 077,
                    Voornaam = "Jean-Marie",
                    Familienaam =
                    "Bruyère",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 18),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 078,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Bruyere",
                    Wedde = 1100,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 19),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 079,
                    Voornaam = "Dave",
                    Familienaam =
                    "Bruylandts",
                    Wedde = 1500,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 20),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 080,
                    Voornaam = "Johan",
                    Familienaam =
                    "Bruyneel",
                    Wedde = 1900,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 21),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 081,
                    Voornaam = "Lucien",
                    Familienaam =
                    "Buysse",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 22),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 082,
                    Voornaam = "Christophe",
                    Familienaam =
                    "Brandt",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 23),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 083,
                    Voornaam = "Norbert",
                    Familienaam =
                    "Callens",
                    Wedde = 2200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 24),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 084,
                    Voornaam = "Johan",
                    Familienaam =
                    "Capiot",
                    Wedde = 1000,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 25),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 085,
                    Voornaam = "Pino",
                    Familienaam =
                    "Cerami",
                    Wedde = 1400,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 26),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 086,
                    Voornaam = "Georges",
                    Familienaam =
                    "Christiaens",
                    Wedde = 1800,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 27),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 087,
                    Voornaam = "Georges",
                    Familienaam =
                    "Claes",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 28),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 088,
                    Voornaam = "Karel",
                    Familienaam =
                    "Clerckx",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 3, 29),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 089,
                    Voornaam = "Alex",
                    Familienaam =
                    "Close",
                    Wedde = 2100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 3, 30),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 090,
                    Voornaam = "Yvan",
                    Familienaam =
                    "Corbusier",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 3, 31),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 091,
                    Voornaam = "Hilaire",
                    Familienaam =
                    "Couvreur",
                    Wedde = 1300,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 1),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 092,
                    Voornaam = "Wilfried",
                    Familienaam =
                    "Cretskens",
                    Wedde = 1700,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 2),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 093,
                    Voornaam = "Claude",
                    Familienaam =
                    "Criquielion",
                    Wedde = 1200,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 3),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 094,
                    Voornaam = "Emile",
                    Familienaam =
                    "Daems",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 4),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 095,
                    Voornaam = "Gustave",
                    Familienaam =
                    "Danneels",
                    Wedde = 2000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 5),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 096,
                    Voornaam = "Fred",
                    Familienaam =
                    "De Bruyne",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 6),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 097,
                    Voornaam = "Arthur",
                    Familienaam =
                    "Decabooter",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 7),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 098,
                    Voornaam = "Hans",
                    Familienaam =
                    "De Clerq",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 8),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 099,
                    Voornaam = "Roger",
                    Familienaam =
                    "Decock",
                    Wedde = 1100,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 9),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 100,
                    Voornaam = "Georges",
                    Familienaam =
                    "Decraeye",
                    Wedde = 1500,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 10),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 101,
                    Voornaam = "Odiel",
                    Familienaam =
                    "Defraeye",
                    Wedde = 1900,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 11),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 102,
                    Voornaam = "Albert",
                    Familienaam =
                    "De Jonghe",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 12),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 103,
                    Voornaam = "Julien",
                    Familienaam =
                    "Delbecque",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 13),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 104,
                    Voornaam = "Alfons",
                    Familienaam =
                    "Deloor",
                    Wedde = 2200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 14),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 105,
                    Voornaam = "Gustaaf",
                    Familienaam =
                    "Deloor",
                    Wedde = 1000,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 15),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 106,
                    Voornaam = "Hubert",
                    Familienaam =
                    "Deltour",
                    Wedde = 1400,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 16),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 107,
                    Voornaam = "Paul",
                    Familienaam =
                    "Deman",
                    Wedde = 1800,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 17),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 108,
                    Voornaam = "Marc",
                    Familienaam =
                    "Demeyer",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 18),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 109,
                    Voornaam = "Frans",
                    Familienaam =
                    "De Mulder",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 19),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 110,
                    Voornaam = "Johan",
                    Familienaam =
                    "De Muynck",
                    Wedde = 2100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 20),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 111,
                    Voornaam = "Jef",
                    Familienaam =
                    "Demuysere",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 21),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 112,
                    Voornaam = "Jules",
                    Familienaam =
                    "Depoorter",
                    Wedde = 1300,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 22),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 113,
                    Voornaam = "Richard",
                    Familienaam =
                    "Depoorter",
                    Wedde = 1700,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 23),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 114,
                    Voornaam = "Prosper",
                    Familienaam =
                    "Depredomme",
                    Wedde = 1200,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 24),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 115,
                    Voornaam = "Willy",
                    Familienaam =
                    "Derboven",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 25),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 116,
                    Voornaam = "Germain",
                    Familienaam =
                    "Derijcke",
                    Wedde = 2000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 26),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 117,
                    Voornaam = "Michel",
                    Familienaam =
                    "Dernies",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 27),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 118,
                    Voornaam = "Charles",
                    Familienaam =
                    "Deruyter",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 4, 28),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 119,
                    Voornaam = "Maurice",
                    Familienaam =
                    "Desimpelaere",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 4, 29),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 120,
                    Voornaam = "Gilbert",
                    Familienaam =
                    "Desmet",
                    Wedde = 1100,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 4, 30),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 121,
                    Voornaam = "Georges",
                    Familienaam =
                    "Desplenter",
                    Wedde = 1500,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 1),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 122,
                    Voornaam = "Léon",
                    Familienaam =
                    "Despontin",
                    Wedde = 1900,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 2),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 123,
                    Voornaam = "Eric",
                    Familienaam =
                    "De Vlaeminck",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 3),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 124,
                    Voornaam = "Roger",
                    Familienaam =
                    "De Vlaeminck",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 4),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 125,
                    Voornaam = "Stijn",
                    Familienaam =
                    "Devolder",
                    Wedde = 2200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 5),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 126,
                    Voornaam = "Maurice",
                    Familienaam =
                    "Dewaele",
                    Wedde = 1000,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 6),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 127,
                    Voornaam = "Alfons",
                    Familienaam =
                    "De Wolf",
                    Wedde = 1400,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 7),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 128,
                    Voornaam = "Rudy",
                    Familienaam =
                    "Dhaenens",
                    Wedde = 1800,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 8),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 129,
                    Voornaam = "Michel",
                    Familienaam =
                    "D''Hooghe",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 9),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 130,
                    Voornaam = "Ludo",
                    Familienaam =
                    "Dierckxsens",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 10),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 131,
                    Voornaam = "Frans",
                    Familienaam =
                    "Dictus",
                    Wedde = 2100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 11),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 132,
                    Voornaam = "Lomme",
                    Familienaam =
                    "Driessens",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 12),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 133,
                    Voornaam = "Gustave",
                    Familienaam =
                    "Drioul",
                    Wedde = 1300,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 13),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 134,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Dupont",
                    Wedde = 1700,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 14),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 135,
                    Voornaam = "Niko",
                    Familienaam =
                    "Eeckhout",
                    Wedde = 1200,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 15),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 136,
                    Voornaam = "Nico",
                    Familienaam =
                    "Emonds",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 16),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 137,
                    Voornaam = "Peter",
                    Familienaam =
                    "Farazijn",
                    Wedde = 2000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 17),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 138,
                    Voornaam = "Herman",
                    Familienaam =
                    "Frison",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 18),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 139,
                    Voornaam = "Henri",
                    Familienaam =
                    "Garnier",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 19),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 140,
                    Voornaam = "Frans",
                    Familienaam =
                    "Gielen",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 20),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 141,
                    Voornaam = "Romain",
                    Familienaam =
                    "Gijssels",
                    Wedde = 1100,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 21),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 142,
                    Voornaam = "Walter",
                    Familienaam =
                    "Godefroot",
                    Wedde = 1500,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 22),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 143,
                    Voornaam = "Dries",
                    Familienaam =
                    "Govaerts",
                    Wedde = 1900,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 23),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 144,
                    Voornaam = "Sylvain",
                    Familienaam =
                    "Grysolle",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 24),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 145,
                    Voornaam = "Roger",
                    Familienaam =
                    "Gyselinck",
                    Wedde = 1800,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 25),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 146,
                    Voornaam = "Paul",
                    Familienaam =
                    "Haghedooren",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 26),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 147,
                    Voornaam = "Alfred",
                    Familienaam =
                    "Hamerlinck",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 27),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 148,
                    Voornaam = "Louis",
                    Familienaam =
                    "Hardiquest",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 28),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 149,
                    Voornaam = "Emile",
                    Familienaam =
                    "Hardy",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 5, 29),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 150,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Hendrikx",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 5, 30),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 151,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Hoevenaers",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 5, 31),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 152,
                    Voornaam = "Kevin",
                    Familienaam =
                    "Hulsmans",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 1),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 153,
                    Voornaam = "Raymond",
                    Familienaam =
                    "Impanis",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 2),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 154,
                    Voornaam = "Paul",
                    Familienaam =
                    "In''t",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 3),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 155,
                    Voornaam = "Willy",
                    Familienaam =
                    "In''t",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 4),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 156,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Janssens",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 5),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 157,
                    Voornaam = "Benjamin",
                    Familienaam =
                    "Javaux",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 6),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 158,
                    Voornaam = "Karel",
                    Familienaam =
                    "Kaers",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 7),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 159,
                    Voornaam = "Francis",
                    Familienaam =
                    "Kemplaire",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 8),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 160,
                    Voornaam = "Norbert",
                    Familienaam =
                    "Kerckhove",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 9),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 161,
                    Voornaam = "Désiré",
                    Familienaam =
                    "Keteleer",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 10),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 162,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Kint",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 11),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 163,
                    Voornaam = "Firmin",
                    Familienaam =
                    "Lambot",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 12),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 164,
                    Voornaam = "Roger",
                    Familienaam =
                    "Lambrecht",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 13),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 165,
                    Voornaam = "Eric",
                    Familienaam =
                    "Leman",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 14),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 166,
                    Voornaam = "Camille",
                    Familienaam =
                    "Leroy",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 15),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 167,
                    Voornaam = "Roland",
                    Familienaam =
                    "Liboton",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 16),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 168,
                    Voornaam = "Jules",
                    Familienaam =
                    "Lowie",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 17),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 169,
                    Voornaam = "André",
                    Familienaam =
                    "Lurquin",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 18),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 170,
                    Voornaam = "Henri",
                    Familienaam =
                    "Rik",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 19),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 171,
                    Voornaam = "Pierrot",
                    Familienaam =
                    "Machiels",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 20),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 172,
                    Voornaam = "André",
                    Familienaam =
                    "Maelbrancke",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 21),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 173,
                    Voornaam = "Freddy",
                    Familienaam =
                    "Maertens",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 22),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 174,
                    Voornaam = "Romain",
                    Familienaam =
                    "Maes",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 23),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 175,
                    Voornaam = "Sylvère",
                    Familienaam =
                    "Maes",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 24),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 176,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Marchand",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 25),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 177,
                    Voornaam = "René",
                    Familienaam =
                    "Martens",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 26),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 178,
                    Voornaam = "Jacques",
                    Familienaam =
                    "Martin",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 27),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 179,
                    Voornaam = "Emile",
                    Familienaam =
                    "père",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 6, 28),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 180,
                    Voornaam = "Florent",
                    Familienaam =
                    "Mathieu",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 6, 29),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 181,
                    Voornaam = "Nico",
                    Familienaam =
                    "Mattan",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 6, 30),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 182,
                    Voornaam = "Filip",
                    Familienaam =
                    "Meirhaeghe",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 1),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 183,
                    Voornaam = "Axel",
                    Familienaam =
                    "Merckx",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 2),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 184,
                    Voornaam = "Eddy",
                    Familienaam =
                    "Merckx",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 3),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 185,
                    Voornaam = "André",
                    Familienaam =
                    "Messelis",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 4),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 186,
                    Voornaam = "Maurice",
                    Familienaam =
                    "Meuleman",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 5),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 187,
                    Voornaam = "Eloi",
                    Familienaam =
                    "Meulenberg",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 6),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 188,
                    Voornaam = "Frans",
                    Familienaam =
                    "Mintjens",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 7),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 189,
                    Voornaam = "Yvo",
                    Familienaam =
                    "Molenaers",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 8),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 190,
                    Voornaam = "Maurice",
                    Familienaam =
                    "Mollin",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 9),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 191,
                    Voornaam = "Arthur",
                    Familienaam =
                    "Mommerency",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 10),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 192,
                    Voornaam = "Jean-Pierre",
                    Familienaam =
                    "Monséré",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 11),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 193,
                    Voornaam = "Willy",
                    Familienaam =
                    "Monty",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 12),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 194,
                    Voornaam = "Sammie",
                    Familienaam =
                    "Moreels",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 13),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 195,
                    Voornaam = "Alfred",
                    Familienaam =
                    "Mottard",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 14),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 196,
                    Voornaam = "Ernest",
                    Familienaam =
                    "Mottart",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 15),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 197,
                    Voornaam = "Louis",
                    Familienaam =
                    "Mottiat",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 16),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 198,
                    Voornaam = "Johan",
                    Familienaam =
                    "Museeuw",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 17),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 199,
                    Voornaam = "Wilfried",
                    Familienaam =
                    "Nelissen",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 18),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 200,
                    Voornaam = "François",
                    Familienaam =
                    "Neuville",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 19),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 201,
                    Voornaam = "André",
                    Familienaam =
                    "Noyelle",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 20),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 202,
                    Voornaam = "Guy",
                    Familienaam =
                    "Nulens",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 21),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 203,
                    Voornaam = "Nick",
                    Familienaam =
                    "Nuyens",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 22),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 204,
                    Voornaam = "Sven",
                    Familienaam =
                    "Nys",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 23),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 205,
                    Voornaam = "Stan",
                    Familienaam =
                    "Ockers",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 24),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 206,
                    Voornaam = "Petrus",
                    Familienaam =
                    "Oellibrandt",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 25),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 207,
                    Voornaam = "Valère",
                    Familienaam =
                    "Ollivier",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 26),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 208,
                    Voornaam = "Eddy",
                    Familienaam =
                    "Peelman",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 27),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 209,
                    Voornaam = "Edward",
                    Familienaam =
                    "Peeters",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 28),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 210,
                    Voornaam = "Luc",
                    Familienaam =
                    "Petitjean",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 7, 29),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 211,
                    Voornaam = "Victor",
                    Familienaam =
                    "Louis",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 7, 30),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 212,
                    Voornaam = "Georges",
                    Familienaam =
                    "Pintens",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 7, 31),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 213,
                    Voornaam = "Théodore",
                    Familienaam =
                    "Pirmez",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 1),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 214,
                    Voornaam = "Eddy",
                    Familienaam =
                    "Planckaert",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 2),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 215,
                    Voornaam = "Jo",
                    Familienaam =
                    "Planckaert",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 3),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 216,
                    Voornaam = "Walter",
                    Familienaam =
                    "Planckaert",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 4),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 217,
                    Voornaam = "Willy",
                    Familienaam =
                    "Planckaert",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 5),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 218,
                    Voornaam = "Michel",
                    Familienaam =
                    "Pollentier",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 6),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 219,
                    Voornaam = "Léon",
                    Familienaam =
                    "Poncelet",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 7),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 220,
                    Voornaam = "Louis",
                    Familienaam =
                    "Proost",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 8),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 221,
                    Voornaam = "Robert",
                    Familienaam =
                    "Protin",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 9),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 222,
                    Voornaam = "Albert",
                    Familienaam =
                    "Ramon",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 10),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 223,
                    Voornaam = "Gaston",
                    Familienaam =
                    "Rebry",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 11),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 224,
                    Voornaam = "Jens",
                    Familienaam =
                    "Renders",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 12),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 225,
                    Voornaam = "Guido",
                    Familienaam =
                    "Reybrouck",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 13),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 226,
                    Voornaam = "Marcel",
                    Familienaam =
                    "Rijckaert",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 14),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 227,
                    Voornaam = "Albert",
                    Familienaam =
                    "Ritserveldt",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 15),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 228,
                    Voornaam = "Bert",
                    Familienaam =
                    "Roesems",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 16),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 229,
                    Voornaam = "Louis",
                    Familienaam =
                    "Rolus",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 17),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 230,
                    Voornaam = "Georges",
                    Familienaam =
                    "Ronsse",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 18),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 231,
                    Voornaam = "André",
                    Familienaam =
                    "Rosseel",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 19),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 232,
                    Voornaam = "Félicien",
                    Familienaam =
                    "Salmon",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 20),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 233,
                    Voornaam = "Léopold",
                    Familienaam =
                    "Schaeken",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 21),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 234,
                    Voornaam = "Willy",
                    Familienaam =
                    "Scheers",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 22),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 235,
                    Voornaam = "Alfons",
                    Familienaam =
                    "Schepers",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 23),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 236,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Scherens",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 24),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 237,
                    Voornaam = "Jef",
                    Familienaam =
                    "Scherens",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 25),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 238,
                    Voornaam = "Briek",
                    Familienaam =
                    "Schotte",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 26),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 239,
                    Voornaam = "Frans",
                    Familienaam =
                    "Schoubben",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 27),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 240,
                    Voornaam = "Léon",
                    Familienaam =
                    "Scieur",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 28),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 241,
                    Voornaam = "Félix",
                    Familienaam =
                    "Sellier",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 8, 29),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 242,
                    Voornaam = "Edward",
                    Familienaam =
                    "Sels",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 8, 30),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 243,
                    Voornaam = "Albert",
                    Familienaam =
                    "Sercu",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 8, 31),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 244,
                    Voornaam = "Patrick",
                    Familienaam =
                    "Sercu",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 1),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 245,
                    Voornaam = "Andy",
                    Familienaam =
                    "de Smet",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 2),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 246,
                    Voornaam = "Joseph",
                    Familienaam =
                    "Somers",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 3),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 247,
                    Voornaam = "Tom",
                    Familienaam =
                    "Steels",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 4),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 248,
                    Voornaam = "Ernest",
                    Familienaam =
                    "Sterckx",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 5),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 249,
                    Voornaam = "Lucien",
                    Familienaam =
                    "Storme",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 6),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 250,
                    Voornaam = "Tom",
                    Familienaam =
                    "Stubbe",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 7),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 251,
                    Voornaam = "Roger",
                    Familienaam =
                    "Swerts",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 8),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 252,
                    Voornaam = "Arthur",
                    Familienaam =
                    "Targez",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 10),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 253,
                    Voornaam = "Andrei",
                    Familienaam =
                    "Tchmil",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 11),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 254,
                    Voornaam = "Emmanuel",
                    Familienaam =
                    "Thoma",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 12),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 255,
                    Voornaam = "Philippe",
                    Familienaam =
                    "Thys",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 13),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 256,
                    Voornaam = "Hector",
                    Familienaam =
                    "Tiberghien",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 14),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 257,
                    Voornaam = "Léon",
                    Familienaam =
                    "Tommies",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 15),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 258,
                    Voornaam = "Lode",
                    Familienaam =
                    "Troonbeeckx",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 16),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 259,
                    Voornaam = "Greg",
                    Familienaam =
                    "Van Avermaet",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 17),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 260,
                    Voornaam = "Armand",
                    Familienaam =
                    "Van Bruaene",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 18),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 261,
                    Voornaam = "Georges",
                    Familienaam =
                    "Vanconingsloo",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 19),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 262,
                    Voornaam = "Léon",
                    Familienaam =
                    "Van Daele",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 20),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 263,
                    Voornaam = "Charles",
                    Familienaam =
                    "Van Den Born",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 21),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 264,
                    Voornaam = "Frank",
                    Familienaam =
                    "Vandenbroucke",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 22),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 265,
                    Voornaam = "Odiel",
                    Familienaam =
                    "Vanden Meerschaut",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 23),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 266,
                    Voornaam = "Eric",
                    Familienaam =
                    "Vanderaerden",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 24),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 267,
                    Voornaam = "Kurt",
                    Familienaam =
                    "Van de Wouwer",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 25),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 268,
                    Voornaam = "Richard",
                    Familienaam =
                    "Van Genechten",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 26),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 269,
                    Voornaam = "Martin",
                    Familienaam =
                    "Van Geneugden",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 9, 27),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 270,
                    Voornaam = "Cyrille",
                    Familienaam =
                    "Van Hauwaert",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 9, 28),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 271,
                    Voornaam = "Maurice",
                    Familienaam =
                    "Van Herzele",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 9, 30),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 272,
                    Voornaam = "Jules",
                    Familienaam =
                    "Van Hevel",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 1),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 273,
                    Voornaam = "Edwig",
                    Familienaam =
                    "Van Hooydonck",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 2),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 274,
                    Voornaam = "Lucien",
                    Familienaam =
                    "Van Impe",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 3),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 275,
                    Voornaam = "Henri",
                    Familienaam =
                    "Van Kerkhove",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 4),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 276,
                    Voornaam = "Rik",
                    Familienaam =
                    "Van Linden",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 5),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 277,
                    Voornaam = "Rik",
                    Familienaam =
                    "Van Looy",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 6),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 278,
                    Voornaam = "Willy",
                    Familienaam =
                    "Vannitsen",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 7),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 279,
                    Voornaam = "Peter",
                    Familienaam =
                    "Van Petegem",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 8),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 280,
                    Voornaam = "Peter",
                    Familienaam =
                    "Van Santvliet",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 9),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 281,
                    Voornaam = "Victor",
                    Familienaam =
                    "Van Schil",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 10),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 282,
                    Voornaam = "Herman",
                    Familienaam =
                    "van Springel",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 11),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 283,
                    Voornaam = "Rik",
                    Familienaam =
                    "Van Steenbergen",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 12),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 284,
                    Voornaam = "Guillaume",
                    Familienaam =
                    "Van Tongerloo",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 13),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 285,
                    Voornaam = "Noël",
                    Familienaam =
                    "Vantyghem",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 14),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 286,
                    Voornaam = "Rik",
                    Familienaam =
                    "Verbrugghe",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 15),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 287,
                    Voornaam = "Auguste",
                    Familienaam =
                    "Verdyck",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 16),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 288,
                    Voornaam = "Jozef",
                    Familienaam =
                    "Verhaert",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 17),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 289,
                    Voornaam = "René",
                    Familienaam =
                    "Vermandel",
                    Wedde = 1500,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 18),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 290,
                    Voornaam = "Stive",
                    Familienaam =
                    "Vermaut",
                    Wedde = 1900,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 19),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 291,
                    Voornaam = "Adolf",
                    Familienaam =
                    "Verschueren",
                    Wedde = 1400,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 20),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 292,
                    Voornaam = "Constant",
                    Familienaam =
                    "Verschueren",
                    Wedde = 1800,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 21),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 293,
                    Voornaam = "Johan",
                    Familienaam =
                    "Verstrepen",
                    Wedde = 2200,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 22),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 294,
                    Voornaam = "Félicien",
                    Familienaam = "Vervaecke",
                    Wedde = 1000,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 23),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 295,
                    Voornaam = "Julien",
                    Familienaam = "Vervaecke",
                    Wedde = 1400,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 24),
                    CampusId = 4,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 296,
                    Voornaam = "Edward",
                    Familienaam = "Vissers",
                    Wedde = 1800,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 25),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 297,
                    Voornaam = "Lucien",
                    Familienaam = "Vlaemynck",
                    Wedde = 1300,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 26),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 298,
                    Voornaam = "André",
                    Familienaam = "Vlaeyen",
                    Wedde = 1700,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 27),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 299,
                    Voornaam = "Jean",
                    Familienaam =
                    "Vliegen",
                    Wedde = 2100,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 28),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 300,
                    Voornaam = "Luc",
                    Familienaam =
                    "Wallays",
                    Wedde = 1600,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 10, 29),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 301,
                    Voornaam = "René",
                    Familienaam =
                    "Walschot",
                    Wedde = 1300,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 10, 30),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 302,
                    Voornaam = "Jean-Marie",
                    Familienaam =
                    "Wampers",
                    Wedde = 1700,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 10, 31),
                    CampusId = 1,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 303,
                    Voornaam = "Robert",
                    Familienaam =
                    "Wancour",
                    Wedde = 1200,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 11, 1),
                    CampusId = 2,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 304,
                    Voornaam = "Bart",
                    Familienaam =
                    "Wellens",
                    Wedde = 1600,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 11, 2),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 305,
                    Voornaam = "Wilfried",
                    Familienaam =
                    "Wesemael",
                    Wedde = 2000,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 11, 3),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 306,
                    Voornaam = "Wouter",
                    Familienaam =
                    "Weylandt",
                    Wedde = 1500,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 11, 4),
                    CampusId = 5,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 307,
                    Voornaam = "Marc",
                    Familienaam =
                    "Wauters",
                    Wedde = 1900,
                    HeeftRijbewijs = false,
                    InDienst =
                    new DateTime(2019, 11, 5),
                    CampusId = 3,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 308,
                    Voornaam = "Daniel",
                    Familienaam =
                    "Willems",
                    Wedde = 1600,
                    HeeftRijbewijs = null,
                    InDienst =
                    new DateTime(2019, 11, 6),
                    CampusId = 6,
                    LandCode = "BE"
                },
                new Docent
                {
                    DocentId = 309,
                    Voornaam = "Jozef",
                    Familienaam =
                    "Wouters",
                    Wedde = 1100,
                    HeeftRijbewijs = true,
                    InDienst =
                    new DateTime(2019, 11, 7),
                    CampusId = 1,
                    LandCode = "BE"
                }

        );
            #endregion
        }
    }
}
