using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Docent
    {
        public int DocentId { get; set; }
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }
        public decimal Wedde { get; set; }
        public Campus Campus { get; set; } // navigation property
    }
}
