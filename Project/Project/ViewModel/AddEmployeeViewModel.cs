using Project.Commands;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class AddEmployeeViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name)); // Notifică UI-ul despre schimbări
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
        private string _selectedFunction;
        public string SelectedFunction
        {
            get => _selectedFunction;
            set
            {
                _selectedFunction = value;
                OnPropertyChanged(nameof(SelectedFunction));
            }
        }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ObservableCollection<string> ShiftStartList { get; set; }
        public string SelectedShiftStart { get; set; }

        public ObservableCollection<string> ShiftEndList { get; set; }
        public string SelectedShiftEnd { get; set; }

        public ICommand SaveCommand { get; }

        private readonly CliniciDataContext _context;

        public AddEmployeeViewModel()
        {
            _context = new CliniciDataContext(); 

            ClinicList = new ObservableCollection<string>();
            DepartmentList = new ObservableCollection<string>();
            TitleList = new ObservableCollection<string> { "Dr.", "Admin", "Asist." };
            FunctionList = new ObservableCollection<string>();
            ShiftStartList = new ObservableCollection<string> { "08:00", "09:00", "10:00" };
            ShiftEndList = new ObservableCollection<string> { "14:00", "15:00", "16:00" };

            LoadClinicList(); 

            SaveCommand = new BaseCommand(SaveEmployee); 
        }

        // Load clinics from model
        private void LoadClinicList()
        {
            var clinicaModel = new ClinicaModel();
            var allClinics = clinicaModel.GetAllClinici(); 

            ClinicList.Clear();
            foreach (var clinic in allClinics)
            {
                ClinicList.Add(clinic.NumeClinica); 
            }
        }
        private void LoadDepartmentList()
        {
            if (string.IsNullOrWhiteSpace(SelectedClinic))
                return;

            var clinicaModel = new ClinicaModel();
            int clinicId = clinicaModel.GetIdByName(SelectedClinic); 

            var departments = _context.Departaments
                                      .Where(d => d.id_clinica == clinicId)
                                      .Select(d => d.denumire) 
                                      .ToList();

            DepartmentList.Clear();
            foreach (var department in departments)
            {
                DepartmentList.Add(department);
            }

            SelectedDepartment = DepartmentList.Count > 0 ? DepartmentList[0] : null;
        }

        private void LoadFunctionList()
        {
            FunctionList.Clear();

            if (SelectedTitle == "Dr.")
            {
                FunctionList.Add("Medic");
                FunctionList.Add("Sef Lab");
            }
            else if (SelectedTitle == "Admin")
            {
                FunctionList.Add("Admin");
            }
            else if (SelectedTitle == "Asist.")
            {
                FunctionList.Add("Asistent");
            }

            SelectedFunction = FunctionList.Count > 0 ? FunctionList[0] : null;
        }
        private void SaveEmployee(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) ||
                    string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(PhoneNumber) ||
                    string.IsNullOrWhiteSpace(SelectedTitle) || string.IsNullOrWhiteSpace(SelectedFunction))
                {
                    MessageBox.Show("Error: All fields are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (SelectedTitle != "Admin" && SelectedFunction != "Sef Lab" &&
                    (string.IsNullOrWhiteSpace(SelectedClinic) || string.IsNullOrWhiteSpace(SelectedDepartment) ||
                    string.IsNullOrWhiteSpace(SelectedShiftStart) || string.IsNullOrWhiteSpace(SelectedShiftEnd)))
                {
                    MessageBox.Show("Error: Clinic, Department, and Shift Hours are required for non-Admin and non-Sef Lab titles.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (StartDate.HasValue && EndDate.HasValue && StartDate.Value >= EndDate.Value)
                {
                    MessageBox.Show("Error: Start date must be earlier than end date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var angajatModel = new MediciModel();
                string nume_complet = Name + " " + Surname;
                int? idAngajatExistent = angajatModel.GetMedicIdByNumeCompletFaraTitulatura(nume_complet);
                if (idAngajatExistent != null)
                {
                    MessageBox.Show("Error: Angajatul exista deja in baja de date", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                var newEmployee = new MediciModel
                {
                    Titulatura = SelectedTitle,
                    Nume = Name,
                    Prenume = Surname,
                    Username = Username,
                    Parola = Password,
                    Email = Email,
                    Telefon = PhoneNumber,
                    Sectie = SelectedFunction == "Sef Lab" ? null : (SelectedTitle == "Admin" ? null : SelectedDepartment),
                    Program = SelectedFunction == "Sef Lab" ? null : $"{SelectedShiftStart} - {SelectedShiftEnd}",
                    Functie = SelectedFunction,
                    DataIncadrare = StartDate?.ToString("yyyy-MM-dd"),
                    IdClinica = SelectedTitle == "Admin" ? null : _context.Clinicas.FirstOrDefault(c => c.nume_clinica == SelectedClinic)?.id_clinica
                };

                _context.Angajats.InsertOnSubmit(new Angajat
                {
                    titulatura = newEmployee.Titulatura,
                    nume = newEmployee.Nume,
                    prenume = newEmployee.Prenume,
                    username = newEmployee.Username,
                    parola = newEmployee.Parola,
                    email = newEmployee.Email,
                    telefon = newEmployee.Telefon,
                    specialitate = newEmployee.Sectie,
                    imagine_cale = null,
                    rating = SelectedTitle == "Admin" ? (decimal?)null : 0
                });
                _context.SubmitChanges();

                int newEmployeeId = _context.Angajats.OrderByDescending(a => a.id_angajat).First().id_angajat;

                _context.Functies.InsertOnSubmit(new Functie
                {
                    id_angajat = newEmployeeId,
                    id_clinica = newEmployee.IdClinica,
                    nume_functie = newEmployee.Functie,
                    data_incadrare = StartDate,
                    data_expirare_contract = EndDate
                });
                _context.SubmitChanges();

                if (SelectedTitle != "Admin" && SelectedFunction != "Sef Lab")
                {
                    var incadrare = new IncadrareDepartamentModel
                    {
                        id_departament = _context.Departaments.FirstOrDefault(d => d.denumire == SelectedDepartment)?.id_departament,
                        id_angajat = newEmployeeId,
                        intrare_tura = SelectedShiftStart,
                        iesire_tura = SelectedShiftEnd
                    };
                    incadrare.AdaugaIncadrare();
                }

                MessageBox.Show("Employee saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
