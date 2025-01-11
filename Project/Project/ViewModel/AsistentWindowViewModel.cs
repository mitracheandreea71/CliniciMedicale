using Project.Model;
using System;
using Project.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Commands;

namespace Project.ViewModel
{
    public class AsistentWindowViewModel : BaseViewModel
    {
        private AsistentiModel _asistent;
        private object _currentView;
        public AsistentWindowViewModel(AsistentiModel asistent)
        {
            _asistent = asistent;
            _currentView = new AsistentHomeView(asistent);
            EventAggregator.Instance.Subscribe<ViewChangeMessage<AsistentiModel>>(message =>
            {
                switch (message.NewView)
                {
                    case "ViewAnalizeAsist":
                        CurrentView = new ViewAnalizeAsistView(message.Model);
                        break;
                    case "ViewProgramariAsist":
                        CurrentView = new ViewProgramariAsistView(message.Model);
                        break;
                    case "ViewAsigurariAsist":
                        CurrentView = new ViewAsigurariAsistView(message.Model);
                        break;
                    case "HomePage":
                        CurrentView = new AsistentHomeView(message.Model);
                        break;
                }
            });
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
