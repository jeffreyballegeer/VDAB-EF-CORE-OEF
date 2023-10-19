using Microsoft.EntityFrameworkCore;
using Model.Entities;

var context = new EFTakenContext();

Console.WriteLine(context.Klanten.ToQueryString());