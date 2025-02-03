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
    internal class AddClinicViewModel:BaseViewModel
    {
        public string ClinicName { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Program { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string CIF { get; set; }
        public string IBAN { get; set; }
        public string Bank { get; set; }

        public ICommand AddClinicCommand { get; }

        private readonly CliniciEntities _context;
        private readonly ClinicaModel _clinicaModel;

        public AddClinicViewModel()
        {
            _context = new CliniciEntities();
            _clinicaModel = new ClinicaModel();
            AddClinicCommand = new BaseCommand(AddClinic);
        }

        private void AddClinic(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ClinicName) || string.IsNullOrWhiteSpace(County) ||
                    string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(Address) ||
                    string.IsNullOrWhiteSpace(Program) || string.IsNullOrWhiteSpace(ContactNumber) ||
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(CIF) ||
                    string.IsNullOrWhiteSpace(IBAN) || string.IsNullOrWhiteSpace(Bank))
                {
                    MessageBox.Show("Toate câmpurile sunt obligatorii!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newClinic = new ClinicaModel
                {
                    NumeClinica = ClinicName,
                    Judet = County,
                    Oras = City,
                    Adresa = Address,
                    Program = Program,
                    NrContact = ContactNumber,
                    Email = Email,
                    CIF = CIF,
                    IBAN = IBAN,
                    Banca = Bank
                };

                _clinicaModel.AddClinic(newClinic);

                MessageBox.Show("Clinica a fost adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
    }
}
