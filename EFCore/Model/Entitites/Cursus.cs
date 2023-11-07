using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Cursus
    {
        public Cursus()
        {
            Boeken = new List<Boek>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CursusNr { get; set; }
        public string Naam { get; set; }
        public ICollection<Boek> Boeken { get; set; }
    }
}
