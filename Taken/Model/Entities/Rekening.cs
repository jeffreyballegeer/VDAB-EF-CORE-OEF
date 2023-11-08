using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public class Rekening
    {
        public Rekening() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RekeningNr { get; set; }

        public int KlantNr { get; set; }
        public decimal Saldo { get; set; }
        public char Soort { get; set; }
        [ForeignKey("KlantNr")]
        public Klant Klant { get; set; } // nav prop

        public void Storten(decimal bedrag)
        {
            Saldo += bedrag;
        }
    }
}
