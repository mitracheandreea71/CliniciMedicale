using System;
using System.Windows;
using System.Windows.Input;
using Project.Commands;
using Project.Model;

namespace Project.ViewModel
{
    internal class DeletePatientViewModel : BaseViewModel
    {
        private readonly PacientModel _pacientModel;

        public string Cnp { get; set; }
        public ICommand DeletePatientCommand { get; }

        public DeletePatientViewModel()
        {
            _pacientModel = new PacientModel();
            DeletePatientCommand = new BaseCommand(DeletePatient);
        }

        private void DeletePatient(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Cnp))
            {
                MessageBox.Show("Introduceți CNP-ul pacientului!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool success = _pacientModel.DeletePatient(Cnp);

            if (success)
            {
                MessageBox.Show("Pacientul și toate datele asociate au fost șterse cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Pacientul nu a fost găsit sau a apărut o eroare la ștergere.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
