using Microsoft.EntityFrameworkCore;
using Model.Entitites;

using var context = new EFOpleidingenContext();
foreach (var docent in context.Docenten)
    Console.WriteLine(docent.Naam);
//Console.WriteLine(context.Docenten.ToQueryString());  // Log SQL query to console 
//Console.WriteLine(context.Campussen.ToQueryString()); // Log SQL query to console 