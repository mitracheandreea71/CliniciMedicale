﻿using System;
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
    public class MediciModel
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
        public double? Rating { get; set; }
        public string? CaleImagine { get; set; }
        public string NumeComplet => $" {Titulatura} {Nume} {Prenume}";
        public string Program { get; set; }
        public string Functie { get; set; }
        public string DataIncadrare { get; set; }
        public int? IdClinica { get; set; }

        public string Activ { get; set; }


        private readonly CliniciEntities _context;

        public MediciModel()
        {
            _context = new CliniciEntities();
        }

        public MediciModel GetMedicByCaleImagine(string caleImagine)
        {
            var medic = _context.Angajats
                .Where(a => a.imagine_cale == caleImagine)
                .FirstOrDefault();

            if (medic != null)
            {
                var programTure = _context.Incadrare_Departament
                    .Where(i => i.id_angajat == medic.id_angajat)
                    .Select(i => new
                    {
                        Intrare = i.intrare_tura,
                        Iesire = i.iesire_tura
                    })
                    .ToList();

                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));

                var FunctieMedic = _context.Functies.Where(f => f.id_angajat == medic.id_angajat).ToList().First();

                return new MediciModel
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
                    Program = ProgramMedic,
                    Functie = FunctieMedic.nume_functie,
                    DataIncadrare = FunctieMedic.data_incadrare.ToString(),
                    IdClinica = FunctieMedic.id_clinica
                };
            }

            return null;
        }
        public ObservableCollection<MediciModel> GetAllMedici()
        {
            ObservableCollection<MediciModel> medici = new ObservableCollection<MediciModel>();

            var functiiMedici = _context.Functies.Where(f => f.nume_functie == "Medic" || f.nume_functie=="Sef Lab")
                                                 .OrderByDescending(f => f.Angajat.rating);

            foreach (var functie in functiiMedici)
            {
                var programTure = _context.Incadrare_Departament.Where(i => i.id_angajat == functie.Angajat.id_angajat)
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
                        Program = ProgramMedic,
                        Functie = functie.nume_functie,
                        DataIncadrare = functie.data_incadrare.ToString(),
                        IdClinica = functie.id_clinica
                    }
                    );
            }
            
            return medici;
        }

        public bool DeleteEmployeesByDepartment(string departmentName)
        {
            var employees = _context.Angajats.Where(e => e.specialitate == departmentName).ToList();
            if (!employees.Any()) return false;

            foreach (var employee in employees)
            {
                var functions = _context.Functies.Where(f => f.id_angajat == employee.id_angajat).ToList();
                _context.Functies.RemoveRange(functions);

                var incadrari = _context.Incadrare_Departament.Where(i => i.id_angajat == employee.id_angajat).ToList();
                _context.Incadrare_Departament.RemoveRange(incadrari);

                _context.Angajats.Remove(employee);
            }

            _context.SaveChanges();
            return true;
        }
        public ObservableCollection<MediciModel> GetAllAngajati()
        {
            ObservableCollection<MediciModel> angajati = new ObservableCollection<MediciModel>();

            var functiiMedici = _context.Functies.ToList();

            foreach (var functie in functiiMedici)
            {
                var programTure = _context.Incadrare_Departament
                    .Where(i => i.id_angajat == functie.Angajat.id_angajat)
                    .Select(i => new
                    {
                        Intrare = i.intrare_tura,
                        Iesire = i.iesire_tura
                    })
                    .ToList();

                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));

                angajati.Add(new MediciModel
                {
                    IdAngajat = functie.Angajat.id_angajat,
                    Titulatura = functie.Angajat.titulatura ?? string.Empty,
                    Nume = functie.Angajat.nume ?? string.Empty,
                    Prenume = functie.Angajat.prenume ?? string.Empty,
                    Username = functie.Angajat.username ?? string.Empty,
                    Parola = functie.Angajat.parola ?? string.Empty,
                    Email = functie.Angajat.email ?? string.Empty,
                    Telefon = functie.Angajat.telefon ?? string.Empty,
                    Sectie = functie.Angajat.specialitate ?? string.Empty,
                    Rating = (double?)(functie.Angajat.rating ?? default(decimal)),
                    CaleImagine = functie.Angajat.imagine_cale ?? string.Empty,
                    Program = ProgramMedic,
                    Functie = functie.nume_functie ?? string.Empty,
                    DataIncadrare = functie.data_incadrare?.ToString() ?? string.Empty,
                    IdClinica = functie.id_clinica
                });
            }

            return angajati;
        }

        public bool AddEmployee(
            string titulatura, string nume, string prenume, string username, string parola,
            string email, string telefon, string functie, string sectie, int? idClinica,
            DateTime? dataIncadrare, DateTime? dataExpirare, string intrareTura, string iesireTura)
        {
            try
            {
                var newAngajat = new Angajat
                {
                    titulatura = titulatura,
                    nume = nume,
                    prenume = prenume,
                    username = username,
                    parola = parola,
                    email = email,
                    telefon = telefon,
                    specialitate = sectie,
                    imagine_cale = null,
                    rating = titulatura == "Admin" ? (decimal?)null : 0
                };

                _context.Angajats.Add(newAngajat);
                _context.SaveChanges();

                int newEmployeeId = newAngajat.id_angajat;

                _context.Functies.Add(new Functie
                {
                    id_angajat = newEmployeeId,
                    id_clinica = idClinica,
                    nume_functie = functie,
                    data_incadrare = dataIncadrare,
                    data_expirare_contract = dataExpirare
                });
                _context.SaveChanges();

                if (functie != "Admin" && functie != "Sef Lab" && idClinica.HasValue)
                {
                    var departament = _context.Departaments.FirstOrDefault(d => d.denumire == sectie);
                    if (departament != null)
                    {
                        _context.Incadrare_Departament.Add(new Incadrare_Departament
                        {
                            id_departament = departament.id_departament,
                            id_angajat = newEmployeeId,
                            intrare_tura = intrareTura,
                            iesire_tura = iesireTura
                        });
                        _context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ObservableCollection<MediciModel> GetAllMediciPentruDepartament(string departament)
        {
            var medici = from angajat in _context.Angajats
                         join functie in _context.Functies on angajat.id_angajat equals functie.id_angajat
                         join incadrare in _context.Incadrare_Departament on angajat.id_angajat equals incadrare.id_angajat
                         join specialitate in _context.Departaments on incadrare.id_departament equals specialitate.id_departament
                         where (functie.nume_functie == "Medic" || functie.nume_functie == "Sef Lab") && specialitate.denumire == departament
                         select angajat;

            ObservableCollection<MediciModel> mediciRet = new ObservableCollection<MediciModel>();
           foreach (var medic in medici)
            {
                var programTure = _context.Incadrare_Departament.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                var FunctieMedic = _context.Functies.Where(f => f.id_angajat == medic.id_angajat).ToList().First();

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
                        Program = ProgramMedic,
                        Functie = FunctieMedic.nume_functie,
                        DataIncadrare = FunctieMedic.data_incadrare.ToString(),
                        IdClinica = FunctieMedic.id_clinica
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
                         where clinica.oras == oras && (functie.nume_functie == "Medic" || functie.nume_functie == "Sef Lab")
                         select angajat;

            ObservableCollection<MediciModel> mediciRet = new ObservableCollection<MediciModel>();
            foreach (var medic in medici)
            {
                var programTure = _context.Incadrare_Departament.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                var FunctieMedic = _context.Functies.Where(f => f.id_angajat == medic.id_angajat).ToList().First();
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
                        Rating = medic.rating.HasValue ? (double)medic.rating.Value : 0.0,
                        CaleImagine = medic.imagine_cale,
                        Program = ProgramMedic,
                        Functie = FunctieMedic.nume_functie,
                        DataIncadrare = FunctieMedic.data_incadrare.ToString(),
                        IdClinica = FunctieMedic.id_clinica
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
                         where clinica.nume_clinica == locatie && (functie.nume_functie=="Medic" || functie.nume_functie=="Sef Lab")
                         select angajat;

            ObservableCollection<MediciModel> mediciRet = new ObservableCollection<MediciModel>();
            foreach (var medic in medici)
            {
                var programTure = _context.Incadrare_Departament.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                var FunctieMedic = _context.Functies.Where(f => f.id_angajat == medic.id_angajat).ToList().First();

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
                        Rating = medic.rating.HasValue ? (double)medic.rating.Value : 0.0,
                        CaleImagine = medic.imagine_cale,
                        Program = ProgramMedic,
                        Functie = FunctieMedic.nume_functie,
                        DataIncadrare = FunctieMedic.data_incadrare.ToString(),
                        IdClinica = FunctieMedic.id_clinica
                    }
                    );
            }

            return mediciRet;
        }

        public int? GetMedicIdByNumeComplet(string numeComplet)
        {
            if (string.IsNullOrEmpty(numeComplet))
                return null;

            var parts = numeComplet.Trim().Split(' ');

            if (parts.Length < 3)
                return null; 

            int indexNume = parts.Length - 2; 
            string titulatura = string.Join(" ", parts.Take(indexNume)); 

            string nume = parts[indexNume]; 
            string prenume = string.Join(" ", parts.Skip(indexNume + 1)); 

            var medic = _context.Angajats
                .FirstOrDefault(a => a.titulatura == titulatura &&
                                     a.nume == nume &&
                                     a.prenume == prenume);

            return medic?.id_angajat; 
        }


        public int? GetMedicIdByNumeCompletFaraTitulatura(string numeComplet)
        {
            if (string.IsNullOrEmpty(numeComplet))
                return null;

            var parts = numeComplet.Trim().Split(' ');


            int indexNume = parts.Length - 2;
            string nume = parts[indexNume];
            string prenume = string.Join(" ", parts.Skip(indexNume + 1));

            var medic = _context.Angajats
                .FirstOrDefault(a => a.nume == nume &&
                                     a.prenume == prenume);

            return medic?.id_angajat;
        }
        public string getMedicDepartament(int medicId)
        {
            var incadrare_departament = _context.Incadrare_Departament.Where(d => d.id_angajat == medicId).ToList().First();
            var department = _context.Departaments.Where(d => d.id_departament == incadrare_departament.id_departament).ToList().First();
            return department.denumire;
        }

        public string getMedicClinic(int medicId)
        {
            var incadrare_departament = _context.Incadrare_Departament.Where(d => d.id_angajat == medicId).ToList().First();
            var department = _context.Departaments.Where(d => d.id_departament == incadrare_departament.id_departament).ToList().First();
            return department.Clinica.nume_clinica;
        }

        public MediciModel GetMedicById(int idAngajat)
        {
            var medic = _context.Angajats
                .FirstOrDefault(a => a.id_angajat == idAngajat);

            if (medic != null)
            {
                var programTure = _context.Incadrare_Departament
                    .Where(i => i.id_angajat == medic.id_angajat)
                    .Select(i => new
                    {
                        Intrare = i.intrare_tura,
                        Iesire = i.iesire_tura
                    })
                    .ToList();

                string ProgramMedic = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));

                var FunctieMedic = _context.Functies
                    .FirstOrDefault(f => f.id_angajat == medic.id_angajat);

                return new MediciModel
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
                    Rating = medic.rating.HasValue ? (double)medic.rating.Value : 0.0,
                    CaleImagine = medic.imagine_cale,
                    Program = ProgramMedic,
                    Functie = FunctieMedic?.nume_functie,
                    DataIncadrare = FunctieMedic?.data_incadrare.ToString(),
                    IdClinica = FunctieMedic?.id_clinica
                };
            }

            return null;
        }

        public List<string> GetMediciByDepartamentAndClinica(string departament, string clinica)
        {
            int idClinica = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == clinica)?.id_clinica ?? 0;

            return (from angajat in _context.Angajats
                    join functie in _context.Functies on angajat.id_angajat equals functie.id_angajat
                    where angajat.specialitate == departament
                          && angajat.titulatura.Contains("Dr")
                          && functie.id_clinica == idClinica
                    select angajat.titulatura + " " + angajat.nume + " " + angajat.prenume)
                .ToList();
        }

        public bool DeleteEmployeeById(int idAngajat)
        {
            try
            {
                var functie = _context.Functies.FirstOrDefault(f => f.id_angajat == idAngajat);
                if (functie != null)
                {
                    _context.Functies.Remove(functie);
                }

                var incadrari = _context.Incadrare_Departament.Where(i => i.id_angajat == idAngajat).ToList();
                if (incadrari.Any())
                {
                    _context.Incadrare_Departament.RemoveRange(incadrari);
                }

                var angajat = _context.Angajats.FirstOrDefault(a => a.id_angajat == idAngajat);
                if (angajat == null)
                    return false;

                _context.Angajats.Remove(angajat);
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
