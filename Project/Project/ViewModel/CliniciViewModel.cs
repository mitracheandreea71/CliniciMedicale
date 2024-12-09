using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;

namespace Project.ViewModel
{
    internal class CliniciViewModel : BaseViewModel
    {
        private ObservableCollection<ClinicaModel> _clinici;
        public ObservableCollection<ClinicaModel> Clinici => _clinici;

        public CliniciViewModel() {
            _clinici = (new ClinicaModel()).GetAllClinici();
        }
    }
}
