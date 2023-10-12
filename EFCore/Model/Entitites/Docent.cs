using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public partial class Docent
    {
        public int DocentId { get; set; }
        [Required]
        [StringLength(20)]
        public string Voornaam { get; set; }
        [Required]
        [StringLength(30)]
        public string Familienaam { get; set; }
        [Column("Maandwedde")]
        [Precision(18, 2)]
        public decimal Wedde { get; set; }
        [Column(TypeName = "date")]
        public DateTime InDienst { get; set; }
        public bool? HeeftRijbewijs { get; set; }
        [ForeignKey("Land")]
        public string LandCode { get; set; }
        public int CampusId { get; set; }
        public Campus Campus { get; set; }  //nav prop
        public Geslacht Geslacht { get; set; }
        public Land Land { get; set; }
    }
}
