using Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Project.Model;

namespace Project.ViewModel
{
    internal class ExtendEmployeeContractViewModel : BaseViewModel
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

        private DateTime? _newExpiryDate;
        public DateTime? NewExpiryDate
        {
            get => _newExpiryDate;
            set
            {
                _newExpiryDate = value;
                OnPropertyChanged(nameof(NewExpiryDate));
            }
        }

        public ICommand ExtendContractCommand { get; }

        private readonly CliniciEntities _context;

        public ExtendEmployeeContractViewModel()
        {
            _context = new CliniciEntities();
            ExtendContractCommand = new BaseCommand(ExecuteExtendContract);
        }

        private void ExecuteExtendContract(object parameter)
        {
            if (string.IsNullOrWhiteSpace(EmployeeName))
            {
                MessageBox.Show("Error: Employee name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!NewExpiryDate.HasValue)
            {
                MessageBox.Show("Error: A new expiry date must be selected.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



            try
            {
                //var employee = _context.Angajats.FirstOrDefault(a => a.nume == EmployeeName);
                var angajatModel = new MediciModel();
                string idAngajat = angajatModel.GetMedicIdByNumeCompletFaraTitulatura(EmployeeName).ToString();
                var angajat = angajatModel.GetMedicById(int.Parse(idAngajat));

                if (angajat == null)
                {
                    MessageBox.Show($"Error: Employee '{EmployeeName}' not found.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var functie = _context.Functies.FirstOrDefault(f => f.id_angajat == angajat.IdAngajat);

                if (functie == null)
                {
                    MessageBox.Show("Error: No contract found for the selected employee.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (NewExpiryDate <= functie.data_expirare_contract)
                {
                    MessageBox.Show($"Error: The new expiry date must be later than the current expiry date ({functie.data_expirare_contract:yyyy-MM-dd}).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                functie.data_expirare_contract = NewExpiryDate;
                _context.SaveChanges();

                MessageBox.Show($"Contract extended successfully for '{EmployeeName}' until {NewExpiryDate:yyyy-MM-dd}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                EmployeeName = string.Empty;
                NewExpiryDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
