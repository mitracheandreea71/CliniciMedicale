using Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;

namespace Project.ViewModel
{
    internal class MedicWindowViewModel : BaseViewModel
    {
        private MediciModel medic;
        public MedicWindowViewModel(MediciModel medic)
        {
            this.medic = medic;
        }

        public string CaleImagine
        {
            get => medic.CaleImagine;
        }
    }
}
