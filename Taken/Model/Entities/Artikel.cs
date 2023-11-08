using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public abstract class Artikel
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public virtual Artikelgroep Artikelgroep { get; set; }
        public int ArtikelgroepId { get; set; }
    }
}
