using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class EFTakenContext : DbContext
    {
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Rekening> Rekeningen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            "Server=.\\SQLExpress;Database=EFBank;" +
            "Trusted_Connection=true;encrypt=false",    //trusted connection = windowsuser als login
            options => options.MaxBatchSize(150));      // # sql commands die in 1* naar de db gestuurd kunnen worden
        }
    }
}
