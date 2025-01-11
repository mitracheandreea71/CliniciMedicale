using Project.Commands;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Project.View;

namespace Project.ViewModel
{
    internal class ProgramariMedicViewModel : BaseViewModel
    {
        private MediciModel _medic;
        private ObservableCollection<ConsultatieModel> _consultatii;
        private ConsultatieModel _consultatieSelectata { get; set; }

        public ICommand showEditView { get; set; }
        public ICommand showAddDiagnosticView { get; set; }

        public ProgramariMedicViewModel(MediciModel medic)
        { 
            _medic = medic;
            _consultatii = new ConsultatieModel().GetAllConsultatiiForMedic(_medic.IdAngajat);
            showEditView = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<ConsultatieModel>("EditProgramare", c as ConsultatieModel,_medic)); });
            showAddDiagnosticView = new BaseCommand(c => verifyDate(c as ConsultatieModel));
        }

        void verifyDate(ConsultatieModel consultatie)
        {
            if (DateTime.Now < consultatie.DataConsultatie)
            {
                MessageBox.Show($"Nu poti adauga un diagnostic inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            EventAggregator.Instance.Publish(new ViewChangeMessage<ConsultatieModel>("AddDiagnostic", consultatie, _medic));
        }

        public ConsultatieModel Consultatie
        {
            get => _consultatieSelectata;
        }

        public ObservableCollection<ConsultatieModel> Consultatii
        {
            get => _consultatii;
        }
    }
}
