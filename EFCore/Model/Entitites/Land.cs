using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Land
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] //geen autonummering
        public string LandCode { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<Docent> Docenten { get; set; } // virtual for Lazy loading with proxies
    }
}
