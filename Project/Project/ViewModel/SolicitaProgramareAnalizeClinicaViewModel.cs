using Project.Commands;
using Project.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Project.ViewModel
{
    internal class SolicitaProgramareAnalizeClinicaViewModel : BaseViewModel
    {
        private readonly CliniciEntities _context;

        private ObservableCollection<string> _clinici;
        private string _clinicaSelectata;
        public string NumeFormular { get; }

        public SolicitaProgramareAnalizeClinicaViewModel(string numeFormular)
        {
            NumeFormular = numeFormular;
            _context = new CliniciEntities();
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
                var cliniciNume = _context.Clinicas
                                    .Where(c => _context.Functies
                                    .Any(f => f.id_clinica == c.id_clinica && f.nume_functie == "Sef Lab"))
                                    .Select(c => c.nume_clinica)
                                    .ToList();
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
            string numeF = NumeFormular;
            string numeClinica = ClinicaSelectata;
            var solicitaProgramareAnalizePacientViewModel = new SolicitaProgramareAnalizePacientViewModel(numeF, numeClinica);

            var solicitaProgramareAnalizePacientWindow = new SolicitaProgramareAnalizePacientWindow
            {
                DataContext = solicitaProgramareAnalizePacientViewModel
            };

            solicitaProgramareAnalizePacientWindow.Show();

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
                if (window is SolicitaProgramareAnalizeClinicaWindow)
                {
                    (window as Window)?.Close();
                    break;
                }
            }
        }
    }
}
