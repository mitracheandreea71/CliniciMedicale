using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    internal class MediciViewModel : BaseViewModel
    {
        private ObservableCollection<MediciModel> _medici;
        public ObservableCollection<MediciModel> Medici => _medici;

        public MediciViewModel()
        {
            _medici = (new MediciModel()).GetAllMedici();
        }
    }
}
