using Project.Commands;
using Project.Model;
using Project.View;
using Project.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Project.ViewModel
{
    internal class IstoricMoreViewModel : BaseViewModel
    {
        public struct IstoricItem
        {
            public string Tip { get; set; } 
            public string Data { get; set; } 
            public int IdItem { get; set; }

            public string ImageSource
            {
                get
                {
                    return Tip == "Analiza" ? "/Images/analiza.png" : "/Images/consultatie.png";

                }
            }
        }

        private string _cnp;
        public string Cnp
        {
            get => _cnp;
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(Cnp));
            }
        }

        private ObservableCollection<IstoricItem> _istoricItems;
        public ObservableCollection<IstoricItem> IstoricItems
        {
            get => _istoricItems;
            set
            {
                _istoricItems = value;
                OnPropertyChanged(nameof(IstoricItems));
            }
        }

        public ICommand VizualizareIstoricCommand { get; }
        public ICommand VizualizareRezultatCommand { get; }

        private readonly BuletinAnalizeModel _buletinAnalizeModel;
        private readonly ConsultatieModel _consultatieModel;
        private readonly CliniciDataContext _context;
        private readonly MediciModel _medicModel;
        public IstoricMoreViewModel()
        {
            IstoricItems = new ObservableCollection<IstoricItem>();
            _buletinAnalizeModel = new BuletinAnalizeModel();
            _consultatieModel = new ConsultatieModel();
            _context = new CliniciDataContext();
            _medicModel = new MediciModel();
            VizualizareIstoricCommand = new BaseCommand(VizualizareIstoric, CanExecuteVizualizareIstoric);
            VizualizareRezultatCommand = new BaseCommand(VizualizareRezultat);
        }

        private bool CanExecuteVizualizareIstoric(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Cnp);
        }

        private void VizualizareIstoric(object parameter)
        {
            int idP;
            if (!int.TryParse(Cnp, out idP))
            {
                MessageBox.Show("CNP-ul introdus nu este valid.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient == idP);
            if (pacient == null)
            {
                MessageBox.Show("Pacientul nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            IstoricItems.Clear();
            idP = int.Parse(Cnp);
            var analize = _buletinAnalizeModel.GetBuletineByPacientID(idP);
            foreach (var analiza in analize)
            {
                IstoricItems.Add(new IstoricItem
                {
                    Tip = "Analiza",
                    Data = analiza.DataRecoltare.ToString("dd/MM/yyyy"),
                    IdItem = analiza.IDBuletin
                });
            }
            var consultatii = _consultatieModel.GetConsultatiiByPacientID(idP);
            foreach (var consultatie in consultatii)
            {
                IstoricItems.Add(new IstoricItem
                {
                    Tip = "Consultatie",
                    Data = consultatie.DataConsultatie?.ToString("dd/MM/yyyy") + " Ora: " + consultatie.Ora,
                    IdItem = consultatie.IdConsultatie
                });
            }
           
        }

        private void VizualizareRezultat(object parameter)
        {
            if (parameter is IstoricItem item && item.Tip == "Analiza")
            {
                int idBuletin = item.IdItem;
                int idPacient = int.Parse(Cnp);

                var buletin = _buletinAnalizeModel.GetBuletineByPacientID(idPacient)
                                                  .FirstOrDefault(b => b.IDBuletin == idBuletin);

                if (buletin == null)
                {
                    MessageBox.Show("Analiza selectată nu aparține pacientului curent.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var rezultatWindow = new RezultatAnalizeWindow(idBuletin, idPacient);
                rezultatWindow.ShowDialog();
            }
            else if (parameter is IstoricItem consultatieItem && consultatieItem.Tip == "Consultatie")
            {
                int idConsultatie = consultatieItem.IdItem;
                int idPacient = int.Parse(Cnp);

                var consultatie = _consultatieModel.GetConsultatiiByPacientID(idPacient)
                                                   .FirstOrDefault(c => c.IdConsultatie == idConsultatie);

                if (consultatie == null)
                {
                    MessageBox.Show("Consultația selectată nu aparține pacientului curent.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string id_medic = consultatie.IdDoctor.ToString();
                int im = int.Parse(id_medic);
                MediciModel medic = _medicModel.GetMedicById(im);
                var vizualizareDW = new VizualizareDiagnosticWindow(consultatie,medic);
                vizualizareDW.Show();

               // MessageBox.Show($"Consultația pentru pacientul {idPacient} este validată.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Doar rezultatele analizelor sau consultațiilor pot fi vizualizate.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
