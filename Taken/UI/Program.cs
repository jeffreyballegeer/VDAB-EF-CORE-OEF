﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.IdentityModel.Tokens;
using Model.Entities;
using System.Collections.Immutable;
using System.Net.Http.Headers;

//var context = new EFTakenContext();

// Excercise 6.13: Show all customers including any accounts with balances and total balance. Use eager loading

//foreach (var klant in FindAllKlantenAndRekeningen())
//{
//    Console.WriteLine(klant.Voornaam);
//    decimal totaal = Decimal.Zero;
//    foreach (var rekening in klant.Rekeningen)
//    {
//        Console.WriteLine($"{rekening.RekeningNr}: {rekening.Saldo}");
//        totaal += rekening.Saldo;
//    }
//    Console.WriteLine($"Totaal: {totaal}{Environment.NewLine}");
//}

//List<Klant> FindAllKlantenAndRekeningen()
//{
//    using var context = new EFTakenContext();
//    return (from Klant in context.Klanten.Include("Rekeningen")
//            orderby Klant.Voornaam
//            select Klant).ToList();
//}


// Excercise 7.6 : Voeg nieuw rekeningnr toe aan bestaande klant

//List<Klant> FindAllKlanten()
//{
//    using var context = new EFTakenContext();
//    return (from Klant in context.Klanten
//            orderby Klant.Voornaam
//            select Klant).ToList();
//}

//foreach (var klant in FindAllKlanten())
//{
//    Console.WriteLine($"KlantNr {klant.KlantNr} - {klant.Voornaam}");
//}

//if (int.TryParse(Console.ReadLine(), out int klantNr))
//{
//    var klant = context.Klanten.Find(klantNr);
//    if (klant != null)
//    {
//        Console.WriteLine("Geef een nieuw rekeningnummer in voor deze klant");
//        string nieuwRekeningNr = Console.ReadLine() ?? string.Empty;
//        if (!nieuwRekeningNr.IsNullOrEmpty())
//        {
//            Rekening rekening = new Rekening
//            {
//                //Klant = klant,
//                RekeningNr = nieuwRekeningNr,
//                KlantNr = klant.KlantNr,
//                Saldo = Decimal.Zero,
//                Soort = 'Z'
//            };
//            context.Rekeningen.Add(rekening);
//            context.SaveChanges();
//        }
//        else
//            Console.WriteLine($"Gelieve een rekeningnummer in te geven.");
//    }
//    else
//        Console.WriteLine($"Klant niet gevonden");
//}
//else
//    Console.WriteLine("Tik een getal");


//Excercise 8.5 : Allow deposits to account

Console.WriteLine("Geef een rekeningnr");
string gewenstRekeningNr = Console.ReadLine();
if (!gewenstRekeningNr.IsNullOrEmpty())
{
    using var context = new EFTakenContext();
    var gevondenRekeningNr = context.Rekeningen.Find(gewenstRekeningNr);
    if (gevondenRekeningNr != null)
    {
        Console.WriteLine("Welk bedrag moet gestort worden?");
        if (decimal.TryParse(Console.ReadLine(), out var stortBedrag))
        {
            if (stortBedrag > Decimal.Zero)
            {
                gevondenRekeningNr.Storten(stortBedrag);
                context.SaveChanges();
                Console.WriteLine("Gestort!");
            }
            else
                Console.WriteLine("Te storten bedrag moet meer zijn dan 0");
        }
        else
            Console.WriteLine("het te storten bedrag moet een getal zijn");
    }
    else Console.WriteLine("Het ingevoerde rekeningnr bestaat niet!");
}
else
    Console.WriteLine("U heeft niets ingevoerd");