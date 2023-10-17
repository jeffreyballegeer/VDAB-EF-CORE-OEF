using Microsoft.EntityFrameworkCore;
using Model.Entities;

var context = new EFTakenContext();
foreach (Klant klant in context.Klanten)
{

}
Console.WriteLine(context.Klanten.ToQueryString());