using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Project.Model
{
    public class ClinicaModel
    {
        public int ClinicaID { get; set; }
        public string NumeClinica { get; set; }
        public string Judet { get; set; }
        public string Oras { get; set; }
        public string Adresa { get; set; }
        public string Program { get; set; }
        public string NrContact { get; set; }
        public string Email { get; set; }
        public string CIF { get; set; }
        public string IBAN { get; set; }
        public string Banca { get; set; }
        public string CaleImagine { get; set; }

        public List<string> ListaSpecializari { get; set; }

        private readonly CliniciDataContext _context;

        public ClinicaModel()
        { 
            _context = new CliniciDataContext();
        }

        public ObservableCollection<ClinicaModel> GetAllClinici()
        {
            ObservableCollection<ClinicaModel> clinici = new ObservableCollection<ClinicaModel>();

            foreach (var clinica in _context.Clinicas)
            {
                clinici.Add(
                    new ClinicaModel
                    {
                        ClinicaID = clinica.id_clinica,
                        NumeClinica = clinica.nume_clinica,
                        Judet = clinica.judet,
                        Oras = clinica.oras,
                        Adresa = clinica.adresa,
                        Program = clinica.program,
                        NrContact = clinica.nr_contact,
                        Email = clinica.email,
                        CIF = clinica.CIF,
                        IBAN = clinica.IBAN,
                        Banca = clinica.Banca,
                        CaleImagine = clinica.cale_imagine
                    }
                    );
            }

            foreach (var clinica in clinici)
            {
                clinica.ListaSpecializari = (from departament in _context.Departaments
                                             where departament.id_clinica == clinica.ClinicaID
                                             select departament.denumire).ToList();
            }

            return clinici;
        }

        public ClinicaModel GetClinicaById(int idClinica)
        {
            ClinicaModel clinica = (new ClinicaModel()).GetAllClinici().Where(c => c.ClinicaID == idClinica).First();
            return clinica;
        }
        public ObservableCollection<string> GetAllOrase()
        {
            var orase = _context.Clinicas.Select(Clinica => Clinica.oras)
                                          .Distinct()
                                          .ToList();
            return new ObservableCollection<string>(orase);
        }

        public ObservableCollection<string> GetAllLocatii()
        {
            var locatii = _context.Clinicas.Select(Clinica => Clinica.nume_clinica).ToList();

            return new ObservableCollection<string>(locatii);
        }

        public ObservableCollection<string> GetAllDepartaments()
        { 
            var departamente = _context.Departaments.Select(Departament => Departament.denumire).ToList();

            return new ObservableCollection<string>(departamente);
        }

        public int GetIdByName(string name)
        {
            var clinica = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == name);
            if (clinica != null)
            {
                return clinica.id_clinica;
            }
            return -1;
        }

        public void AddClinic(ClinicaModel clinic)
        {
            var newClinic = new Clinica
            {
                nume_clinica = clinic.NumeClinica,
                judet = clinic.Judet,
                oras = clinic.Oras,
                adresa = clinic.Adresa,
                program = clinic.Program,
                nr_contact = clinic.NrContact,
                email = clinic.Email,
                CIF = clinic.CIF,
                IBAN = clinic.IBAN,
                Banca = clinic.Banca
            };

            _context.Clinicas.InsertOnSubmit(newClinic);
            _context.SubmitChanges();
        }
    }
}
