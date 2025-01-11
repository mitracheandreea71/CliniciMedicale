using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class AsigurareModel
    {
        public string NumeAsigurator { get; set; }
        public string NumeAsigurare { get; set; }
        public DateTime? DataAsigurare { get; set; }
        public DateTime? DataExpirare { get; set; }
        int IdPacient { get; set; }

        private readonly CliniciDataContext _context;

        public AsigurareModel()
        { 
            _context= new CliniciDataContext();
        }

        public bool isPacientInsured(int idPacient)
        {
            if (_context.Asigurares.Where(a => a.id_pacient == idPacient).Any())
                return true;
            return false;
        }
        public AsigurareModel getAsigurareByPacientId(int idPacient)
        {
            if (!_context.Asigurares.Where(a => a.id_pacient == idPacient).Any())
                return null;
            var asigurare = _context.Asigurares.Where(a => a.id_pacient == idPacient).First();

            if (!_context.Asiguraris.Where(asig => asig.id_asigurator == asigurare.id_asigurator).Any())
                return null;
            var asigurator = _context.Asiguraris.Where(asig => asig.id_asigurator == asigurare.id_asigurator).First();
            if (asigurator == null || asigurare == null)
                return null;
            AsigurareModel asigurareRet = new AsigurareModel
            {
                IdPacient= idPacient,
                NumeAsigurare=asigurator.nume_asigurare,
                NumeAsigurator=asigurator.nume_asigurator,
                DataAsigurare=asigurare.data_asigurare,
                DataExpirare=asigurare.data_expirare
            };
            return asigurareRet;
        }
    }
}
