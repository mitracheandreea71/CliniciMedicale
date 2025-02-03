using Project.Commands;
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
    internal class AddDepartmentViewModel:BaseViewModel
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

        private readonly CliniciEntities _context;

        public AddDepartmentViewModel()
        {
            _context = new CliniciEntities();
            ClinicList = new ObservableCollection<string>();

            LoadClinicList();

            AddDepartmentCommand = new BaseCommand(ExecuteAddDepartment);
        }

        private void LoadClinicList()
        {
            try
            {
                var clinics = _context.Clinicas.Select(c => c.nume_clinica).ToList();
                ClinicList.Clear();

                foreach (var clinic in clinics)
                {
                    ClinicList.Add(clinic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading clinics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteAddDepartment(object parameter)
        {
            if (string.IsNullOrWhiteSpace(DepartmentName) || string.IsNullOrWhiteSpace(SelectedClinic))
            {
                MessageBox.Show("Both Clinic and Department Name are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var clinicId = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == SelectedClinic)?.id_clinica;

                if (clinicId == null)
                {
                    MessageBox.Show("Selected clinic not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingDepartment = _context.Departaments.FirstOrDefault(d => d.denumire == DepartmentName && d.id_clinica == clinicId);

                if (existingDepartment != null)
                {
                    MessageBox.Show("A department with the same name already exists in the selected clinic.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _context.Departaments.Add(new Departament
                {
                    denumire = DepartmentName,
                    id_clinica = clinicId
                });

                _context.SaveChanges();

                MessageBox.Show("Department added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Clear inputs
                DepartmentName = string.Empty;
                SelectedClinic = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the department: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
