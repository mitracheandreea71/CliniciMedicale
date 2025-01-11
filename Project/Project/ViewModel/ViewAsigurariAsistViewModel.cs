using Project.Commands;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    public class ViewAsigurariAsistViewModel : BaseViewModel
    {
        private AsistentiModel _asistent;
        private string _asigurator;
        private string _asigurare;
        private string _dataAsigurare;
        private string _dataExpirare;
        private string _cnppacient;
        public ICommand BackButton { get; }
        public ICommand VerificaAsigurare { get; }
        public ViewAsigurariAsistViewModel(AsistentiModel asistent)
        { 
            _asistent = asistent;
            BackButton = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<AsistentiModel>("HomePage", _asistent)); });
            VerificaAsigurare = new BaseCommand(c => verificaPacient(int.Parse(_cnppacient)));
            _asigurare = null;
        }

        void verificaPacient(int cnpPacient)
        {
            if (!new AsigurareModel().isPacientInsured(int.Parse(_cnppacient)))
            {
                _asigurare = null;
                MessageBox.Show($"Pacientul nu este asigurat", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            AsigurareModel asigurare = new AsigurareModel().getAsigurareByPacientId(int.Parse(_cnppacient));
            Asigurator = asigurare.NumeAsigurator;
            TipAsigurare = asigurare.NumeAsigurare;
            DataExpirare = asigurare.DataExpirare.ToString();
            DataAsigurare = asigurare.DataAsigurare.ToString();
        }
        public string CNPPacient
        { 
            get => _cnppacient;
            set 
            {
                _cnppacient = value;
                OnPropertyChanged(nameof(CNPPacient));
            }
        }
        public string Asigurator
        {
            get
            {
                return _asigurator;
            }
            set
            {
                _asigurator = value;
                OnPropertyChanged(nameof(Asigurator));
            }
        }
        public string TipAsigurare
        {
            get
            {
                return _asigurare;
            }
            set
            {
                _asigurare = value;
                OnPropertyChanged(nameof(TipAsigurare));
            }
        }
        public string DataAsigurare
        {
            get
            {
                return _dataAsigurare;
            }
            set
            {
                _dataAsigurare = value;
                OnPropertyChanged(nameof(DataAsigurare));
            }
        }
        public string DataExpirare
        {
            get
            {
                return _dataExpirare;
            }
            set
            {
                _dataExpirare = value;
                OnPropertyChanged(nameof(DataExpirare));
            }
        }
    }
}
