using Project.Commands;
using Project.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class SolicitaProgramareAnalizePacientViewModel : BaseViewModel
    {
        public string NumeFormular { get; }
        public string NumeClinica { get; }

        private DateTime? _dataProgramare;
        private string _cnp;
        private string _nume;
        private string _prenume;
        private string _judet;
        private string _adresa;

        private readonly ClinicaModel _clinicaModel;
        private readonly FormularAnalizeModel _formularModel;
        private readonly PacientModel _pacientModel;
        private readonly BuletinAnalizeModel _buletinAnalizeModel;

        public ICommand TrimiteProgramareCommand { get; }

        public SolicitaProgramareAnalizePacientViewModel(string numeFormular, string numeClinica)
        {
            NumeFormular = numeFormular;
            NumeClinica = numeClinica;
            _clinicaModel = new ClinicaModel();
            _formularModel = new FormularAnalizeModel();
            _pacientModel = new PacientModel();
            _buletinAnalizeModel = new BuletinAnalizeModel();

            TrimiteProgramareCommand = new BaseCommand(ExecuteTrimiteProgramare, CanExecuteTrimiteProgramare);
        }

        public DateTime? DataProgramare
        {
            get => _dataProgramare;
            set
            {
                _dataProgramare = value;
                OnPropertyChanged(nameof(DataProgramare));
            }
        }

        public string CNP
        {
            get => _cnp;
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(CNP));
            }
        }

        public string Nume
        {
            get => _nume;
            set
            {
                _nume = value;
                OnPropertyChanged(nameof(Nume));
            }
        }

        public string Prenume
        {
            get => _prenume;
            set
            {
                _prenume = value;
                OnPropertyChanged(nameof(Prenume));
            }
        }

        public string Judet
        {
            get => _judet;
            set
            {
                _judet = value;
                OnPropertyChanged(nameof(Judet));
            }
        }

        public string Adresa
        {
            get => _adresa;
            set
            {
                _adresa = value;
                OnPropertyChanged(nameof(Adresa));
            }
        }

        private void ExecuteTrimiteProgramare(object parameter)
        {
            if (string.IsNullOrWhiteSpace(CNP) ||
                string.IsNullOrWhiteSpace(Nume) ||
                string.IsNullOrWhiteSpace(Prenume) ||
                string.IsNullOrWhiteSpace(Judet) ||
                string.IsNullOrWhiteSpace(Adresa) ||
                !DataProgramare.HasValue)
            {
                MessageBox.Show("Te rugăm să completezi toate câmpurile.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                int pacientID = _pacientModel.GetOrCreatePacient(CNP, Nume, Prenume, Judet, Adresa);

                int idFormular = _formularModel.GetIdByName(NumeFormular);
                int idClinica = _clinicaModel.GetIdByName(NumeClinica);

                int? idSefLab = _buletinAnalizeModel.GetSefLabByClinicaId(idClinica);
                if (!idSefLab.HasValue)
                {
                    MessageBox.Show("Nu s-a găsit un șef de laborator pentru această clinică.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                TimeSpan hour = new TimeSpan(7, 0, 0);
                DateTime dataRecoltare = DataProgramare.Value.Date + hour;

                _buletinAnalizeModel.AddBuletinAnalize(idFormular, pacientID, dataRecoltare, idSefLab.Value);

                MessageBox.Show("Programarea a fost trimisă cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

                if (parameter is Window window)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la trimiterea programării: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteTrimiteProgramare(object parameter)
        {
            return true;
        }
    }
}
