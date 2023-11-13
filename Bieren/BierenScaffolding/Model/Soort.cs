using System;
using System.Collections.Generic;

namespace Model;

public partial class Soort
{
    public int SoortNr { get; set; }

    public string SoortNaam { get; set; } = null!;

    public virtual ICollection<Bier> Bieren { get; set; } = new List<Bier>();
}
