using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class FacturaModel : BaseViewModel
    {
        private int _index;
        private string _denumire;
        private string _um;
        private int _cantitate;
        private decimal _pretUnitar;
        private decimal _tvaProcent;
        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }
        public string Denumire
        {
            get => _denumire;
            set
            {
                _denumire = value;
                OnPropertyChanged(nameof(Denumire));
            }
        }
        public string UM
        {
            get => _um;
            set
            {
                _um = value;
                OnPropertyChanged(nameof(UM));
            }
        }
        public int Cantitate
        {
            get => _cantitate;
            set
            {
                _cantitate = value;
                OnPropertyChanged(nameof(Cantitate));
                OnPropertyChanged(nameof(Valoare));
                OnPropertyChanged(nameof(ValoareTVA));
            }
        }
        public decimal PretUnitar
        {
            get => _pretUnitar;
            set
            {
                _pretUnitar = value;
                OnPropertyChanged(nameof(PretUnitar));
                OnPropertyChanged(nameof(Valoare));
                OnPropertyChanged(nameof(ValoareTVA));
            }
        }
        public decimal TVAProcent
        {
            get => _tvaProcent;
            set
            {
                _tvaProcent = value;
                OnPropertyChanged(nameof(TVAProcent));
                OnPropertyChanged(nameof(ValoareTVA));
            }
        }
        public decimal Valoare => (Cantitate * PretUnitar);
        public decimal ValoareTVA => Valoare * (TVAProcent / 100);
    }
}
