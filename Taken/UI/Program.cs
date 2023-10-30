using Microsoft.EntityFrameworkCore;
using Model.Entities;
using System.Net.Http.Headers;

var context = new EFTakenContext();

//Console.WriteLine(context.Klanten.ToQueryString());

//foreach (var rekening in FindAllRekeningen())
//{
//    Console.WriteLine(rekening.Klant.Voornaam);
//    foreach (var rekening in )
//    Console.WriteLine($"{rekening.RekeningNr}: {rekening.Saldo}");
//    Console.WriteLine($"Totaal: {rekening.RekeningNr}");
//}

//List<Rekening> FindAllRekeningen()
//{
//    using var context = new EFTakenContext();
//    return (from rekening in context.Rekeningen.Include("Klant")
//            orderby rekening.Klant.Voornaam
//            select rekening).ToList();
//}


foreach (var klant in FindAllKlantenAndRekeningen())
{
    Console.WriteLine(klant.Voornaam);
    decimal totaal = Decimal.Zero;
    foreach (var rekening in klant.Rekeningen)
    {
        Console.WriteLine($"{rekening.RekeningNr}: {rekening.Saldo}");
        totaal += rekening.Saldo;
    }
    Console.WriteLine($"Totaal: {totaal}{Environment.NewLine}");
}

List<Klant> FindAllKlantenAndRekeningen()
{
    using var context = new EFTakenContext();
    return (from Klant in context.Klanten.Include("Rekeningen")
            orderby Klant.Voornaam
            select Klant).ToList();
}