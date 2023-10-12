using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    internal class EFOpleidingenContext : DbContext
    {
        public static IConfigurationRoot configuration;
        public DbSet<Campus> Campussen { get; set; }
        public DbSet<Docent> Docenten { get; set; }

        // /* CONNECTIONSTRING HARDCODED : */
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //    "Server=.\\SQLExpress;Database=EFOpleidingen;" +
        //    "Trusted_Connection=true;encrypt=false",    //trusted connection = windowsgebruiker als login
        //    options => options.MaxBatchSize(150));      // # sql commands die in 1* naar de db gestuurd kunnen worden
        //}

        /* Connectionstring in configfile */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName) // locatie van config bestand
            .AddJsonFile("appsettings.json", false).Build();        // naam van config bestand
            var connectionString =
            configuration.GetConnectionString("EFOpleidingen");     // haal connectiestring uit configfile
            if (connectionString != null)
                optionsBuilder.UseSqlServer(connectionString,
                options => options.MaxBatchSize(150));
        }
    }
}
