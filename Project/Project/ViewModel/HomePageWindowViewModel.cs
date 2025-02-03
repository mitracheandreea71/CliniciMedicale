using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.View;
using Project.Commands;
using Project.Model;

namespace Project.ViewModel
{
    internal class HomePageWindowViewModel : BaseViewModel
    {
        private object _currentView;
        private PacientModel _pacient;
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
        public ICommand ShowCliniciMoreView { get; }
        public ICommand ShowAnalizeMoreView { get; }
        public ICommand ShowRezultateAnalize { get; }
        public ICommand ShowIstoricMoreView { get; }
        public HomePageWindowViewModel(PacientModel pacient)
        {
            CurrentView = new PaginaPrincipalaView();

            _pacient = pacient;
            ShowMediciMoreView = new BaseCommand(_ => CurrentView = new MediciMoreView());
            ShowPaginaPrincipalaView = new BaseCommand(_ => CurrentView = new PaginaPrincipalaView());
            ShowCliniciMoreView = new BaseCommand(_ => CurrentView = new CliniciMoreView());
            ShowAnalizeMoreView = new BaseCommand(_ => CurrentView = new AnalizeMoreView());
            ShowRezultateAnalize = new BaseCommand(_ => CurrentView = new AnalizeView());
            ShowIstoricMoreView = new BaseCommand(_ => CurrentView = new IstoricMoreView(_pacient));
        }
    }
}
