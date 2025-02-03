using Project.Commands;
using Project.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class AddEmployeeViewModel : BaseViewModel
    {
        private readonly ClinicaModel _clinicaModel;
        private readonly MediciModel _mediciModel;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ObservableCollection<string> ClinicList { get; set; }

        private string _selectedClinic;
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

        public ObservableCollection<string> DepartmentList { get; set; }
        public string SelectedDepartment { get; set; }

        public ObservableCollection<string> TitleList { get; set; }
        private string _selectedTitle;
        public string SelectedTitle
        {
            get => _selectedTitle;
            set
            {
                _selectedTitle = value;
                OnPropertyChanged(nameof(SelectedTitle));
                LoadFunctionList();
            }
        }
        public ObservableCollection<string> FunctionList { get; set; }
        public string SelectedFunction { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ObservableCollection<string> ShiftStartList { get; set; }
        public string SelectedShiftStart { get; set; }

        public ObservableCollection<string> ShiftEndList { get; set; }
        public string SelectedShiftEnd { get; set; }

        public ICommand SaveCommand { get; }

        public AddEmployeeViewModel()
        {
            _clinicaModel = new ClinicaModel();
            _mediciModel = new MediciModel();

            ClinicList = new ObservableCollection<string>(_clinicaModel.GetAllClinici().Select(c => c.NumeClinica));
            DepartmentList = new ObservableCollection<string>();
            TitleList = new ObservableCollection<string> { "Dr.", "Admin", "Asist." };
            FunctionList = new ObservableCollection<string>();
            ShiftStartList = new ObservableCollection<string> { "08:00", "09:00", "10:00" };
            ShiftEndList = new ObservableCollection<string> { "14:00", "15:00", "16:00" };

            SaveCommand = new BaseCommand(SaveEmployee);
        }

        private void LoadDepartmentList()
        {
            DepartmentList.Clear();
            if (string.IsNullOrWhiteSpace(SelectedClinic)) return;

            var departments = _clinicaModel.GetDepartmentsByClinic(SelectedClinic);
            foreach (var department in departments)
            {
                DepartmentList.Add(department);
            }
        }

        private void LoadFunctionList()
        {
            FunctionList.Clear();
            if (SelectedTitle == "Dr.") FunctionList.Add("Medic");
            else if (SelectedTitle == "Admin") FunctionList.Add("Admin");
            else if (SelectedTitle == "Asist.") FunctionList.Add("Asistent");

            SelectedFunction = FunctionList.FirstOrDefault();
        }

        private void SaveEmployee(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) ||
                string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(SelectedTitle) || string.IsNullOrWhiteSpace(SelectedFunction))
            {
                MessageBox.Show("Error: All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int? clinicId = _clinicaModel.GetClinicIdByName(SelectedClinic);

            bool success = _mediciModel.AddEmployee(
                SelectedTitle, Name, Surname, Username, Password, Email, PhoneNumber,
                SelectedFunction, SelectedFunction == "Sef Lab" ? null : SelectedDepartment,
                clinicId, StartDate, EndDate, SelectedShiftStart, SelectedShiftEnd
            );

            if (success)
            {
                MessageBox.Show("Employee saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error saving employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
