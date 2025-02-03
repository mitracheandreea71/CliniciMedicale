using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    internal class VizualizareDiagnosticViewModel:BaseViewModel
    {
        private ConsultatieModel _consultatie;
        private MediciModel _medic;
        private PacientModel _pacient;
        private ClinicaModel _clinica;
        private DiagnosticModel _diagnostic;
        public VizualizareDiagnosticViewModel(ConsultatieModel consultatie, MediciModel medic)
        {
            _consultatie = consultatie;
            _medic = medic;
            _pacient = (new PacientModel()).GetPacientByID((int)_consultatie.IdPacient);
            _clinica = (new ClinicaModel()).GetClinicaById((int)_medic.IdClinica);
            _diagnostic = (new DiagnosticModel()).GetDiagnosticForConsultatie((int)_consultatie.IdConsultatie);
            if (_diagnostic == null)
            {
                _diagnostic = new DiagnosticModel();
                _diagnostic.IdConsultatie = _consultatie.IdConsultatie;
            }
        }

        public string NumePacient
        {
            get => _pacient.Nume;
            set => OnPropertyChanged(nameof(NumePacient));
        }
        public string PrenumePacient
        {
            get => _pacient.Prenume;
            set => OnPropertyChanged(nameof(PrenumePacient));
        }
        public string JudetPacient
        {
            get => _pacient.Judet;
            set => OnPropertyChanged(nameof(JudetPacient));
        }
        public string AdresaPacient
        {
            get => _pacient.Adresa;
            set => OnPropertyChanged(nameof(AdresaPacient));
        }
        public string NumeClinica
        {
            get => _clinica.NumeClinica;
            set => OnPropertyChanged(nameof(NumeClinica));
        }
        public string JudetClinica
        {
            get => _clinica.Judet;
            set => OnPropertyChanged(nameof(JudetClinica));
        }
        public string LocalitateClinica
        {
            get => _clinica.Oras;
            set => OnPropertyChanged(nameof(LocalitateClinica));
        }
        public string Tratament
        {
            get => _diagnostic.Tratament;
            set
            {
                _diagnostic.Tratament = value;
                _diagnostic.SaveDiagnostic();
                OnPropertyChanged(nameof(Tratament));
            }
        }
        public string Observatii
        {
            get => _diagnostic.Observatii;
            set
            {
                _diagnostic.Observatii = value;
                _diagnostic.SaveDiagnostic();
                OnPropertyChanged(nameof(Observatii));
            }
        }
        public string Diagnostic
        {
            get => _diagnostic.Diagnostic;
            set
            {
                _diagnostic.Diagnostic = value;
                _diagnostic.SaveDiagnostic();
                OnPropertyChanged(nameof(Diagnostic));
            }
        }
    }
}
