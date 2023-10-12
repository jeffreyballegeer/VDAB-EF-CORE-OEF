using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitites
{
    internal class EFOpleidingenContext : DbContext
    {
        public DbSet<Campus> Campussen { get; set; }
        public DbSet<Docent> Docenten { get; set; }
    }
}
