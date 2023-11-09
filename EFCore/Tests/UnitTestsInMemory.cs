using Microsoft.EntityFrameworkCore;
using Model.Entitites;
using Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class UnitTestsInMemory
    {
        DbContextOptions<EFOpleidingenContext> options;

        [TestInitialize]
        public void Initializer()
        {
            options = new DbContextOptionsBuilder<EFOpleidingenContext>()
            .UseInMemoryDatabase($"InMemoryDatabase{Guid.NewGuid()}")
            .Options;
        }

        [TestMethod]
        public void GetDocentenVoorCampus_Docenten_AantalIsZesDocenten()
        {
            //creer een dbcontext, maar voorzie daarin onze eigen databaseverbinding
            using var context = new EFOpleidingenContext(options);
            //toevoegen land
            context.Landen.Add(new Land() { LandCode = "BE", Naam = "België" });
            //toevoegen campussen
            context.Campussen.Add(new Campus()
            {
                CampusId = 1,
                Naam = "Andros",
                Adres = new Adres
                {
                    Straat = "Somersstraat",
                    Huisnummer = "22",
                    Postcode = "2018",
                    Gemeente = "Antwerpen"
                }
            });
            context.Campussen.Add(new Campus()
            {
                CampusId = 2,
                Naam = "Delos",
                Adres = new Adres
                {
                    Straat = "Oude Vest",
                    Huisnummer = "17",
                    Postcode = "9200",
                    Gemeente = "Dendermonde"
                }
            });
            //toevoegen docenten
            context.Docenten.Add(new Docent()
            {
                DocentId = 001,
                Voornaam = "Willy",
                Familienaam = "Abbeloos",
                Wedde = 1500m,
                HeeftRijbewijs = new Nullable<bool>(),
                InDienst = new DateTime(2019, 1, 1),
                CampusId = 1,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 002,
                Voornaam = "Joseph",
                Familienaam = "Abelshausen",
                Wedde = 1800m,
                HeeftRijbewijs = true,
                InDienst = new DateTime(2019, 1, 2),
                CampusId = 2,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 003,
                Voornaam = "Joseph",
                Familienaam = "Achten",
                Wedde = 1300m,
                HeeftRijbewijs = false,
                InDienst = new DateTime(2019, 1, 3),
                CampusId = 1,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 004,
                Voornaam = "François",
                Familienaam = "Adam",
                Wedde = 1700m,
                HeeftRijbewijs = new Nullable<bool>(),
                InDienst = new DateTime(2019, 1, 4),
                CampusId = 2,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 005,
                Voornaam = "Jan",
                Familienaam = "Adriaensens",
                Wedde = 2100m,
                HeeftRijbewijs = true,
                InDienst = new DateTime(2019, 1, 5),
                CampusId = 1,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 006,
                Voornaam = "René",
                Familienaam = "Adriaensens",
                Wedde = 1600m,
                HeeftRijbewijs = false,
                InDienst = new DateTime(2019, 1, 6),
                CampusId = 2,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 007,
                Voornaam = "Frans",
                Familienaam = "Aerenhouts",
                Wedde = 1300m,
                HeeftRijbewijs = new Nullable<bool>(),
                InDienst = new DateTime(2019, 1, 7),
                CampusId = 1,
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 008,
                Voornaam = "Emile",
                Familienaam = "Aerts",
                Wedde = 1700m,
                HeeftRijbewijs = true,
                InDienst = new DateTime(2019, 1, 8),
                CampusId = 1,
                LandCode = "BE"
            });
            //      record met onbestaande CampusId : 
            context.Docenten.Add(new Docent()
            {
                DocentId = 009,
                Voornaam = "Jean",
                Familienaam = "Aerts",
                Wedde = 1200m,
                HeeftRijbewijs = false,
                InDienst = new DateTime(2019, 1, 9),
                CampusId = 3,       //Onbestaande campus
                LandCode = "BE"
            });
            context.Docenten.Add(new Docent()
            {
                DocentId = 010,
                Voornaam = "Mario",
                Familienaam = "Aerts",
                Wedde = 1600m,
                HeeftRijbewijs = new Nullable<bool>(),
                InDienst = new DateTime(2019, 1, 10),
                CampusId = 1,
                LandCode = "BE"
            });
            context.SaveChanges();
            // voorzie een docentservice
            var docentService = new DocentService(context);
            //roep GetDocentenVoorCampus op voor campus id 1
            var docenten = docentService.GetDocentenVoorCampus(1);
            //het resultaat zou 6 moeten zijn
            Assert.AreEqual(6, docenten.Count());
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]  //deze testmethode dient een exception op te werpen
        public void GetDocent_Docent0_ThrowArgumentException()
        {
            using var context = new EFOpleidingenContext(options);  //maak een context
            var docentService = new DocentService(context);         //create a service based on that context
            var docent = docentService.GetDocent(0);                //use the service to get docenten
        }

        [TestMethod]
        public void ToevoegenDocent_DocentZonderLand_DocentHeeftLandBE()
        {
            using var context = new EFOpleidingenContext(options);

            context.Landen.Add(new Land()                           //voeg land toe
            {
                LandCode = "BE",
                Naam = "België"
            });
            context.SaveChanges();
            var docentService = new DocentService(context);

            var docent = new Docent()                               //voeg docent toe, zonder landcode (wordt opgevangen in de service
            {
                DocentId = 20,
                Voornaam = "Fanny",
                Familienaam = "Kiekeboe",
                Wedde = 10100,
                InDienst = new DateTime(2019, 1, 1),
                CampusId = 1
            };
            docentService.ToevoegenDocent(docent);
            context.SaveChanges();

            var docent1 = docentService.GetDocent(20);              //vraag de toegevoegde docent op
            Assert.AreEqual("BE", docent1.LandCode);                //Het toegevoegde land zou België moeten zijn
        }

    }
}