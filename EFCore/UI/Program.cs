using Microsoft.EntityFrameworkCore;
using Model.Entitites;


//GetAllDocentsFromDb_LogSQLToConsole();
//GetAllDocentsWithMinimumWage_UsingLinq();
//GetAllDocentsWithMinimumWage_UsingQueryMethods();
//GetDocentOnPrimaryKey_UsingFind();
//GetPartialObjects_UsingLINQ();
//GetPartialObjects_UsingQueryMethods();

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


#endregion