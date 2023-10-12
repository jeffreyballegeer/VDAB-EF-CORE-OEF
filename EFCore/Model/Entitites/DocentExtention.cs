using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public partial class Docent
    {
        public string Naam
        {
            get { return Voornaam + " " + Familienaam; }
        }
        [Column(TypeName = "date")]
        public DateTime Geboortedatum { get; set; }
    }
}
