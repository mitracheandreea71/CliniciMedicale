using System;
using System.Windows;
using System.Windows.Input;
using Project.Commands;
using Project.Model;

namespace Project.ViewModel
{
    internal class DeleteClinicViewModel : BaseViewModel
    {
        private readonly ClinicaModel _clinicaModel;

        public string ClinicName { get; set; }
        public ICommand DeleteClinicCommand { get; }

        public DeleteClinicViewModel()
        {
            _clinicaModel = new ClinicaModel();
            DeleteClinicCommand = new BaseCommand(DeleteClinic);
        }

        private void DeleteClinic(object parameter)
        {
            if (string.IsNullOrWhiteSpace(ClinicName))
            {
                MessageBox.Show("Introduceți numele clinicii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool success = _clinicaModel.DeleteClinic(ClinicName);

            if (success)
            {
                MessageBox.Show("Clinica și toate datele asociate au fost șterse cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Clinica nu a fost găsită sau a apărut o eroare la ștergere.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
