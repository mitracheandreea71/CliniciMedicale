using Project.Commands;
using Project.Model;
using Project.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class SolicitaProgramareDepartamentSiMedicViewModel : BaseViewModel
    {
        private string _numeClinica;
        private ObservableCollection<string> _departamente;
        private string _departamentSelectat;
        private ObservableCollection<string> _medici;
        private string _medicSelectat;
        private readonly ClinicaModel _clinicaModel;
        private readonly MediciModel _mediciModel;

        public ICommand MaiDeparteCommand { get; }

        public SolicitaProgramareDepartamentSiMedicViewModel(string numeClinica)
        {
            _numeClinica = numeClinica;
            _clinicaModel = new ClinicaModel();
            _mediciModel = new MediciModel();

            Departamente = new ObservableCollection<string>();
            Medici = new ObservableCollection<string>();

            LoadDepartamente();
            MaiDeparteCommand = new BaseCommand(ExecuteMaiDeparte, CanExecuteMaiDeparte);
        }

        public string NumeClinica
        {
            get => _numeClinica;
            set
            {
                _numeClinica = value;
                OnPropertyChanged(nameof(NumeClinica));
            }
        }

        public ObservableCollection<string> Departamente
        {
            get => _departamente;
            set
            {
                _departamente = value;
                OnPropertyChanged(nameof(Departamente));
            }
        }

        public string DepartamentSelectat
        {
            get => _departamentSelectat;
            set
            {
                _departamentSelectat = value;
                OnPropertyChanged(nameof(DepartamentSelectat));
                LoadMedici();
            }
        }

        public ObservableCollection<string> Medici
        {
            get => _medici;
            set
            {
                _medici = value;
                OnPropertyChanged(nameof(Medici));
            }
        }

        public string MedicSelectat
        {
            get => _medicSelectat;
            set
            {
                _medicSelectat = value;
                OnPropertyChanged(nameof(MedicSelectat));
            }
        }

        private void LoadDepartamente()
        {
            var departamente = _clinicaModel.GetDepartamenteByClinica(_numeClinica);

            Departamente.Clear();
            foreach (var dep in departamente)
            {
                Departamente.Add(dep);
            }
        }

        private void LoadMedici()
        {
            if (string.IsNullOrEmpty(DepartamentSelectat))
                return;

            var medici = _mediciModel.GetMediciByDepartamentAndClinica(DepartamentSelectat, _numeClinica);

            Medici.Clear();
            foreach (var medic in medici)
            {
                Medici.Add(medic);
            }
        }

        private void ExecuteMaiDeparte(object parameter)
        {
            if (string.IsNullOrEmpty(MedicSelectat))
            {
                MessageBox.Show("Vă rugăm să selectați un medic.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string medicSelectat = parameter as string;
            int? idMedic = _mediciModel.GetMedicIdByNumeComplet(medicSelectat);

            if (idMedic.HasValue)
            {
                var solicitaProgramareWindow = new SolicitaProgramareWindow(idMedic.Value, medicSelectat);
                solicitaProgramareWindow.Show();
                CloseCurrentWindow();
            }
            else
            {
                MessageBox.Show("Medicul selectat nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseCurrentWindow()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is SolicitaProgramareDepartamentSiMedicWindow)
                {
                    (window as Window)?.Close();
                    break;
                }
            }
        }

        private bool CanExecuteMaiDeparte(object parameter)
        {
            return !string.IsNullOrEmpty(MedicSelectat);
        }
    }
}
