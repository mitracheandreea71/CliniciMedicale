using Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Project.ViewModel
{
    internal class DeleteClinicViewModel : BaseViewModel
    {
        public string ClinicName { get; set; }

        public ICommand DeleteClinicCommand { get; }

        private readonly CliniciDataContext _context;

        public DeleteClinicViewModel()
        {
            _context = new CliniciDataContext();
            DeleteClinicCommand = new BaseCommand(DeleteClinic);
        }

        private void DeleteClinic(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ClinicName))
                {
                    MessageBox.Show("Introduceți numele clinicii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var clinicToDelete = _context.Clinicas.FirstOrDefault(c => c.nume_clinica == ClinicName);
                if (clinicToDelete == null)
                {
                    MessageBox.Show("Clinica nu a fost găsită!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var departmentsToDelete = _context.Departaments.Where(d => d.id_clinica == clinicToDelete.id_clinica).ToList();

                foreach (var department in departmentsToDelete)
                {
                    var employeeAssignments = _context.Incadrare_Departaments.Where(i => i.id_departament == department.id_departament).ToList();

                    foreach (var assignment in employeeAssignments)
                    {
                        var employeeFunction = _context.Functies.FirstOrDefault(f => f.id_angajat == assignment.id_angajat);
                        if (employeeFunction != null)
                        {
                            _context.Functies.DeleteOnSubmit(employeeFunction);
                        }

                        var employee = _context.Angajats.FirstOrDefault(a => a.id_angajat == assignment.id_angajat);
                        if (employee != null)
                        {
                            _context.Angajats.DeleteOnSubmit(employee);
                        }

                        _context.Incadrare_Departaments.DeleteOnSubmit(assignment);
                    }

                    _context.Departaments.DeleteOnSubmit(department);
                }
                _context.Clinicas.DeleteOnSubmit(clinicToDelete);
                _context.SubmitChanges();

                MessageBox.Show("Clinica și toate datele asociate au fost șterse cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
