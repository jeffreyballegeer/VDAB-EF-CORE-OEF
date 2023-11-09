using Model.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Services
{
    public class DocentService
    {
        private EFOpleidingenContext context;
        public DocentService(EFOpleidingenContext context)
        {
            this.context = context;
        }
        public void ToevoegenDocent(Docent docent)
        {
            if (docent.LandCode == string.Empty || docent.LandCode == null)
                docent.LandCode = "BE";
            context.Docenten.Add(docent);
        }
        public IEnumerable<Docent> GetDocentenVoorCampus(int campus)
        {
            return context.Docenten.Where(x => x.CampusId == campus).ToList();
        }
        public Docent GetDocent(int id)
        {
            //opvragen van docent id 0 = exception
            if (id == 0)
            {
                throw new ArgumentException(nameof(id));
            }
            return context.Docenten.Single(x => x.DocentId == id);
        }
    }
}
