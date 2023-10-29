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
        private readonly ILazyLoader lazyLoader;
        public Campus(ILazyLoader lazyLoader)       // lazyload by dependency injection
        {
            this.lazyLoader = lazyLoader;
        }
        public Campus()
        {
            Docenten = new List<Docent>();
        }

        public int CampusId { get; set; }
        [Required]
        [Column("CampusNaam")]
        public string Naam { get; set; }
        public string Straat { get; set; }
        public string Huisnummer { get; set; }
        public string Postcode { get; set; }
        [StringLength(50)]
        public string Gemeente { get; set; }
        [NotMapped]
        public string Commentaar { get; set; }

        // navigation property (in een campus werken meerdere docenten) 
        private ICollection<Docent> docenten;
        public ICollection<Docent> Docenten {
            get => lazyLoader.Load(this, ref docenten);
            set => docenten = value;

                }
    }
}
