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
    public class Land
    {
        private readonly ILazyLoader lazyLoader;

        public Land(ILazyLoader lazyLoader)                     // lazyloader by dependency injection
        {
            this.lazyLoader = lazyLoader;
        }
        public Land()                                           // empty ctor used in EFOpleidingenContext seeding
        {
            Docenten = new List<Docent>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]  //geen autonummering
        public string LandCode { get; set; }
        public string Naam { get; set; }

        private ICollection<Docent> docenten;
        public ICollection<Docent> Docenten                     //nav prop
        {
            get => lazyLoader.Load(this, ref docenten);
            set => docenten = value; 
        } 
    }
}
