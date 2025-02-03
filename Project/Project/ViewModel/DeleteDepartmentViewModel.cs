using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Project.Commands;
using Project.Model;

namespace Project.ViewModel
{
    internal class DeleteDepartmentViewModel : BaseViewModel
    {
        private string _selectedClinic;
        private string _selectedDepartment;
        private readonly ClinicaModel _clinicaModel;
        private readonly MediciModel _mediciModel;

        public string SelectedClinic
        {
            get => _selectedClinic;
            set
            {
                _selectedClinic = value;
                OnPropertyChanged(nameof(SelectedClinic));
                LoadDepartmentList();
            }
        }

        public string SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        public ObservableCollection<string> ClinicList { get; set; }
        public ObservableCollection<string> DepartmentList { get; set; }

        public ICommand DeleteDepartmentCommand { get; }

        public DeleteDepartmentViewModel()
        {
            _clinicaModel = new ClinicaModel();
            _mediciModel = new MediciModel();

            ClinicList = new ObservableCollection<string>();
            DepartmentList = new ObservableCollection<string>();

            LoadClinicList();

            DeleteDepartmentCommand = new BaseCommand(ExecuteDeleteDepartment);
        }

        private void LoadClinicList()
        {
            try
            {
                var clinics = _clinicaModel.GetAllClinics();
                ClinicList.Clear();
                foreach (var clinic in clinics)
                {
                    ClinicList.Add(clinic);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred while loading clinics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDepartmentList()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedClinic))
                {
                    DepartmentList.Clear();
                    return;
                }

                var departments = _clinicaModel.GetDepartmentsByClinic(SelectedClinic);
                DepartmentList.Clear();
                foreach (var department in departments)
                {
                    DepartmentList.Add(department);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred while loading departments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteDepartment(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedClinic) || string.IsNullOrWhiteSpace(SelectedDepartment))
                {
                    MessageBox.Show("Please select both a clinic and a department.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool employeesDeleted = _mediciModel.DeleteEmployeesByDepartment(SelectedDepartment);
                bool departmentDeleted = _clinicaModel.DeleteDepartment(SelectedClinic, SelectedDepartment);

                if (departmentDeleted)
                {
                    MessageBox.Show("Department and associated employees deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDepartmentList();
                }
                else
                {
                    MessageBox.Show("Error: Department not found or could not be deleted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the department: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
