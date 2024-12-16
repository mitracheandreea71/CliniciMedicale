using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Project.Model;

namespace Project.ViewModel
{
    internal class AnalizeRezultatViewModel : BaseViewModel
    {
        public BuletinAnalizeModel _BuletinAnalize;

        public AnalizeRezultatViewModel(int idBuletin,int idPacient) 
        {
            _BuletinAnalize = (new BuletinAnalizeModel()).GetBuletinAnalizeByID(idBuletin,idPacient);
        }

        public BuletinAnalizeModel BuletinAnalize
        {
            get => _BuletinAnalize;
            set
            {
                _BuletinAnalize = value;
                OnPropertyChanged(nameof(BuletinAnalize));
            }
        }

        public string CategorieAnalize
        {
            get => $"{_BuletinAnalize.FormularAnalize.TipAnalize.ToUpper()} - VALIDAT {_BuletinAnalize.NumeSefLab.ToUpper()}";
            set
            {
                _BuletinAnalize.FormularAnalize.TipAnalize = value;
                OnPropertyChanged(nameof(CategorieAnalize));
            }
        }
        public string NumeAnalize
        {
            get => _BuletinAnalize.FormularAnalize.NumeFormular;
            set
            {
                _BuletinAnalize.FormularAnalize.NumeFormular = value;
                OnPropertyChanged(nameof(NumeAnalize));
            }
        }

        public ObservableCollection<RezultatAnalizeModel> ListaAnalize
        {
            get => _BuletinAnalize.RezultateAnalize;
            set 
            {
                _BuletinAnalize.RezultateAnalize = value;
                OnPropertyChanged(nameof(ListaAnalize));
            }
        }
    }
}
