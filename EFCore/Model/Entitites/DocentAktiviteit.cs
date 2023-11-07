using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    [Table("DocentenActiviteiten")]
    public class DocentActiviteit
    {
        public int DocentId { get; set; }       // Key
        public int ActiviteitId { get; set; }   // Key
        public int AantalUren { get; set; }
        public Docent Docent { get; set; }
        public Activiteit Activiteit { get; set; }
    }
}
