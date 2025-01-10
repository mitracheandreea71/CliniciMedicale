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

namespace Project.ViewModel
{
    internal class MedicWindowViewModel : BaseViewModel
    {
        private MediciModel medic;
        private object _currentView;

        public ICommand ViewProfile { get; }
        public ICommand ViewProgramari { get; }

        public MedicWindowViewModel(MediciModel medic)
        {
            this.medic = medic;
            _currentView = new MedicHomeView(medic);
            ViewProfile = new BaseCommand(_ => CurrentView = new MedicProfileView(medic));
            ViewProgramari = new BaseCommand(_ => CurrentView = new ProgramariMedicView(medic));

            EventAggregator.Instance.Subscribe<ViewChangeMessage>(message =>
            {
                switch (message.NewView)
                {
                    case "EditProgramare":
                        CurrentView = new EditProgramareMedicView(message.Consultatie,message.Medic);
                        break;
                    case "AddDiagnostic":
                        CurrentView = new AddDiagnosticView(message.Consultatie,message.Medic); 
                        break;
                }
            });
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
