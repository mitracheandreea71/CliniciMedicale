using Project.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Project.Model;
using System.Linq;

namespace Project.ViewModel
{
    internal class AddDepartmentViewModel : BaseViewModel
    {
        private string _departmentName;
        public string DepartmentName
        {
            get => _departmentName;
            set
            {
                _departmentName = value;
                OnPropertyChanged(nameof(DepartmentName));
            }
        }

        private string _selectedClinic;
        public string SelectedClinic
        {
            get => _selectedClinic;
            set
            {
                _selectedClinic = value;
                OnPropertyChanged(nameof(SelectedClinic));
            }
        }

        public ObservableCollection<string> ClinicList { get; set; }
        public ICommand AddDepartmentCommand { get; }

        private readonly ClinicaModel _clinicaModel;

        public AddDepartmentViewModel()
        {
            _clinicaModel = new ClinicaModel();
            ClinicList = new ObservableCollection<string>(_clinicaModel.GetAllClinics());

            AddDepartmentCommand = new BaseCommand(ExecuteAddDepartment);
        }

        private void ExecuteAddDepartment(object parameter)
        {
            if (string.IsNullOrWhiteSpace(DepartmentName) || string.IsNullOrWhiteSpace(SelectedClinic))
            {
                MessageBox.Show("Both Clinic and Department Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool isAdded = _clinicaModel.AddDepartment(SelectedClinic, DepartmentName);

            if (isAdded)
            {
                MessageBox.Show("Department added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                DepartmentName = string.Empty;
                SelectedClinic = null;
            }
            else
            {
                MessageBox.Show("A department with the same name already exists or an error occurred.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
