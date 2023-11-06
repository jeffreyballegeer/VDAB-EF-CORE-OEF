using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Model.Entitites
{
    public class Campus
    {
        public Campus()
        {
            Docenten = new List<Docent>();
        }

        public int CampusId { get; set; }
        [Required]
        [Column("CampusNaam")]
        public string Naam { get; set; }

        public Adres Adres { get; set; }
        //public string Straat { get; set; }
        //public string Huisnummer { get; set; }
        //public string Postcode { get; set; }
        //[StringLength(50)]
        //public string Gemeente { get; set; }
        [NotMapped]
        public string Commentaar { get; set; }

        public ICollection<Docent> Docenten { get; set; }// navigation property (in een campus werken meerdere docenten) 
    }
}
