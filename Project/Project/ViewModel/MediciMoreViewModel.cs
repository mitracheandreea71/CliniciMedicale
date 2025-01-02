using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.Commands;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using Project.View;

namespace Project.ViewModel
{
    internal class MediciMoreViewModel : BaseViewModel
    {
        private string _numeFiltru;
        private string _numeOras;
        private string _numeLocatie;
        private string _numeSpecialitate;

        private ObservableCollection<MediciModel> _TotiMedicii;
        private ObservableCollection<MediciModel> _MediciFiltrati;

        private ObservableCollection<string> _ListaOrase;
        private ObservableCollection<string> _ListaLocatii;
        private ObservableCollection<string> _ListaSpecialitati;

        public ICommand SolicitaProgramareCommand { get; }
        public MediciMoreViewModel()
        {
            NumeFiltru = "Nume";
            _numeSpecialitate = "";
            _numeLocatie = "";
            _numeOras = "";
            _TotiMedicii = (new MediciModel()).GetAllMedici();
            _MediciFiltrati = new ObservableCollection<MediciModel>(_TotiMedicii);
            _ListaOrase = (new ClinicaModel()).GetAllOrase();
            _ListaLocatii = (new ClinicaModel()).GetAllLocatii();
            _ListaSpecialitati = (new ClinicaModel()).GetAllDepartaments();
            SolicitaProgramareCommand = new BaseCommand(ExecuteSolicitaProgramare, CanExecuteSolicitaProgramare);
        }

        private void ExecuteSolicitaProgramare(object parameter)
        {
            var medic = parameter as MediciModel;
            if (medic != null)
            {
                string caleImagine = medic.CaleImagine;

                if (!string.IsNullOrWhiteSpace(caleImagine))
                {
                    MediciModel medicGasit = new MediciModel().GetMedicByCaleImagine(caleImagine);

                    if (medicGasit != null)
                    {
                        SolicitaProgramareWindow window = new SolicitaProgramareWindow(medicGasit.IdAngajat, medicGasit.NumeComplet);
                        window.Show();
                    }
                    else
                    {
                        MessageBox.Show("Medicul nu a fost găsit în baza de date.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Calea imaginii medicului este invalidă.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Eroare: Medic invalid.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanExecuteSolicitaProgramare(object parameter)
        {
            return parameter is MediciModel;
        }
        public string NumeLocatie
        {
            get => _numeLocatie;
            set
            {
                _numeLocatie = value;
                OnPropertyChanged(nameof(NumeLocatie));
                AplicaFiltruLocatie();
            }
        }

        public string NumeOras
        {
            get => _numeOras;
            set
            {
                _numeOras = value;
                OnPropertyChanged(nameof(NumeOras));
                AplicaFiltruOras();
            }
        }

        public string NumeSpecialitate 
        {
            get => _numeSpecialitate;
            set {
                _numeSpecialitate = value;
                OnPropertyChanged(nameof(NumeSpecialitate));
                AplicaFiltruSpecialitate();
            }
        }
        public ObservableCollection<string> Specialitati
        {
            get => _ListaSpecialitati;
            set {
                _ListaSpecialitati = value;
                OnPropertyChanged(nameof(Specialitati));
            }
        }

        public ObservableCollection<string> Locatii
        {
            get => _ListaLocatii;
            set {
                _ListaLocatii = value;
                OnPropertyChanged(nameof(Locatii));
            }
        }
        public ObservableCollection<string> ListaOrase
        {
            get => _ListaOrase;
            set {
                _ListaOrase = value;
                OnPropertyChanged(nameof(ListaOrase));
            }
        } 

        public ObservableCollection<MediciModel> Medici
        {
            get => _MediciFiltrati;
            set
            {
                _MediciFiltrati = value;
                OnPropertyChanged(nameof(Medici));
            }
        }

        public string NumeFiltru
        {
            get => _numeFiltru;
            set {
                _numeFiltru = value;
                OnPropertyChanged(nameof(NumeFiltru));
                AplicaFiltruNume();
            }
        }

        private void AplicaFiltruLocatie()
        {
            if (!(string.IsNullOrEmpty(_numeLocatie)))
                Medici = (new MediciModel()).GetAllMediciPentruLocatie(_numeLocatie);
        }
        private void AplicaFiltruOras() 
        {
            if (!(string.IsNullOrEmpty(_numeOras)))
                Medici = (new MediciModel()).GetAllMediciPentruOras(_numeOras);
        }

        private void AplicaFiltruSpecialitate()
        {
            if (!(string.IsNullOrEmpty(_numeSpecialitate)))
                Medici = (new MediciModel()).GetAllMediciPentruDepartament(_numeSpecialitate);
        }
        private void AplicaFiltruNume()
        {
            if (string.Compare("Nume",NumeFiltru)!=0)
                Medici = new ObservableCollection<MediciModel>(_TotiMedicii.Where(m => m.NumeComplet.ToLower().Contains(_numeFiltru.ToLower())));
        }
    }
}
