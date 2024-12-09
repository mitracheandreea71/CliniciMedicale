using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.View;
using Project.Commands;

namespace Project.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand ShowMediciMoreView { get; }
        public ICommand ShowPaginaPrincipalaView { get; }
        public MainWindowViewModel() {
            CurrentView = new PaginaPrincipalaView();

            ShowMediciMoreView = new BaseCommand(_ => CurrentView = new MediciMoreView());
            ShowPaginaPrincipalaView = new BaseCommand(_ => CurrentView = new PaginaPrincipalaView());
        }
    }
}
