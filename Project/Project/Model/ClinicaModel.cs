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

        public string Activ { get; set; }

        public List<string> ListaSpecializari { get; set; }

        private readonly CliniciEntities _context;

        public ClinicaModel()
        { 
            _context = new CliniciEntities();
        }
        public ClinicaModel GetClinicaByMedicId(int medicId)
        { 
            var functie = _context.Functies.Where(f => f.id_angajat == medicId).FirstOrDefault();
            if (functie == null)
                return null;
            var clinica = _context.Clinicas.Where(c => c.id_clinica == functie.id_clinica).FirstOrDefault();
            if (clinica == null)
                return null;
            ClinicaModel clinicaRet = new ClinicaModel
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
            };
            return clinicaRet;
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

        public List<string> GetDepartmentsByClinic(string clinicName)
        {
            var clinicId = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == clinicName)?.id_clinica;
            if (clinicId == null) return new List<string>();

            return _context.Departaments
                .Where(d => d.id_clinica == clinicId)
                .Select(d => d.denumire)
                .ToList();
        }

        public bool AddDepartment(string clinicName, string departmentName)
        {
            try
            {
                using (var _context = new CliniciEntities())
                {
                    var clinic = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == clinicName);
                    if (clinic == null) return false;

                    if (_context.Departaments.Any(d => d.denumire == departmentName && d.id_clinica == clinic.id_clinica))
                    {
                        return false;
                    }

                    var newDepartment = new Departament
                    {
                        denumire = departmentName,
                        id_clinica = clinic.id_clinica
                    };

                    _context.Departaments.Add(newDepartment);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteClinic(string clinicName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(clinicName))
                {
                    return false;
                }

                var clinicToDeactivate = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == clinicName);
                if (clinicToDeactivate == null)
                {
                    return false;
                }

                clinicToDeactivate.activ = "OFF";

                var departmentsToDeactivate = _context.Departaments
                    .Where(d => d.id_clinica == clinicToDeactivate.id_clinica)
                    .ToList();

                foreach (var department in departmentsToDeactivate)
                {
                    department.activ = "OFF";

                    var employeeAssignments = _context.Incadrare_Departament
                        .Where(i => i.id_departament == department.id_departament)
                        .ToList();

                    foreach (var assignment in employeeAssignments)
                    {
                        var employeeFunction = _context.Functies.FirstOrDefault(f => f.id_angajat == assignment.id_angajat);
                        if (employeeFunction != null)
                        {
                            employeeFunction.activ = "OFF";
                        }

                        var employee = _context.Angajats.FirstOrDefault(a => a.id_angajat == assignment.id_angajat);
                        if (employee != null)
                        {
                            employee.activ = "OFF";
                        }

                        assignment.activ = "OFF";
                    }
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GetAllClinics()
        {
            return _context.Clinicas.Select(c => c.nume_clinica).ToList();
        }

        public ClinicaModel GetClinicaById(int idClinica)
        {
            ClinicaModel clinica = (new ClinicaModel()).GetAllClinici().Where(c => c.ClinicaID == idClinica).First();
            return clinica;
        }

        public bool DeleteDepartment(string clinicName, string departmentName)
        {
            var clinicId = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == clinicName)?.id_clinica;
            if (clinicId == null) return false;

            var department = _context.Departaments.FirstOrDefault(d => d.denumire == departmentName && d.id_clinica == clinicId);
            if (department == null) return false;

            _context.Departaments.Remove(department);
            _context.SaveChanges();
            return true;
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

        public List<string> GetDepartamenteByClinica(string numeClinica)
        {
            return _context.Departaments
                .Where(d => d.Clinica.nume_clinica == numeClinica)
                .Select(d => d.denumire)
                .ToList();
        }

        public int? GetClinicIdByName(string clinicName)
        {
            return _context.Clinicas
                .Where(c => c.nume_clinica == clinicName)
                .Select(c => (int?)c.id_clinica)
                .FirstOrDefault();
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

            _context.Clinicas.Add(newClinic);
            _context.SaveChanges();
        }
    }
}
