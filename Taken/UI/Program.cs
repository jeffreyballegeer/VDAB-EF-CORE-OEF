using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.IdentityModel.Tokens;
using Model.Entities;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Transactions;

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

//Console.WriteLine("Geef een rekeningnr");
//string gewenstRekeningNr = Console.ReadLine();
//if (!gewenstRekeningNr.IsNullOrEmpty())
//{
//    using var context = new EFTakenContext();
//    var gevondenRekeningNr = context.Rekeningen.Find(gewenstRekeningNr);
//    if (gevondenRekeningNr != null)
//    {
//        Console.WriteLine("Welk bedrag moet gestort worden?");
//        if (decimal.TryParse(Console.ReadLine(), out var stortBedrag))
//        {
//            if (stortBedrag > Decimal.Zero)
//            {
//                gevondenRekeningNr.Storten(stortBedrag);
//                context.SaveChanges();
//                Console.WriteLine("Gestort!");
//            }
//            else
//                Console.WriteLine("Te storten bedrag moet meer zijn dan 0");
//        }
//        else
//            Console.WriteLine("het te storten bedrag moet een getal zijn");
//    }
//    else Console.WriteLine("Het ingevoerde rekeningnr bestaat niet!");
//}
//else
//    Console.WriteLine("U heeft niets ingevoerd");


//-------------------
//excercise 9.3 : Delete customer if it has no accountnumbers
//--> My TRY : had no idea how to include the rekeningen of a customer so solved with extra db call. (see course example code below my code)
//int rekeningenVanklant(int klantId)
//{
//    using var context2 = new EFTakenContext();
//    return context2.Rekeningen
//        .Where(r => r.KlantNr == klantId)
//        .Count();
//}

//using var context = new EFTakenContext();
//Console.WriteLine("Beschikbare klanten : ");
//foreach (Klant klant in context.Klanten)
//    Console.WriteLine($"{klant.KlantNr} - {klant.Voornaam}");

//Console.WriteLine("Welk klantnr moet verwijderd worden?");
//if (int.TryParse(Console.ReadLine(), out int klantNr))
//{
//    var teVerwijderenKlant = context.Klanten.Find(klantNr);
//    if (teVerwijderenKlant != null)
//    {
//        if (rekeningenVanklant(klantNr) == 0)
//        {
//            context.Klanten.Remove(context.Klanten.Find(klantNr));
//            context.SaveChanges();
//            Console.WriteLine($"klantnr {klantNr} is verwijderd.");
//        }
//        else
//            Console.WriteLine($"klantnr {klantNr} heeft {rekeningenVanklant(klantNr)} rekening(en) en kan niet verwijderd worden.");
//    }
//    else
//        Console.WriteLine($"Klantnt {klantNr} bestaat niet");
//}
//else
//    Console.WriteLine("Dit is geen nummer");

// COURSE EXAMPLE ->
//Console.Write("KlantNr:");
//if (int.TryParse(Console.ReadLine(), out var klantNummer))
//{
//    using var entities = new EFTakenContext();
//    var klant = entities.Klanten.Include("Rekeningen")
//    .FirstOrDefault(k => k.KlantNr == klantNummer);
//    if (klant is null)
//        Console.WriteLine("Klant niet gevonden");
//    else
//    {
//        if (klant.Rekeningen.Count != 0)
//            Console.WriteLine("Klant heeft nog rekeningen");
//        else
//        {
//            entities.Klanten.Remove(klant);
//            entities.SaveChanges();
//        }
//    }
//}
//else
//    Console.WriteLine("Tik een getal");


//-------------
//Excercise 12.3 : Show all personeelsleden + ondergeschikten starting with highest in the hierachy
//Couse example : 
//using var context = new EFTakenContext();
//var hoogstenInHierarchie =
//(from personeelslid in context.Personeelsleden
// where personeelslid.Manager == null
// select personeelslid).ToList();
//Afbeelden(hoogstenInHierarchie, 0);
//void Afbeelden(List<Personeelslid> personeel, int insprong)
//{
//    foreach (var personeelslid in personeel)
//    {
//        Console.Write(new String('\t', insprong));
//        Console.WriteLine(personeelslid.Voornaam);
//        if (personeelslid.Ondergeschikten.Count != 0)
//            Afbeelden(personeelslid.Ondergeschikten.ToList(), insprong + 1);
//    }
//}


//-----------------
//Excercise 12.4 : Build classes & database according to entity layout, add some data to database
//Course example:
//using var context = new EFTakenContext();
//var soepterrine = new NonFoodArtikel()
//{
//    Naam = "Villeroy & Boch",
//    Garantie = 24
//};
//var grasmachine = new NonFoodArtikel()
//{
//    Naam = "SABO 40-spirit",
//    Garantie = 60
//};
//var frietpatatjes = new FoodArtikel()
//{
//    Naam = "Frietaardappelen 5kg",
//    Houdbaarheid = 1
//};

//var tuin = new Artikelgroep { Naam = "Tuinartikelen" };
//var keuken = new Artikelgroep { Naam = "Keukenartikelen" };
//tuin.Artikels.Add(grasmachine);
//keuken.Artikels.Add(soepterrine);
//keuken.Artikels.Add(frietpatatjes);
//context.Artikelgroepen.Add(tuin);
//context.Artikelgroepen.Add(keuken);
//context.SaveChanges();

//------------
//Excercise 
Console.Write("RekeningNr. van rekening:");
var vanRekeningNr = Console.ReadLine();
Console.Write("RekeningNr. naar rekening:");
var naarRekeningNr = Console.ReadLine();
try
{
    Console.Write("Bedrag:");
    var bedrag = decimal.Parse(Console.ReadLine());
    if (bedrag <= decimal.Zero)
        Console.WriteLine("Tik een positief bedrag");
    else
    {
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.RepeatableRead
        };
        using var transactionScope = new TransactionScope(
        TransactionScopeOption.Required, transactionOptions);
        using var entities = new EFTakenContext();
        var vanRekening = entities.Rekeningen.Find(vanRekeningNr);
        if (vanRekening == null)
            Console.WriteLine("Van rekening niet gevonden");
        else
        {
            var naarRekening = entities.Rekeningen.Find(naarRekeningNr);
            if (naarRekening == null)
                Console.WriteLine("Naar rekening niet gevonden");
            else
                try
                {
                    vanRekening.Overschrijven(naarRekening, bedrag);
                    entities.SaveChanges();
                    transactionScope.Complete();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
catch (FormatException)
{
    Console.WriteLine("Tik een bedrag");
}