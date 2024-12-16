using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.View;


namespace Project.ViewModel
{
    internal class RezultateWindowViewModel : BaseViewModel
    {
        private object _RezultateView;
        public RezultateWindowViewModel(int idBuletin, int idPacient)
        {
            _RezultateView = new AnalizeRezultatView(idBuletin,idPacient);
        }

        public object RezultateView
        {
            get => _RezultateView;
            set
            {
                _RezultateView = value;
                OnPropertyChanged(nameof(RezultateView));
            }
        }
    }
}
