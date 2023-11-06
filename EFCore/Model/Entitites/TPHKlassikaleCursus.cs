using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class TPHKlassikaleCursus : TPHCursus
    {
        public DateTime Van { get; set; }
        public DateTime Tot { get; set; }
    }
}
