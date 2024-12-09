using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Project.Model
{
    public class MediciModel : INotifyPropertyChanged
    {
        public int IdAngajat { get; set; }
        public string Titulatura { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Username { get; set; }
        public string Parola { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Sectie { get; set; }
        public double Rating { get; set; }
        public string CaleImagine { get; set; }
        public string NumeComplet => $" {Titulatura} {Nume} {Prenume}";
        public string Program { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CliniciDataContext _context;

        public MediciModel()
        {
            _context = new CliniciDataContext();
        }

        public ObservableCollection<MediciModel> GetAllMedici()
        {
            ObservableCollection<MediciModel> medici = new ObservableCollection<MediciModel>();

            var functiiMedici = _context.Functies.Where(f => f.nume_functie == "Medic")
                                                 .OrderByDescending(f => f.Angajat.rating);

            foreach (var functie in functiiMedici)
            {
                var programTure = _context.Incadrare_Departaments.Where(i => i.id_angajat == functie.Angajat.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                medici.Add(
                    new MediciModel
                    {
                        IdAngajat = functie.Angajat.id_angajat,
                        Titulatura = functie.Angajat.titulatura,
                        Nume = functie.Angajat.nume,
                        Prenume = functie.Angajat.prenume,
                        Username = functie.Angajat.username,
                        Parola  = functie.Angajat.parola,
                        Email = functie.Angajat.email,
                        Telefon = functie.Angajat.telefon,
                        Sectie = functie.Angajat.specialitate,
                        Rating = (double)functie.Angajat.rating,
                        CaleImagine = functie.Angajat.imagine_cale,
                        Program = ProgramMedic
                    }
                    );
            }
            
            return medici;
        }

        public ObservableCollection<MediciModel> GetAllMediciPentruDepartament(string departament)
        {
            var medici = from angajat in _context.Angajats
                         join functie in _context.Functies on angajat.id_angajat equals functie.id_angajat
                         join incadrare in _context.Incadrare_Departaments on angajat.id_angajat equals incadrare.id_angajat
                         join specialitate in _context.Departaments on incadrare.id_departament equals specialitate.id_departament
                         where functie.nume_functie == "Medic" && specialitate.denumire == departament
                         select angajat;

            ObservableCollection<MediciModel> mediciRet = new ObservableCollection<MediciModel>();
           foreach (var medic in medici)
            {
                var programTure = _context.Incadrare_Departaments.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                mediciRet.Add(
                    new MediciModel
                    {
                        IdAngajat = medic.id_angajat,
                        Titulatura = medic.titulatura,
                        Nume = medic.nume,
                        Prenume = medic.prenume,
                        Username = medic.username,
                        Parola = medic.parola,
                        Email = medic.email,
                        Telefon = medic.telefon,
                        Sectie = medic.specialitate,
                        Rating = (double)medic.rating,
                        CaleImagine = medic.imagine_cale,
                        Program = ProgramMedic
                    }
                    );
            }

            return mediciRet;
        }

        public ObservableCollection<MediciModel> GetAllMediciPentruOras(string oras)
        {
            var medici = from angajat in _context.Angajats
                         join functie in _context.Functies on angajat.id_angajat equals functie.id_angajat
                         join clinica in _context.Clinicas on functie.id_clinica equals clinica.id_clinica
                         where clinica.oras == oras
                         select angajat;

            ObservableCollection<MediciModel> mediciRet = new ObservableCollection<MediciModel>();
            foreach (var medic in medici)
            {
                var programTure = _context.Incadrare_Departaments.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                mediciRet.Add(
                    new MediciModel
                    {
                        IdAngajat = medic.id_angajat,
                        Titulatura = medic.titulatura,
                        Nume = medic.nume,
                        Prenume = medic.prenume,
                        Username = medic.username,
                        Parola = medic.parola,
                        Email = medic.email,
                        Telefon = medic.telefon,
                        Sectie = medic.specialitate,
                        Rating = (double)medic.rating,
                        CaleImagine = medic.imagine_cale,
                        Program = ProgramMedic
                    }
                    );
            }

            return mediciRet;
        }

        public ObservableCollection<MediciModel> GetAllMediciPentruLocatie(string locatie)
        {
            var medici = from angajat in _context.Angajats
                         join functie in _context.Functies on angajat.id_angajat equals functie.id_angajat
                         join clinica in _context.Clinicas on functie.id_clinica equals clinica.id_clinica
                         where clinica.nume_clinica == locatie
                         select angajat;

            ObservableCollection<MediciModel> mediciRet = new ObservableCollection<MediciModel>();
            foreach (var medic in medici)
            {
                var programTure = _context.Incadrare_Departaments.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                mediciRet.Add(
                    new MediciModel
                    {
                        IdAngajat = medic.id_angajat,
                        Titulatura = medic.titulatura,
                        Nume = medic.nume,
                        Prenume = medic.prenume,
                        Username = medic.username,
                        Parola = medic.parola,
                        Email = medic.email,
                        Telefon = medic.telefon,
                        Sectie = medic.specialitate,
                        Rating = (double)medic.rating,
                        CaleImagine = medic.imagine_cale,
                        Program = ProgramMedic
                    }
                    );
            }

            return mediciRet;
        }
    }
}
