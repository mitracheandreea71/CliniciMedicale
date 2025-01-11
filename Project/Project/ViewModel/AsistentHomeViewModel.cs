using Project.Commands;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class AsistentHomeViewModel : BaseViewModel
    {
        private AsistentiModel _asistent;
        public ICommand showViewAnalizeAsist { get; }
        public ICommand showViewProgramariAsist { get; }
        public ICommand showViewAsigurariAsist { get; }
        public AsistentHomeViewModel(AsistentiModel asistent)
        {
            _asistent = asistent;
            showViewAnalizeAsist = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<AsistentiModel>("ViewAnalizeAsist", _asistent)); });
            showViewProgramariAsist = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<AsistentiModel>("ViewProgramariAsist", _asistent)); });
            showViewAsigurariAsist = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<AsistentiModel>("ViewAsigurariAsist", _asistent)); });
        }
        public string WelcomeMessage
        {
            get => $"Bine ai venit, {_asistent.Nume} {_asistent.Prenume}!";
        }
    }
}
