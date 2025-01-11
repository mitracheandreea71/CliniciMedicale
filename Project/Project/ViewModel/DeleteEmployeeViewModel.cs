using System;
using System.Linq;
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

        private readonly CliniciDataContext _context;

        public DeleteEmployeeViewModel()
        {
            _context = new CliniciDataContext();
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
                var mediciModel = new MediciModel();
                int? idAngajat =  mediciModel.GetMedicIdByNumeCompletFaraTitulatura(EmployeeName);
                if (idAngajat == null)
                {
                    MessageBox.Show($"Error: Employee '{EmployeeName}' not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var functie = _context.Functies.FirstOrDefault(f => f.id_angajat == idAngajat);
                if (functie != null)
                {
                    _context.Functies.DeleteOnSubmit(functie);
                }

                var incadrari = _context.Incadrare_Departaments.Where(i => i.id_angajat == idAngajat).ToList();
                if (incadrari.Any())
                {
                    _context.Incadrare_Departaments.DeleteAllOnSubmit(incadrari);
                }
                var angajat = _context.Angajats.FirstOrDefault(a => a.id_angajat == idAngajat);
                _context.Angajats.DeleteOnSubmit(angajat);

                _context.SubmitChanges();

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
