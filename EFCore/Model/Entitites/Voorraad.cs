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
        public int MagazijnNr { get; set; }
        public int CursusNr { get; set; }
        [Required]
        public int AantalStuks { get; set; }
        [Required]
        public int RekNr { get; set; }
    }
}
