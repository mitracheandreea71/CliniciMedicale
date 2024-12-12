using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    internal class CliniciMoreViewModel : BaseViewModel
    {
        public string _numeOras;

        public ObservableCollection<string> _ListaOrase;
        public ObservableCollection<ClinicaModel> _ToateClinicile;
        public ObservableCollection<ClinicaModel> _CliniciFiltrate;
        public CliniciMoreViewModel() { 
            _numeOras = string.Empty;
            _ToateClinicile = (new ClinicaModel()).GetAllClinici();
            _CliniciFiltrate = (new ClinicaModel()).GetAllClinici();
            _ListaOrase = (new ClinicaModel()).GetAllOrase(); 
        }

        public ObservableCollection<ClinicaModel> Clinici
        {
            get => _CliniciFiltrate;
            set {
                _CliniciFiltrate = value;
                OnPropertyChanged(nameof(Clinici));
            }
        }
        public string NumeOras
        {
            get => _numeOras;
            set {
                _numeOras = value;
                OnPropertyChanged(nameof(NumeOras));
                AplicaFiltruOras();
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

        private void AplicaFiltruOras()
        {
            if (!string.IsNullOrEmpty(_numeOras))
                Clinici = new ObservableCollection<ClinicaModel>(_ToateClinicile.Where(m => m.Oras.ToLower().Contains(_numeOras.ToLower())));
        }
    }
}
