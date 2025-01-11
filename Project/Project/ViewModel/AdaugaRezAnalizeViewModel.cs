using System;
using Project.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Project.ViewModel
{
    internal class AdaugaRezAnalizeViewModel : BaseViewModel
    {
        private BuletinAnalizeModel _buletin;
        private MediciModel _medic;
        public AdaugaRezAnalizeViewModel(BuletinAnalizeModel buletin, MediciModel medic)
        {
            _buletin = buletin;
            _medic = medic;
        }
        public BuletinAnalizeModel BuletinAnalize
        {
            get => _buletin;
            set
            {
                _buletin = value;
                OnPropertyChanged(nameof(BuletinAnalize));
            }
        }
        public string CategorieAnalize
        {
            get => $"{_buletin.FormularAnalize.TipAnalize.ToUpper()} - VALIDAT {_buletin.NumeSefLab.ToUpper()}";
            set
            {
                _buletin.FormularAnalize.TipAnalize = value;
                OnPropertyChanged(nameof(CategorieAnalize));
            }
        }
        public string NumeAnalize
        {
            get => _buletin.FormularAnalize.NumeFormular;
            set
            {
                _buletin.FormularAnalize.NumeFormular = value;
                OnPropertyChanged(nameof(NumeAnalize));
            }
        }
        public ObservableCollection<RezultatAnalizeModel> ListaAnalize
        {
            get => _buletin.RezultateAnalize;
            set
            {
                _buletin.RezultateAnalize = value;
                OnPropertyChanged(nameof(ListaAnalize));
            }
        }
    }
}
