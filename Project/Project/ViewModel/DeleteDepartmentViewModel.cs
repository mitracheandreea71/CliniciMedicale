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
    internal class DeleteDepartmentViewModel : BaseViewModel
    {
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

        private string _selectedDepartment;
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

        private readonly CliniciEntities _context;

        public DeleteDepartmentViewModel()
        {
            _context = new CliniciEntities();

            ClinicList = new ObservableCollection<string>();
            DepartmentList = new ObservableCollection<string>();

            LoadClinicList();

            DeleteDepartmentCommand = new BaseCommand(ExecuteDeleteDepartment);
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

        private void LoadDepartmentList()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedClinic))
                {
                    DepartmentList.Clear();
                    return;
                }

                var clinicId = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == SelectedClinic)?.id_clinica;

                if (clinicId == null)
                {
                    DepartmentList.Clear();
                    return;
                }

                var departments = _context.Departaments
                                           .Where(d => d.id_clinica == clinicId)
                                           .Select(d => d.denumire)
                                           .ToList();

                DepartmentList.Clear();

                foreach (var department in departments)
                {
                    DepartmentList.Add(department);
                }
            }
            catch (Exception ex)
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

                var clinicId = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == SelectedClinic)?.id_clinica;

                if (clinicId == null)
                {
                    MessageBox.Show("Selected clinic not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var department = _context.Departaments.FirstOrDefault(d => d.denumire == SelectedDepartment && d.id_clinica == clinicId);

                if (department == null)
                {
                    MessageBox.Show("Selected department not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Delete employees associated with the department
                var employeesToDelete = _context.Angajats.Where(e => e.specialitate == SelectedDepartment).ToList();
                foreach (var employee in employeesToDelete)
                {
                    // Delete from Functie
                    var functions = _context.Functies.Where(f => f.id_angajat == employee.id_angajat).ToList();
                    _context.Functies.RemoveRange(functions);

                    // Delete from Incadrare_Departament
                    var incadrari = _context.Incadrare_Departament.Where(i => i.id_angajat == employee.id_angajat).ToList();
                    _context.Incadrare_Departament.RemoveRange(incadrari);

                    // Delete from Angajat
                    _context.Angajats.Remove(employee);
                }

                // Delete the department itself
                _context.Departaments.Remove(department);
                _context.SaveChanges();

                MessageBox.Show("Department and associated employees deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload departments
                LoadDepartmentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the department: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
