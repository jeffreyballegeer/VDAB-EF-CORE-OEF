using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    [Table("Personeelsleden")]
    public class Personeelslid
    {
        [Key]
        public int PersoneelsNr { get; set; }
        public string Voornaam { get; set; }
        public int? ManagerNr { get; set; }
        public ICollection<Personeelslid> Ondergeschikten { get; set; }
        = new List<Personeelslid>();
        [InverseProperty("Ondergeschikten")]
        [ForeignKey("ManagerNr")]
        public Personeelslid? Manager { get; set; }
    }
}
