using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Voorraad
    {
        [ConcurrencyCheck] //of modelBuilder.Entity<Voorraad>().Property(a => a.MagazijnNr).IsConcurrencyToken(); 
        public int MagazijnNr { get; set; }
        [ConcurrencyCheck]
        public int CursusNr { get; set; }
        [Required]
        [ConcurrencyCheck]
        public int AantalStuks { get; set; }
        [Required]
        [ConcurrencyCheck]
        public int RekNr { get; set; }
    }
}
