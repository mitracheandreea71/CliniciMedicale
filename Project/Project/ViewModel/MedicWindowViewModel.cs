using Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Model;
using System.Windows.Input;
using Project.Commands;
using System.Data;
using System.Windows;

namespace Project.ViewModel
{
    internal class MedicWindowViewModel : BaseViewModel
    {
        private MediciModel medic;
        private object _currentView;

        public ICommand ViewProfile { get; }
        public ICommand ViewProgramari { get; }
        public ICommand ViewAnalize { get; }

        public MedicWindowViewModel(MediciModel medic)
        {
            this.medic = medic;
            _currentView = new MedicHomeView(medic);
            ViewProfile = new BaseCommand(_ => CurrentView = new MedicProfileView(medic));
            ViewProgramari = new BaseCommand(_ => verifyMedic());
            ViewAnalize = new BaseCommand(_ => verifySefLab());

            EventAggregator.Instance.Subscribe<ViewChangeMessage<ConsultatieModel>>(message =>
            {
                switch (message.NewView)
                {
                    case "EditProgramare":
                        CurrentView = new EditProgramareMedicView(message.Model, message.Medic);
                        break;
                    case "AddDiagnostic":
                        CurrentView = new AddDiagnosticView(message.Model, message.Medic);
                        break;
                }
            });
            EventAggregator.Instance.Subscribe<ViewChangeMessage<BuletinAnalizeModel>>(message =>
            {
                switch (message.NewView)
                {
                    case "AddRezAnalize":
                        CurrentView = new AdaugaRezAnalizeView(message.Model, message.Medic);
                        break;
                }
            });
        }
        public void verifySefLab()
        {
            if (medic.Functie != "Sef Lab")
            {
                MessageBox.Show($"Nu ai analize atata timp cat nu esti Sef Lab.", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentView = new ViewAnalizeMedicView(medic);
        }
        public void verifyMedic()
        {
            if (medic.Functie != "Medic") {
                MessageBox.Show($"Nu ai programari atata timp cat nu esti medic", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentView = new ProgramariMedicView(medic);
        }

        public string CaleImagine
        {
            get => medic.CaleImagine;
        }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }
}
