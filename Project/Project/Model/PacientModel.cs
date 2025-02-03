using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class PacientModel
    {
        public int IDPacient { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Judet { get; set; }
        public string Adresa { get; set; }
        public string Email { get; set; }
        public string Parola { get; set; }
        public string CNP { get; set; }

        public string Activ { get; set; }

        CliniciEntities _context;

        public PacientModel()
        { 
            _context = new CliniciEntities();
        }

        public void AdaugaPacientInBazaDeDate() 
        {
            int id = _context.Pacients.OrderByDescending(p => p.id_pacient).Select(p => p.id_pacient).First();
            var pacient = new Pacient
            {
                id_pacient = id + 1,
                nume = Nume,
                prenume = Prenume,
                judet = Judet,
                adresa = Adresa,
                email = Email,
                parola = Parola,
                activ = "ON",
                CNP = this.CNP,
            };
            _context.Pacients.Add(pacient);
            _context.SaveChanges();
        }
        public PacientModel GetPacientForEmailAndPasswd(string email, string passwd)
        {
            PacientModel pacient = new PacientModel();
            var p = _context.Pacients.FirstOrDefault(p => p.email == email && p.parola == passwd);

            if (p == null)
                return null;


            pacient.IDPacient = p.id_pacient;
            pacient.Nume = p.nume;
            pacient.Prenume = p.prenume;
            pacient.Judet = p.judet;
            pacient.Adresa = p.adresa;
            pacient.Email = p.email;
            pacient.Parola = p.parola;
            pacient.Activ = p.activ;
            pacient.CNP = p.CNP;

            return pacient;
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
            pacient.Email = p.email;
            pacient.Parola = p.parola;
            pacient.Activ = p.activ;
            pacient.CNP = p.CNP;

            return pacient;
        }

    }
}
