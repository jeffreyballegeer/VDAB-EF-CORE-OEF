using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    //Contains all the classes to demonstrate Table Per Type 
    public abstract class TPTCursus
    {
        public int Id { get; set; }
        public string Naam { get; set; }
    }
    public class TPTZelfstudieCursus : TPTCursus
    {
        public int AantalDagen { get; set; }
    }
    public class TPTKlassikaleCursus : TPTCursus
    {
        public DateTime Van { get; set; }
        public DateTime Tot { get; set; }
    }

}
