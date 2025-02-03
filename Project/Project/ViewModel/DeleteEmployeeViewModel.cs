using System;
using System.Windows;
using System.Windows.Input;
using Project.Commands;
using Project.Model;

namespace Project.ViewModel
{
    internal class DeleteEmployeeViewModel : BaseViewModel
    {
        private string _employeeName;
        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        public ICommand DeleteEmployeeCommand { get; }

        private readonly MediciModel _mediciModel;

        public DeleteEmployeeViewModel()
        {
            _mediciModel = new MediciModel();
            DeleteEmployeeCommand = new BaseCommand(ExecuteDeleteEmployee);
        }

        private void ExecuteDeleteEmployee(object parameter)
        {
            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                MessageBox.Show("Error: Employee name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int? idAngajat = _mediciModel.GetMedicIdByNumeCompletFaraTitulatura(EmployeeName);
                if (idAngajat == null)
                {
                    MessageBox.Show($"Error: Employee '{EmployeeName}' not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool isDeleted = _mediciModel.DeleteEmployeeById(idAngajat.Value);
                if (!isDeleted)
                {
                    MessageBox.Show("Error: Unable to delete employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show($"Employee '{EmployeeName}' deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                EmployeeName = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
