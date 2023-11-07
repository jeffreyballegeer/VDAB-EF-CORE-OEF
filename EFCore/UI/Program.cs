using Microsoft.EntityFrameworkCore;
using Model.Entitites;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


//GetAllDocentsFromDb_LogSQLToConsole();
//GetAllDocentsWithMinimumWage_UsingLinq();
//GetAllDocentsWithMinimumWage_UsingQueryMethods();
//GetDocentOnPrimaryKey_UsingFind();
//GetPartialObjects_UsingLINQ();
//GetPartialObjects_UsingQueryMethods();
//GetAllDocents_GroupByName();
//GetAllDocentsWithNameX_TestForLazyLoadingUsingProxies();
//GetAllDocentsWithCampus_ExampleForEagerLoading();
//GetAllDocentsMatchingSearchIncludeCampus_ExampleForEagerLoading();
//GetAllCampusses_KeepResultsetUsingToList();
//AddNewEntity();
//AddMultipleEntities();
//AddAssociatedEntities();
//AddMixedNewAndExistingEntities_FromOneSide();
//AddMixedNewAndExistingEntities_FromManySide();
//UpdateOneEntity();
//UpdateSomeSelectedEntities();
//UpdateAssociatedEntities();
//UpdateAssociationOfdEntity_FromManySide();
//UpdateAssociationOfdEntity_FromOneSide();
//RemoveOneEntity();


//AddMixedNewAndExistingEntities_FromManySide();
//RemoveOneEntityWithAssociatedEntities();

//AddExampleData_TPH();
//AddExampleData_TPT();
//AddExampleData_TPC();
//GetAllCursussenWithBoeken();
//AddNewBoekAndAddItToACursusAndGetAllCursussenWithBoeken();

//AddNewDocentenAktiviteitenAndDocentAktiviteiten();
//GetAllDocentenAndAktiviteiten();

AddNewWerknemers();
GetWerknemers1_ToonWerknemersZonderOverste();
GetWerknemers2_ToonWerknemersMetOverste_ToonOokOverste();
GetWerknemers3_ToonOverstenMetHunOndergeschikten();
UpdateWerknemers_MakeWerknemer5OversteVanWerknemer6();


#region Methods

void GetAllDocentsFromDb_LogSQLToConsole()
{
    //Code to show all docenten from db
    using var context = new EFOpleidingenContext();
    foreach (var docent in context.Docenten)
        Console.WriteLine(docent.Naam);

    //Log SQL Query's to console
    Console.WriteLine(context.Docenten.ToQueryString());
    Console.WriteLine(context.Campussen.ToQueryString());
}
void GetAllDocentsWithMinimumWage_UsingLinq()
{
    Console.Write("Minimum wedde:");
    if (decimal.TryParse(Console.ReadLine(), out decimal minWedde))
    {
        using var context = new EFOpleidingenContext();
        //lINQ
        var query = from docent in context.Docenten
                    where docent.Wedde >= minWedde
                    orderby docent.Voornaam, docent.Familienaam
                    select docent;
        foreach (var docent in query)
            Console.WriteLine("{0}: {1}", docent.Naam, docent.Wedde);
    }
    else
        Console.WriteLine("Dat was geen getal!");
}
void GetAllDocentsWithMinimumWage_UsingQueryMethods()
{
    Console.Write("Minimum wedde:");
    if (decimal.TryParse(Console.ReadLine(), out decimal minWedde))
    {
        //QUERY METHODS
        using var context = new EFOpleidingenContext();
        var query = context.Docenten
                                .Where(docent => docent.Wedde >= minWedde)
                                .OrderBy(docent => docent.Voornaam)
                                .ThenBy(docent => docent.Familienaam);
        foreach (var docent in query)
            Console.WriteLine("{0}: {1}", docent.Naam, docent.Wedde);
    }
    else
        Console.WriteLine("Dat was geen getal!");
}
void GetDocentOnPrimaryKey_UsingFind()
{
    //Demonstrating .Find : search on primary key
    using var context2 = new EFOpleidingenContext();
    Console.Write("DocentNr.:");
    if (int.TryParse(Console.ReadLine(), out int docentNr))
    {
        var docent = context2.Docenten.Find(docentNr);
        Console.WriteLine(docent == null ? "Niet gevonden" : docent.Naam);
    }
    else
        Console.WriteLine("U tikte geen getal");
}
void GetPartialObjects_UsingLINQ()
{
    //Demonstrating select new {...} : Search only selected fields using LINQ
    using var context3 = new EFOpleidingenContext();
    //LINQ:
    var query2 = from campus in context3.Campussen
                 orderby campus.Naam
                 select new { campus.CampusId, campus.Naam };
    Console.WriteLine(query2.ToQueryString());
    foreach (var campusDeel in query2)
        Console.WriteLine("{0}: {1}", campusDeel.CampusId, campusDeel.Naam);
}
void GetPartialObjects_UsingQueryMethods()
{
    //Demonstrating select new {...} : Search only selected fields using Query Methods
    using var context3 = new EFOpleidingenContext();
    //Query-Methods :
    var query2 = context3.Campussen
                        .OrderBy(campus => campus.Naam)
                        .Select(campus => new { campus.CampusId, campus.Naam });
    Console.WriteLine(query2.ToQueryString());
    foreach (var campusDeel in query2)
        Console.WriteLine("{0}: {1}", campusDeel.CampusId, campusDeel.Naam);
}
void GetAllDocents_GroupByName()
{
    //use group, by & into to group results of a query
    using var context = new EFOpleidingenContext();
    var query = from docent in context.Docenten
                group docent by docent.Voornaam
                into voornaamGroep
                select new
                {
                    Voornaam = voornaamGroep.Key, //key on which was grouped
                    Aantal = voornaamGroep.Count()
                };
    foreach (var voornaamStatistiek in query)
    {
        Console.Write(voornaamStatistiek.Voornaam + ": ");
        Console.WriteLine(voornaamStatistiek.Aantal + " keer.");
    }
}

void GetAllDocentsWithNameX_TestForLazyLoadingUsingProxies()
{
    // to be used with proxies eager loading enabled (.UseLazyLoadingProxies() in program.cs & virtual nav props in entities
    // running on eager loading will yield null error on campus.name
    using var context = new EFOpleidingenContext();
    Console.Write("Voornaam:");
    var voornaam = Console.ReadLine();
    var query = from docent in context.Docenten // (1) 
                where docent.Voornaam == voornaam
                select docent;
    foreach (var docent in query)
        Console.WriteLine("{0} : {1}", docent.Naam, docent.Campus.Naam);
}
void GetAllDocentsWithCampus_ExampleForEagerLoading()
{
    using var context = new EFOpleidingenContext();
    Console.Write("Voornaam:");
    var voornaam = Console.ReadLine();
    //the include means: for all the Docenten, include the related Campus data

    var query = from docent in context.Docenten.Include("Campus")
                where docent.Voornaam == voornaam
                select docent;
    foreach (var docent in query)
        Console.WriteLine("{0} : {1}", docent.Naam, docent.Campus.Naam);
    Console.WriteLine(query.ToQueryString());
}
void GetAllDocentsMatchingSearchIncludeCampus_ExampleForEagerLoading()
{
    using var context = new EFOpleidingenContext();
    Console.Write("Geef (een deel van) de naam van de campus:");
    var deelNaam = Console.ReadLine();
    //using Query Methods
    var query = from campus in context.Campussen.Include("Docenten")
                where campus.Naam.Contains(deelNaam)
                orderby campus.Naam
                select campus;
    //using LINQ
    var query2 = context.Campussen.Include("Docenten")
                                  .Where(campus => campus.Naam.Contains(deelNaam))
                                  .OrderBy(campus => campus.Naam);
    foreach (var campus in query2) // or query2
    {
        var campusNaam = campus.Naam;
        Console.WriteLine(campusNaam);
        Console.WriteLine(new string('-', campusNaam.Length));
        foreach (var docent in campus.Docenten)
            Console.WriteLine(docent.Naam);
        Console.WriteLine();
    }
}

void GetAllCampusses_KeepResultsetUsingToList()
{
    List<Campus> campussen;
    using var context = new EFOpleidingenContext();
    var query = from campus in context.Campussen
                orderby campus.Naam
                select campus;
    campussen = query.ToList(); // this allows usage of the resultset outside of using-statement + no querying DB only once
    foreach (var campus in campussen)       //no extra DB query on this line
        Console.WriteLine(campus.Naam);
    Console.WriteLine();
    foreach (var campus in campussen)       //no extra DB query on this line
        Console.WriteLine(campus.Naam);
    //--------------------
    Console.WriteLine("Now use resultset outside of using statement :");
    //below code shows that using the resultset can happen outside the using statement. (FindAllCampussen gets data in the getter)
    foreach (var campus in FindAllCampussen())
        Console.WriteLine(campus.Naam);
    List<Campus> FindAllCampussen()
    {
        using var context = new EFOpleidingenContext();
        return (from campus in context.Campussen
                orderby campus.Naam
                select campus).ToList();
    }
}

void AddNewEntity()
{
    var campus = new Campus
    {
        Naam = "Campus01",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat01",
            Huisnummer = "1",
            Postcode = "1111",
            Gemeente = "Gemeente01"
        }
    };
    using var context = new EFOpleidingenContext();
    context.Campussen.Add(campus);
    context.SaveChanges();
    Console.WriteLine(campus.CampusId);
}
void AddMultipleEntities()
{
    var campus2 = new Campus
    {
        Naam = "Campus02",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat02",
            Huisnummer = "2",
            Postcode = "2222",
            Gemeente = "Gemeente02"
        }
    };
    var campus3 = new Campus
    {
        Naam = "Campus03",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat03",
            Huisnummer = "3",
            Postcode = "3333",
            Gemeente = "Gemeente03"
        }
    };
    var campus4 = new Campus
    {
        Naam = "Campus04",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat04",
            Huisnummer = "4",
            Postcode = "4444",
            Gemeente = "Gemeente04"
        }
    };
    var campus5 = new Campus
    {
        Naam = "Campus05",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat05",
            Huisnummer = "5",
            Postcode = "5555",
            Gemeente = "Gemeente05"
        }
    };
    using var context = new EFOpleidingenContext();
    context.Campussen.AddRange(campus2, campus3);
    context.Campussen.AddRange(new List<Campus> { campus4, campus5 });
    context.SaveChanges();
}
void AddAssociatedEntities()
{
    //hier gaan we een nieuwe docent toevoegen die werkt aan een nieuwe campus
    var campus7 = new Campus
    {
        Naam = "Campus07",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat07",
            Huisnummer = "7",
            Postcode = "7777",
            Gemeente = "Gemeente07"
        }
    };
    var docent2 = new Docent
    {
        Voornaam = "Voornaam02",
        Familienaam = "Docent02",
        Wedde = 2222,
        LandCode = "NL"
    };
    campus7.Docenten.Add(docent2);
    using var context = new EFOpleidingenContext();
    context.Campussen.Add(campus7);
    context.SaveChanges();

    //hier gaan we een nieuwe campus toevoegen waar een nieuwe docent werkt
    var campus8 = new Campus
    {
        Naam = "Campus08",
        Adres = new Adres                       //db structure changed
        {
            Straat = "Straat08",
            Huisnummer = "8",
            Postcode = "8888",
            Gemeente = "Gemeente08"
        }
    };
    var docent3 = new Docent
    {
        Voornaam = "Voornaam03",
        Familienaam = "Docent03",
        Wedde = 3333,
        LandCode = "IT"
    };
    docent3.Campus = campus8;
    using var context2 = new EFOpleidingenContext();
    context2.Docenten.Add(docent3);
    context2.SaveChanges();
}
void AddMixedNewAndExistingEntities_FromManySide()
{
    //Executed from the relationside : Many
    var docent4 = new Docent
    {
        Voornaam = "VoornaamBLAH",
        Familienaam = "Docent04",
        Wedde = 4444,
        LandCode = "GB"
    };
    using var context = new EFOpleidingenContext();
    var campus1 = context.Campussen.Find(1);
    if (campus1 != null)
    {
        context.Docenten.Add(docent4);
        docent4.Campus = campus1;
        context.SaveChanges();
    }
    else
        Console.WriteLine("Campus 1 niet gevonden");


    var docent5 = new Docent
    {
        Voornaam = "VoornaamBLOH",
        Familienaam = "Docent05",
        Wedde = 5555,
        LandCode = "GB",
        CampusId = 1
    };
    context.Docenten.Add(docent5);
    context.SaveChanges();
}
void AddMixedNewAndExistingEntities_FromOneSide()
{
    //Executed from the relationside : One
    var docent = new Docent
    {
        Voornaam = "Voornaam06",
        Familienaam = "Docent06",
        Wedde = 6666,
        LandCode = "DE"
    };
    using var context = new EFOpleidingenContext();
    var campus = context.Campussen.Find(1);
    if (campus != null)
    {
        campus.Docenten.Add(docent);
        context.SaveChanges();
    }
    else
        Console.WriteLine("Campus 1 niet gevonden");
}

void UpdateOneEntity()
{
    Console.Write("DocentNr.:");
    if (int.TryParse(Console.ReadLine(), out int docentNr))
    {
        using var context = new EFOpleidingenContext();
        var docent = context.Docenten.Find(docentNr);
        if (docent is not null)
        {
            Console.WriteLine("Wedde:{0}", docent.Wedde);
            Console.Write("Bedrag:");
            if (decimal.TryParse(Console.ReadLine(), out decimal bedrag))
            {
                docent.Opslag(bedrag);
                context.SaveChanges();
            }
            else
                Console.WriteLine("Tik een getal");
        }
        else
            Console.WriteLine("Docent niet gevonden");
    }
    else
        Console.WriteLine("Tik een getal");
}
void UpdateSomeSelectedEntities()
{
    Console.Write("Bovengrens : ");
    if (int.TryParse(Console.ReadLine(), out int grens))
    {
        using var context = new EFOpleidingenContext();
        foreach (var docent in context.Docenten)
            if (docent.Wedde <= grens) docent.Opslag(100m);
        context.SaveChanges();
    }
    else
        Console.WriteLine("Tik een getal");
}
void UpdateAssociatedEntities()
{
    using var context = new EFOpleidingenContext();
    var campus1 = context.Campussen.Include("Docenten")
        .FirstOrDefault(c => c.CampusId == 1);          //FirstOrDefault needed because we use also .Include
    if (campus1 != null)
    {
        foreach (var docent in campus1.Docenten)
            docent.Opslag(10m);
        context.SaveChanges();
    }
}
void UpdateAssociationOfdEntity_FromManySide()
{
    //Executed from the relationside : Many,
    //By reading the docent, then the newly associated campus and associating it to the read docent using the Campus property
    using var context = new EFOpleidingenContext();
    var docent1 = context.Docenten.Find(1);
    if (docent1 is not null)
    {
        var campus6 = context.Campussen.Find(6);
        if (campus6 is not null)
        {
            docent1.Campus = campus6;
            context.SaveChanges();
        }
        else
            Console.WriteLine("Campus 6 niet gevonden");
    }
    else
        Console.WriteLine("Docent 1 niet gevonden");

    //Executed from the relationside : Many
    //By reading the docent, then changing the associated campus using the CampusId property
    using var context2 = new EFOpleidingenContext();
    var docent2 = context.Docenten.Find(1);
    if (docent2 is not null)
    {
        docent2.CampusId = 2;
        context.SaveChanges();
    }
    else
        Console.WriteLine("Docent 1 niet gevonden");
}
void UpdateAssociationOfdEntity_FromOneSide()
{
    //Executed from the relationside : One
    using var context = new EFOpleidingenContext();
    var docent1 = context.Docenten.Find(1);
    if (docent1 is not null)
    {
        var campus3 = context.Campussen.Find(3);
        if (campus3 is not null)
        {
            campus3.Docenten.Add(docent1);
            context.SaveChanges();
        }
        else
            Console.WriteLine("Campus 3 niet gevonden");
    }
    else
        Console.WriteLine("Docent 1 niet gevonden");
}

void RemoveOneEntity()
{
    Console.Write("Nummer docent:");
    if (int.TryParse(Console.ReadLine(), out int docentNr))
    {
        using var context = new EFOpleidingenContext();
        var docent = context.Docenten.Find(docentNr);
        if (docent != null)
        {
            context.Docenten.Remove(docent);
            context.SaveChanges();
        }
        else
            Console.WriteLine("Docent niet gevonden");
    }
    else
        Console.WriteLine("Tik een getal");
}

void RemoveOneEntityWithAssociatedEntities()
{
    using var context = new EFOpleidingenContext();
    var duitsland = context.Landen.Find("GB");
    if (duitsland != null)
    {
        context.Landen.Remove(duitsland);
        context.SaveChanges();
    }
}

void AddExampleData_TPH()
{
    //demonstrate the workings of a Table Per Hierachy configuration 
    using var context = new EFOpleidingenContext();
    context.TPHCursussen.Add(new TPHKlassikaleCursus
    {
        Naam = "duits in 24 uur",
        Van = DateTime.Today,
        Tot = DateTime.Today
    });
    context.TPHCursussen.Add(new TPHZelfstudieCursus
    {
        Naam = "chinees in 24 uur",
        AantalDagen = 1
    });
    context.SaveChanges();
}

void AddExampleData_TPT()
{
    //demonstrate the workings of a Table Per Type configuration 
    using var context = new EFOpleidingenContext();
    context.TPTCursussen.Add(new TPTKlassikaleCursus
    {
        Naam = "Frans in 24 uur",
        Van = DateTime.Today,
        Tot = DateTime.Today
    });
    context.TPTCursussen.Add(new TPTZelfstudieCursus
    {
        Naam = "Engels in 24 uur",
        AantalDagen = 1
    });
    context.SaveChanges();
}

void AddExampleData_TPC()
{
    //demonstrate the workings of a Table Per Concrete Class configuration 
    using var context = new EFOpleidingenContext();
    context.TPCCursussen.Add(new TPCKlassikaleCursus
    {
        Naam = "Frans in 24 uur",
        Van = DateTime.Today,
        Tot = DateTime.Today
    });
    context.TPCCursussen.Add(new TPCZelfstudieCursus
    {
        Naam = "Engels in 24 uur",
        AantalDagen = 1
    });
    context.SaveChanges();
}

void GetAllCursussenWithBoeken()
{
    using var context = new EFOpleidingenContext();
    var query = from cursus in context.Cursussen
                .Include("Boeken")
                orderby cursus.Naam
                select cursus;
    foreach (var cursus in query)
    {
        Console.WriteLine(cursus.Naam);
        foreach (var boek in cursus.Boeken)
            Console.WriteLine("\t{0} ({1})", boek.Titel, boek.IsbnNr);
    }
}

void AddNewBoekAndAddItToACursusAndGetAllCursussenWithBoeken()
{
    //example code to add a new entry to a table in a many to many relation without intermediary table (tussentabel)
    using var context = new EFOpleidingenContext();
    var nieuwBoek = new Boek()
    {
        IsbnNr = "0-201-70431-5",
        Titel = "Modern C++ Design"
    }; // (1) 
    context.Boeken.Add(nieuwBoek);
    context.Cursussen.Where(c => c.Naam == "C++").FirstOrDefault()
                     .Boeken.Add(nieuwBoek);
    context.SaveChanges();
    var query = from cursus in context.Cursussen.Include("Boeken")
                orderby cursus.Naam
                select cursus;
    foreach (var cursus in query)
    {
        Console.WriteLine(cursus.Naam);
        foreach (var boek in cursus.Boeken)
            Console.WriteLine("\t{0} ({1})", boek.Titel, boek.IsbnNr);
    };
}

void AddNewDocentenAktiviteitenAndDocentAktiviteiten()
{
    //example code to add data to 2 tables in a many - many relation, with extra data in the intermediary table (tussentabel)

    //create a new docent, campus & aktiviteiten
    using var context = new EFOpleidingenContext();
    var campus = new Campus
    {
        Naam = "CC Roeselare",
        Adres = new Adres
        {
            Straat = "Pildersweg",
            Huisnummer = "7",
            Postcode = "8800",
            Gemeente = "Roeselare"
        }
    };
    var jean = new Docent
    {
        Voornaam = "Jean",
        Familienaam = "Smits",
        Wedde = 1000,
        InDienst = new DateTime(1966, 8, 1),
        HeeftRijbewijs = true,
        ThuisAdres = new Adres
        {
            Straat = "Keizerslaan",
            Huisnummer = "11",
            Postcode = "1000",
            Gemeente = "Brussel"
        },
        LandCode = "BE",
        Campus = campus
    };
    var kiekeboe = new Docent
    {
        Voornaam = "Marcel",
        Familienaam = "Kiekeboe",
        Wedde = 500,
        InDienst = new DateTime(1948, 10, 24),
        HeeftRijbewijs = true,
        ThuisAdres = new Adres
        {
            Straat = "Merholaan",
            Huisnummer = "1B",
            Postcode = "3000",
            Gemeente = "Zoersel"
        },
        LandCode = "BE",
        Campus = campus
    };
    var activiteit1 = new Activiteit { Naam = "EHBO" };
    var activiteit2 = new Activiteit { Naam = "Vergaderen" };
    var activiteit3 = new Activiteit { Naam = "Overleggen" };
    var activiteit4 = new Activiteit { Naam = "Studie" };

    //add and save the new data
    context.Campussen.Add(campus);
    context.SaveChanges();
    context.Docenten.Add(jean);
    context.Docenten.Add(kiekeboe);
    context.SaveChanges();
    context.Activiteiten.Add(activiteit1);
    context.Activiteiten.Add(activiteit2);
    context.Activiteiten.Add(activiteit3);
    context.Activiteiten.Add(activiteit4);
    context.SaveChanges();

    //create, add and save new docentaktiviteiten by using their ids
    var da1 = new DocentActiviteit
    {
        DocentId = jean.DocentId,
        ActiviteitId = activiteit2.ActiviteitId,
        AantalUren = 4
    };
    var da2 = new DocentActiviteit
    {
        DocentId = jean.DocentId,
        ActiviteitId = activiteit3.ActiviteitId,
        AantalUren = 1
    };
    var da3 = new DocentActiviteit
    {
        DocentId = kiekeboe.DocentId,
        ActiviteitId = activiteit4.ActiviteitId,
        AantalUren = 4
    };
    var da4 = new DocentActiviteit
    {
        DocentId = kiekeboe.DocentId,
        ActiviteitId = activiteit1.ActiviteitId,
        AantalUren = 1
    };
    var da5 = new DocentActiviteit
    {
        DocentId = kiekeboe.DocentId,
        ActiviteitId = activiteit2.ActiviteitId,
        AantalUren = 2
    };
    context.AddRange(da1, da2, da3, da4, da5);
    context.SaveChanges();

    //add a new docentaktiviteit by finding a docent and attaching him to the new docentaktiviteit
    var docent = context.Docenten.Find(4);
    docent.DocentenActiviteiten.Add(
    new DocentActiviteit { ActiviteitId = 3, AantalUren = 6 });
    context.SaveChanges();
}

void GetAllDocentenAndAktiviteiten()
{
    //display the docentaktiviteiten by iterating over each docent, and if they have aktiviteiten, show them + hours
    using var context = new EFOpleidingenContext();
    foreach (var d in context.Docenten.Include("DocentenActiviteiten.Activiteit"))// include aktiviteiten, we will need them
    {
        Console.WriteLine(d.Naam);
        if (d.DocentenActiviteiten.Count == 0)
            Console.WriteLine("\tGeen activiteiten");
        else
            foreach (var da in d.DocentenActiviteiten)
                Console.WriteLine("\t{0} {1} uur", da.Activiteit.Naam, da.AantalUren);

        Console.WriteLine();
    }
}

void AddNewWerknemers()
{
    using var context = new EFOpleidingenContext();
    Werknemer joe = new Werknemer { Voornaam = "Joe", Familienaam = "Dalton" };
    Werknemer averell = new Werknemer { Voornaam = "Averell", Familienaam = "Dalton", Overste = joe };
    context.Werknemers.Add(joe);
    context.Werknemers.Add(averell);
    context.SaveChanges();
}
void GetWerknemers1_ToonWerknemersZonderOverste()
{
    using var context = new EFOpleidingenContext();
    var query = from werknemer in context.Werknemers
                where werknemer.Overste == null
                orderby werknemer.Voornaam, werknemer.Familienaam
                select werknemer;
    foreach (var werknemer in query)
        Console.WriteLine($"{werknemer.Voornaam} {werknemer.Familienaam}");
}
void GetWerknemers2_ToonWerknemersMetOverste_ToonOokOverste()
{
    using var context = new EFOpleidingenContext();
    var query = from werknemer in context.Werknemers.Include("Overste")
                where werknemer.Overste != null
                orderby werknemer.Voornaam, werknemer.Familienaam
                select werknemer;
    foreach (var werknemer in query)
        Console.WriteLine(
            $"Werknemer : {werknemer.Voornaam} {werknemer.Familienaam}" +
            $", Overste : {werknemer.Overste.Voornaam} {werknemer.Overste.Familienaam} "
        );
}
void GetWerknemers3_ToonOverstenMetHunOndergeschikten()
{
    using var context = new EFOpleidingenContext();
    var query = from overste in context.Werknemers.Include("Ondergeschikten")
                where overste.Ondergeschikten.Count != 0
                orderby overste.Voornaam, overste.Familienaam
                select overste;
    foreach (var overste in query)
    {
        Console.WriteLine("Overste : " + overste.Voornaam + " " + overste.Familienaam);
        foreach (var werknemer in overste.Ondergeschikten)
            Console.WriteLine("\tWerknemer : " + werknemer.Voornaam + " " + werknemer.Familienaam);
    }
}
void UpdateWerknemers_MakeWerknemer5OversteVanWerknemer6()
{
    using var context = new EFOpleidingenContext();
    var werknemer5 = context.Werknemers.Find(5);
    if (werknemer5 != null)
    {
        var werknemer6 = context.Werknemers.Find(6);
        if (werknemer6 != null)
        {
            werknemer5.Ondergeschikten.Add(werknemer6);
            context.SaveChanges();
        }
        else
            Console.WriteLine("Werknemer 6 niet gevonden.");
    }
    else
        Console.WriteLine("Werknemer 5 niet gevonden.");
}
#endregion