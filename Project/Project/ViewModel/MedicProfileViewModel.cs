using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    internal class MedicProfileViewModel
    {
        private MediciModel _medic;
        public MedicProfileViewModel(MediciModel medic)
        {
            _medic = medic;
        }

        public string Nume
        {
            get => _medic.Nume;
        }
        public string Prenume
        {
            get => _medic.Prenume;
        }
        public string Rating
        {
            get => _medic.Rating.ToString();
        }
        public string Specialitate
        {
            get => _medic.Sectie;
        }
        public string Titulatura
        {
            get => _medic.Titulatura;
        }
        public string Functie
        {
            get => _medic.Titulatura;
        }
        public string DataIncadrare
        {
            get => _medic.DataIncadrare;
        }
        public string Program
        {
            get {
                int index = _medic.Program.IndexOf(':') + 2;
                return _medic.Program.Substring(index);
            }
        }

        public string Departament
        {
            get => _medic.getMedicDepartament(_medic.IdAngajat);
        }
        public string Clinica
        { 
            get =>_medic.getMedicClinic(_medic.IdAngajat);
        }
    }
}
