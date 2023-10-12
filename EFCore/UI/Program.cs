using Model.Entitites;

using var context = new EFOpleidingenContext();
foreach (var docent in context.Docenten)
    Console.WriteLine(docent.Naam);