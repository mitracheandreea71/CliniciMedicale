using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class PacientModel
    {
        public int IDPacient {  get; set; }
        public string Nume {  get; set; }
        public string Prenume { get; set; } 
        public string Judet { get; set; }
        public string Adresa {  get; set; }

        public string Activ { set; get; }

        CliniciEntities _context;

        public PacientModel()
        { 
            _context = new CliniciEntities();
        }

        public bool DoesPatientExist(int idPacient)
        {
            return _context.Pacients.Any(p => p.id_pacient == idPacient);
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

        public int GetOrCreatePacient(string cnp, string nume, string prenume, string judet, string adresa)
        {
            var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient.ToString() == cnp);

            if (pacient == null)
            {
                pacient = new Pacient
                {
                    id_pacient = int.Parse(cnp),
                    nume = nume,
                    prenume = prenume,
                    judet = judet,
                    adresa = adresa
                };

                _context.Pacients.Add(pacient);
                _context.SaveChanges();
            }

            return pacient.id_pacient;
        }

        public bool DeletePatient(string cnp)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cnp))
                {
                    return false;
                }

                var patientToDelete = _context.Pacients.FirstOrDefault(p => p.id_pacient == int.Parse(cnp));
                if (patientToDelete == null)
                {
                    return false;
                }

                var asigurariToDelete = _context.Asigurares.Where(a => a.id_pacient == patientToDelete.id_pacient).ToList();
                _context.Asigurares.RemoveRange(asigurariToDelete);

                var buletineToDelete = _context.Buletin_Analize.Where(b => b.id_pacient == patientToDelete.id_pacient).ToList();
                foreach (var buletin in buletineToDelete)
                {
                    var rezultateToDelete = _context.Rezultat_Analize.Where(r => r.id_buletin_analize == buletin.id_buletin).ToList();
                    _context.Rezultat_Analize.RemoveRange(rezultateToDelete);
                }
                _context.Buletin_Analize.RemoveRange(buletineToDelete);

                var consultatiiToDelete = _context.Consultaties.Where(c => c.id_pacient == patientToDelete.id_pacient).ToList();
                foreach (var consultatie in consultatiiToDelete)
                {
                    var diagnosticeToDelete = _context.Diagnostics.Where(d => d.id_consultatie == consultatie.id_consultatie).ToList();
                    _context.Diagnostics.RemoveRange(diagnosticeToDelete);
                }
                _context.Consultaties.RemoveRange(consultatiiToDelete);

                var facturiToDelete = _context.Facturas.Where(f => f.id_pacient == patientToDelete.id_pacient).ToList();
                foreach (var factura in facturiToDelete)
                {
                    var consultatiiFacturiToDelete = _context.Consultatii_Factura.Where(cf => cf.id_factura == factura.id_factura).ToList();
                    _context.Consultatii_Factura.RemoveRange(consultatiiFacturiToDelete);

                    var analizeFacturiToDelete = _context.Analize_Factura.Where(af => af.id_factura == factura.id_factura).ToList();
                    _context.Analize_Factura.RemoveRange(analizeFacturiToDelete);
                }
                _context.Facturas.RemoveRange(facturiToDelete);

                _context.Pacients.Remove(patientToDelete);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
