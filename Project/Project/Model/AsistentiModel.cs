﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class AsistentiModel
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
        public string NumeComplet => $"{Nume} {Prenume}";
        public string Program { get; set; }
        public string Functie { get; set; }
        public string DataIncadrare { get; set; }
        public string Departament { get; set; }
        public int? IdClinica { get; set; }

        public string Activ { set; get; }

        private readonly CliniciEntities _context;
        public AsistentiModel()
        {
            _context = new CliniciEntities();
        }
        public int? GetAsistentIdByFullName(string fullName)
        {
            if (fullName == null)
                return null;
            string nume = fullName.Split(' ')[0];
            string prenume = fullName.Split(' ')[1];
            var asistent = _context.Angajats.Where(a => a.nume == nume && a.prenume == prenume).ToList().FirstOrDefault();
            if (asistent==null)
            {
                return null;
            }
            return asistent.id_angajat;
        }

        public AsistentiModel GetAsistentById(int idAsistent)
        {
            var asistent = _context.Angajats
                .FirstOrDefault(a => a.id_angajat == idAsistent);

            if (asistent != null)
            {
                var programTure = _context.Incadrare_Departament
                    .Where(i => i.id_angajat == asistent.id_angajat)
                    .Select(i => new
                    {
                        Intrare = i.intrare_tura,
                        Iesire = i.iesire_tura
                    })
                    .ToList();

                string ProgramAsistent = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));

                var FunctieAsistent = _context.Functies
                    .FirstOrDefault(f => f.id_angajat == asistent.id_angajat);

                return new AsistentiModel
                {
                    IdAngajat = asistent.id_angajat,
                    Titulatura = asistent.titulatura,
                    Nume = asistent.nume,
                    Prenume = asistent.prenume,
                    Username = asistent.username,
                    Parola = asistent.parola,
                    Email = asistent.email,
                    Telefon = asistent.telefon,
                    Sectie = asistent.specialitate,
                    Program = ProgramAsistent,
                    Functie = FunctieAsistent?.nume_functie,
                    DataIncadrare = FunctieAsistent?.data_incadrare.ToString(),
                    IdClinica = FunctieAsistent?.id_clinica
                };
            }

            return null;
        }

        public ObservableCollection<AsistentiModel> GetAllAsistentiPentruDepartament(string departament)
        {
            var asistenti = from angajat in _context.Angajats
                         join functie in _context.Functies on angajat.id_angajat equals functie.id_angajat
                         join incadrare in _context.Incadrare_Departament on angajat.id_angajat equals incadrare.id_angajat
                         join specialitate in _context.Departaments on incadrare.id_departament equals specialitate.id_departament
                         where functie.nume_functie == "Asistent" && specialitate.denumire == departament
                         select angajat;

            ObservableCollection<AsistentiModel> asistRet = new ObservableCollection<AsistentiModel>();
            foreach (var medic in asistenti)
            {
                var programTure = _context.Incadrare_Departament.Where(i => i.id_angajat == medic.id_angajat)
                                                                 .Select(i => new
                                                                 {
                                                                     Intrare = i.intrare_tura,
                                                                     Iesire = i.iesire_tura
                                                                 }).ToList();
                string ProgramAsist = string.Join(", ", programTure.Select(t => $"Program: {t.Intrare} - {t.Iesire}"));
                var FunctieAsist = _context.Functies.Where(f => f.id_angajat == medic.id_angajat).ToList().First();

                asistRet.Add(
                    new AsistentiModel
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
                        Program = ProgramAsist,
                        Functie = FunctieAsist.nume_functie,
                        DataIncadrare = FunctieAsist.data_incadrare.ToString(),
                        Departament = departament,
                        IdClinica = FunctieAsist.id_clinica
                    }
                    );
            }

            return asistRet;
        }
    }
}
