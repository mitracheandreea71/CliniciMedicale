using Project.Commands;
using Project.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class SolicitaProgramareClinicaViewModel : BaseViewModel
    {
        private readonly CliniciDataContext _context;

        private ObservableCollection<string> _clinici;
        private string _clinicaSelectata;

        public SolicitaProgramareClinicaViewModel()
        {
            _context = new CliniciDataContext();
            Clinici = new ObservableCollection<string>();
            LoadClinici();
            MaiDeparteCommand = new BaseCommand(ExecuteMaiDeparte, CanExecuteMaiDeparte);
        }

        public ObservableCollection<string> Clinici
        {
            get => _clinici;
            set
            {
                _clinici = value;
                OnPropertyChanged(nameof(Clinici));
            }
        }

        public string ClinicaSelectata
        {
            get => _clinicaSelectata;
            set
            {
                _clinicaSelectata = value;
                OnPropertyChanged(nameof(ClinicaSelectata));
            }
        }

        public ICommand MaiDeparteCommand { get; }

        private void LoadClinici()
        {
            try
            {
                var cliniciNume = _context.Clinicas.Select(c => c.nume_clinica).ToList();
                Clinici = new ObservableCollection<string>(cliniciNume);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea clinicilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteMaiDeparte(object parameter)
        {
            if (string.IsNullOrEmpty(ClinicaSelectata))
            {
                MessageBox.Show("Vă rugăm să selectați o clinică.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var solicitaProgramareDepartamentSiMedicWindow = new SolicitaProgramareDepartamentSiMedicWindow(ClinicaSelectata);
            solicitaProgramareDepartamentSiMedicWindow.Show();

            CloseCurrentWindow();
        }

        private bool CanExecuteMaiDeparte(object parameter)
        {
            return !string.IsNullOrEmpty(ClinicaSelectata);
        }

        private void CloseCurrentWindow()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is SolicitaProgramareClinicaWindow)
                {
                    (window as Window)?.Close();
                    break;
                }
            }
        }
    }
}
