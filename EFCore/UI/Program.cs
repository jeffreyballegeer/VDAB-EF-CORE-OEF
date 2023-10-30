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
UpdateSomeSelectedEntities();

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
        Straat = "Straat01",
        Huisnummer = "1",
        Postcode = "1111",
        Gemeente = "Gemeente01"
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
        Straat = "Straat02",
        Huisnummer = "2",
        Postcode = "2222",
        Gemeente = "Gemeente02"
    };
    var campus3 = new Campus
    {
        Naam = "Campus03",
        Straat = "Straat03",
        Huisnummer = "3",
        Postcode = "3333",
        Gemeente = "Gemeente03"
    };
    var campus4 = new Campus
    {
        Naam = "Campus04",
        Straat = "Straat04",
        Huisnummer = "4",
        Postcode = "4444",
        Gemeente = "Gemeente04"
    };
    var campus5 = new Campus
    {
        Naam = "Campus05",
        Straat = "Straat05",
        Huisnummer = "5",
        Postcode = "5555",
        Gemeente = "Gemeente05"
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
        Straat = "Straat07",
        Huisnummer = "7",
        Postcode = "7777",
        Gemeente = "Gemeente07"
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
        Straat = "Straat08",
        Huisnummer = "8",
        Postcode = "8888",
        Gemeente = "Gemeente08"
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
        Voornaam = "Voornaam04",
        Familienaam = "Docent04",
        Wedde = 4444,
        LandCode = "IT"
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
        Voornaam = "Voornaam05",
        Familienaam = "Docent05",
        Wedde = 5555,
        LandCode = "LU",
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
#endregion