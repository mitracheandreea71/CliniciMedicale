using Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Project.Model;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace Project.ViewModel
{
    internal class SolicitaProgramareViewModel : BaseViewModel
    {
        private int _medicID;
        private string _numeMedic;
        private DateTime? _dataProgramare;

        private string _cnp;
        private string _nume;
        private string _prenume;
        private string _judet;
        private string _adresa;

        private ObservableCollection<string> _availableHours;
        private string _selectedHour;

        private readonly CliniciEntities _context;
        public SolicitaProgramareViewModel(int medicID, string numeMedic)
        {
            _medicID = medicID;
            _numeMedic = numeMedic;
            AvailableHours = new ObservableCollection<string>();
            _context = new CliniciEntities();
            TrimiteProgramareCommand = new BaseCommand(ExecuteTrimiteProgramare, CanExecuteTrimiteProgramare);
            LoadAvailableHours();

            
        }

        public int MedicID
        {
            get => _medicID;
            set
            {
                _medicID = value;
                OnPropertyChanged(nameof(MedicID));
            }
        }

        public string NumeMedic
        {
            get => _numeMedic;
            set
            {
                _numeMedic = value;
                OnPropertyChanged(nameof(NumeMedic));
            }
        }

        public DateTime? DataProgramare
        {
            get => _dataProgramare;
            set
            {
                _dataProgramare = value;
                OnPropertyChanged(nameof(DataProgramare));

                LoadAvailableHours();

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

        public string SelectedHour
        {
            get => _selectedHour;
            set
            {
                _selectedHour = value;
                OnPropertyChanged(nameof(SelectedHour));
            }
        }
        public ObservableCollection<string> AvailableHours
        {
            get => _availableHours;
            set
            {
                _availableHours = value;
                OnPropertyChanged(nameof(AvailableHours));
            }
        }
        public ICommand TrimiteProgramareCommand { get; }
        private void LoadAvailableHours()
        {
            if (!DataProgramare.HasValue)
            {
                AvailableHours.Clear();
                return;
            }
            try
            {
                var incadrareModel = new IncadrareDepartamentModel();
                var intervalOrar = incadrareModel.GetByIdMedic(_medicID);

                if (intervalOrar == null)
                {
                    MessageBox.Show("Intervalul orar al angajatului nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    AvailableHours.Clear();
                    return;
                }

                string intrareTuraString = NormalizeTimeFormat(intervalOrar.intrare_tura);
                if (!TimeSpan.TryParse(intrareTuraString, out TimeSpan intrareTura))
                {
                    MessageBox.Show($"Format invalid pentru 'intrare_tura': {intervalOrar.intrare_tura}.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    AvailableHours.Clear();
                    return;
                }

                string iesireTuraString = NormalizeTimeFormat(intervalOrar.iesire_tura);
                if (!TimeSpan.TryParse(iesireTuraString, out TimeSpan iesireTura))
                {
                    MessageBox.Show($"Format invalid pentru 'iesire_tura': {intervalOrar.iesire_tura}.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    AvailableHours.Clear();
                    return;
                }

                var oreProgramate = _context.Consultaties
                                    .Where(c => c.id_doctor == _medicID &&
                                                c.data_consultatie.HasValue &&
                                                c.data_consultatie.Value.Year == DataProgramare.Value.Year &&
                                                c.data_consultatie.Value.Month == DataProgramare.Value.Month &&
                                                c.data_consultatie.Value.Day == DataProgramare.Value.Day)
                                    .Select(c => c.ora)
                                    .ToList();

                var oreDisponibile = CalculateAvailableHours(intrareTura, iesireTura, oreProgramate);

                AvailableHours.Clear();
                foreach (var ora in oreDisponibile)
                {
                    AvailableHours.Add(ora);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la actualizarea orelor disponibile: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                AvailableHours.Clear();
            }
        }     
        private void ExecuteTrimiteProgramare(object parameter)
        {
            if (string.IsNullOrWhiteSpace(CNP) ||
                string.IsNullOrWhiteSpace(Nume) ||
                string.IsNullOrWhiteSpace(Prenume) ||
                string.IsNullOrWhiteSpace(Judet) ||
                string.IsNullOrWhiteSpace(Adresa) ||
                !DataProgramare.HasValue ||
                string.IsNullOrWhiteSpace(SelectedHour))
            {
                MessageBox.Show("Te rugăm să completezi toate câmpurile.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var pacientExistent = _context.Pacients.FirstOrDefault(p => p.id_pacient.ToString() == CNP);
                int pacientID;
                if (pacientExistent == null)
                {
                    var pacientNou = new Pacient
                    {
                        id_pacient = int.Parse(CNP), 
                        nume = Nume,
                        prenume = Prenume,
                        judet = Judet,
                        adresa = Adresa
                    };

                    _context.Pacients.Add(pacientNou);
                    _context.SaveChanges(); 

                    pacientID = pacientNou.id_pacient; 
                }
                else
                {
                    pacientID = pacientExistent.id_pacient; 
                }

                var consultatieNoua = new Consultatie
                {
                    id_doctor = this.MedicID,
                    id_pacient = pacientID,
                    data_consultatie = this.DataProgramare,
                    ora = this.SelectedHour
                };

                _context.Consultaties.Add(consultatieNoua);
                _context.SaveChanges(); 

                string message = $"Programare trimisă pentru {NumeMedic}.\n\n" +
                                 $"Pacient: {Nume} {Prenume}\n" +
                                 $"CNP: {CNP}\n" +
                                 $"Judet: {Judet}\n" +
                                 $"Adresa: {Adresa}\n" +
                                 $"Data Programare: {DataProgramare.Value.ToShortDateString()}\n" +
                                 $"Ora: {SelectedHour}\n";

                MessageBox.Show(message, "Confirmare Programare", MessageBoxButton.OK, MessageBoxImage.Information);

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
        private List<string> CalculateAvailableHours(TimeSpan intrare, TimeSpan iesire, List<string> oreProgramate)
        {
            List<string> oreDisponibile = new List<string>();

            for (int hour = intrare.Hours; hour < iesire.Hours; hour++)
            {
                string ora = $"{hour}:00";
                if (!oreProgramate.Contains(ora))
                {
                    oreDisponibile.Add(ora);
                }
            }

            return oreDisponibile;
        }
        private bool CanExecuteTrimiteProgramare(object parameter)
        {
            return true;
        }
        private string NormalizeTimeFormat(string time)
        {
            if (string.IsNullOrWhiteSpace(time))
                return null;
            var parts = time.Split(':');

            if (parts.Length == 2)
            {
                string hourPart = parts[0].Length == 1 ? "0" + parts[0] : parts[0];
                return $"{hourPart}:{parts[1]}"; 
            }

            return time; 
        }

    }
}
