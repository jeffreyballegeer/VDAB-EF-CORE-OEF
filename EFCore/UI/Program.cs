using Microsoft.EntityFrameworkCore;
using Model.Entitites;

//Code to show all docenten from db
//using var context = new EFOpleidingenContext();
//foreach (var docent in context.Docenten)
//    Console.WriteLine(docent.Naam);

// Log SQL Query's to console
//Console.WriteLine(context.Docenten.ToQueryString());
//Console.WriteLine(context.Campussen.ToQueryString());

Console.Write("Minimum wedde:");
if (decimal.TryParse(Console.ReadLine(), out decimal minWedde))
{
    using var context = new EFOpleidingenContext();
    var query = from docent in context.Docenten
                where docent.Wedde >= minWedde
                orderby docent.Voornaam, docent.Familienaam
                select docent;
    foreach (var docent in query)
        Console.WriteLine("{0}: {1}", docent.Naam, docent.Wedde);
}
else
    Console.WriteLine("Geef een getal in !");

