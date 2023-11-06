﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    public class Adres
    {
        #region ctors
        public Adres() { }
        public Adres(string straat, string huisnummer,
        string postcode, string gemeente)
        {
            Straat = straat;
            Huisnummer = huisnummer;
            Postcode = postcode;
            Gemeente = gemeente;
        }
        #endregion

        public string? Straat { get; set; }
        public string? Huisnummer { get; set; }
        public string? Postcode { get; set; }
        public string? Gemeente { get; set; }
        public string? Adreslijn => $"{Straat} {Huisnummer}\n{Postcode} {Gemeente}";
    }
}
