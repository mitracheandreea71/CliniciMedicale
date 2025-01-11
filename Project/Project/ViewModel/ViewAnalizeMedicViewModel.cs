using Project.Commands;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class ViewAnalizeMedicViewModel : BaseViewModel
    {
        private MediciModel _medic;
        private ObservableCollection<BuletinAnalizeModel> _buletine;

        public ICommand showAddRezultatAnalizeView { get; set; }
        public ViewAnalizeMedicViewModel(MediciModel medic)
        {
            _medic = medic;
            _buletine = new BuletinAnalizeModel().GetBuletineByMedicID(medic.IdAngajat);
            showAddRezultatAnalizeView = new BaseCommand(c => verifyDate(c as BuletinAnalizeModel));

        }
        void verifyDate(BuletinAnalizeModel buletin)
        {
            if (DateTime.Now < buletin.DataRecoltare)
            {
                MessageBox.Show($"Nu poti adauga un diagnostic inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            EventAggregator.Instance.Publish(new ViewChangeMessage<BuletinAnalizeModel>("AddRezAnalize",buletin, _medic));
        }
        public ObservableCollection<BuletinAnalizeModel> Analize
        { 
            get => _buletine;
        }
    }
}
