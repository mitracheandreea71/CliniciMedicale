using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    internal class PacientModel
    {
        public int IDPacient {  get; set; }
        public string Nume {  get; set; }
        public string Prenume { get; set; } 
        public string Judet { get; set; }
        public string Adresa {  get; set; }

        CliniciDataContext _context;

        public PacientModel()
        { 
            _context = new CliniciDataContext();
        }

        public PacientModel GetPacientByID(int idPacient)
        { 
            PacientModel pacient = new PacientModel();

            var p = _context.Pacients.Where(p => p.id_pacient == idPacient).First();

            pacient.IDPacient = p.id_pacient;
            pacient.Nume = p.nume;
            pacient.Prenume = p.prenume;
            pacient.Judet = p.judet;
            pacient.Adresa = p.adresa;

            return pacient;
        }

    }
}
