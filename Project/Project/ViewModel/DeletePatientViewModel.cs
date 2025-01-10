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
    internal class DeletePatientViewModel : BaseViewModel
    {
        public string Cnp { get; set; }

        public ICommand DeletePatientCommand { get; }

        private readonly CliniciDataContext _context;

        public DeletePatientViewModel()
        {
            _context = new CliniciDataContext();
            DeletePatientCommand = new BaseCommand(DeletePatient);
        }

        private void DeletePatient(object parameter)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Cnp))
                {
                    MessageBox.Show("Introduceți CNP-ul pacientului!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var patientToDelete = _context.Pacients.FirstOrDefault(p => p.id_pacient == int.Parse(Cnp));
                if (patientToDelete == null)
                {
                    MessageBox.Show("Pacientul nu a fost găsit!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var asigurariToDelete = _context.Asigurares.Where(a => a.id_pacient == patientToDelete.id_pacient).ToList();
                _context.Asigurares.DeleteAllOnSubmit(asigurariToDelete);

                var buletineToDelete = _context.Buletin_Analizes.Where(b => b.id_pacient == patientToDelete.id_pacient).ToList();

                foreach (var buletin in buletineToDelete)
                {
                    var rezultateToDelete = _context.Rezultat_Analizes.Where(r => r.id_buletin_analize == buletin.id_buletin).ToList();
                    _context.Rezultat_Analizes.DeleteAllOnSubmit(rezultateToDelete);
                }

                _context.Buletin_Analizes.DeleteAllOnSubmit(buletineToDelete);

                var consultatiiToDelete = _context.Consultaties.Where(c => c.id_pacient == patientToDelete.id_pacient).ToList();

                foreach (var consultatie in consultatiiToDelete)
                {
                    var diagnosticeToDelete = _context.Diagnostics.Where(d => d.id_consultatie == consultatie.id_consultatie).ToList();
                    _context.Diagnostics.DeleteAllOnSubmit(diagnosticeToDelete);
                }

                _context.Consultaties.DeleteAllOnSubmit(consultatiiToDelete);

                var facturiToDelete = _context.Facturas.Where(f => f.id_pacient == patientToDelete.id_pacient).ToList();

                foreach (var factura in facturiToDelete)
                {
                    var consultatiiFacturiToDelete = _context.Consultatii_Facturas.Where(cf => cf.id_factura == factura.id_factura).ToList();
                    _context.Consultatii_Facturas.DeleteAllOnSubmit(consultatiiFacturiToDelete);

                    var analizeFacturiToDelete = _context.Analize_Facturas.Where(af => af.id_factura == factura.id_factura).ToList();
                    _context.Analize_Facturas.DeleteAllOnSubmit(analizeFacturiToDelete);
                }

                _context.Facturas.DeleteAllOnSubmit(facturiToDelete);

                _context.Pacients.DeleteOnSubmit(patientToDelete);

                _context.SubmitChanges();

                MessageBox.Show("Pacientul și toate datele asociate au fost șterse cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
