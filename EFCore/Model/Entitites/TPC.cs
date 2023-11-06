using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public abstract class TPCCursus
    {
        public int Id { get; set; }
        public string Naam { get; set; }
    }
    public class TPCZelfstudieCursus : TPCCursus
    {
        public int AantalDagen { get; set; }
    }
    public class TPCKlassikaleCursus : TPCCursus
    {
        public DateTime Van { get; set; }
        public DateTime Tot { get; set; }
    }
}
