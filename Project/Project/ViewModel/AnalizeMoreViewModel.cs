using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    internal class AnalizeMoreViewModel : BaseViewModel
    {
        public ObservableCollection<FormularAnalizeModel> _ListaFormulareAnalize;
        public ObservableCollection<FormularAnalizeModel> _ListaFormulareAnalizeFiltrate;
        public ObservableCollection<string> _ListaTipuriAnalize;
        public string _TipAnalize;
        public AnalizeMoreViewModel()
        {
            _ListaTipuriAnalize = (new AnalizeModel()).GetAllAnalizeType();
            _ListaFormulareAnalize = (new FormularAnalizeModel()).GetAllFormulare();
            _ListaFormulareAnalizeFiltrate = (new FormularAnalizeModel()).GetAllFormulare();
        }

        public string TipAnalize
        {
            get => _TipAnalize;
            set 
            {
                _TipAnalize = value;
                OnPropertyChanged(nameof(TipAnalize));
                AplicaFiltruTip(_TipAnalize);
            }
        }
        public ObservableCollection<string> ListaTipuriAnalize
        {
            get => _ListaTipuriAnalize;
            set
            {
                _ListaTipuriAnalize = value;
                OnPropertyChanged(nameof(ListaTipuriAnalize));
            }
        }

        public ObservableCollection<FormularAnalizeModel> ListaFormulareAnalize
        {
            get => _ListaFormulareAnalize;
            set
            {
                _ListaFormulareAnalize = value;
                OnPropertyChanged(nameof(ListaFormulareAnalize));
            }
        }
        public void AplicaFiltruTip(string tipAnalize)
        {
            if (!string.IsNullOrEmpty(tipAnalize))
                ListaFormulareAnalize = new ObservableCollection<FormularAnalizeModel>(_ListaFormulareAnalize.Where(l => l.TipAnalize.ToLower().Contains(tipAnalize.ToLower())));
        }
    }
}
