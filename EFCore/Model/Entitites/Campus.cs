using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Campus
    {
        public int CampusId { get; set; }
        public string Naam { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public ICollection<Docent> Docenten { get ; set; } // navigation property (in een campus werken meerdere docenten)
    }
}
