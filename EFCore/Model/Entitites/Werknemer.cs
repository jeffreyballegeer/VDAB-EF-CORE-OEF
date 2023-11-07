using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Werknemer
    {
        [Key]
        public int WerknemerId { get; set; }
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }
        public ICollection<Werknemer> Ondergeschikten { get; set; } = new List<Werknemer>();
        [InverseProperty("Ondergeschikten")]
        [ForeignKey("OversteId")] // Specify the property on the other side of the association
        public Werknemer? Overste { get; set; }
        public int? OversteId { get; set; } // nullable : not everyone has an overste
    }
}
