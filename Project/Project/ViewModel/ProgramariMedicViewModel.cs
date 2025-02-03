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
        private ObservableCollection<ConsultatieModel> _consultatiiFiltrate;

        private DateTime? _selectedDate;
        private ConsultatieModel _consultatieSelectata { get; set; }

        public ICommand showEditView { get; set; }
        public ICommand showAddDiagnosticView { get; set; }

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                FiltreazaConsultatii();
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public ProgramariMedicViewModel(MediciModel medic)
        { 
            _medic = medic;
            _consultatii = new ConsultatieModel().GetAllConsultatiiForMedic(_medic.IdAngajat);
            showEditView = new BaseCommand(c => verifyDateForEdit(c as ConsultatieModel));
            showAddDiagnosticView = new BaseCommand(c => verifyDate(c as ConsultatieModel));
        }

        void verifyDateForEdit(ConsultatieModel consultatie)
        {
            if (DateTime.Now >= consultatie.DataConsultatie)
            {
                MessageBox.Show($"Nu mai poti modifica consultatia", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            EventAggregator.Instance.Publish(new ViewChangeMessage<ConsultatieModel>("EditProgramare", consultatie, _medic));
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

        public void FiltreazaConsultatii()
        {
            if (_selectedDate.HasValue)
            {
                string selectedDateString = _selectedDate.Value.ToString("yyyy-MM-dd");
                _consultatiiFiltrate = new ObservableCollection<ConsultatieModel>(
                    _consultatii.Where(b => b.DataConsultatie.Value.ToString("yyyy-MM-dd") == selectedDateString)
                );
            }
            else
            {
                _consultatiiFiltrate = _consultatii;
            }
            OnPropertyChanged(nameof(Consultatii));
        }

        public ObservableCollection<ConsultatieModel> Consultatii
        {
            get => _consultatiiFiltrate ?? _consultatii;
            set
            {
                _consultatii = value;
                FiltreazaConsultatii();
                OnPropertyChanged(nameof(Consultatii));
            }
        }
    }
}
