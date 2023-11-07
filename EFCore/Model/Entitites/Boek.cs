using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Boek
    {
        public Boek()
        {
            Cursussen = new List<Cursus>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoekNr { get; set; }
        public string IsbnNr { get; set; }
        public string Titel { get; set; }
        public ICollection<Cursus> Cursussen { get; set; }
    }
}
