using Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Project.Model;

namespace Project.ViewModel
{
    internal class SolicitaProgramareViewModel : BaseViewModel
    {
        private int _medicID;
        private string _numeMedic;
        private DateTime? _dataProgramare;
        private string _comentarii;

        public SolicitaProgramareViewModel(int medicID, string numeMedic)
        {
            _medicID = medicID;
            _numeMedic = numeMedic;
            TrimiteProgramareCommand = new BaseCommand(ExecuteTrimiteProgramare, CanExecuteTrimiteProgramare);
        }

        public int MedicID
        {
            get => _medicID;
            set
            {
                _medicID = value;
                OnPropertyChanged(nameof(MedicID));
            }
        }

        public string NumeMedic
        {
            get => _numeMedic;
            set
            {
                _numeMedic = value;
                OnPropertyChanged(nameof(NumeMedic));
            }
        }

        public DateTime? DataProgramare
        {
            get => _dataProgramare;
            set
            {
                _dataProgramare = value;
                OnPropertyChanged(nameof(DataProgramare));
            }
        }

        public string Comentarii
        {
            get => _comentarii;
            set
            {
                _comentarii = value;
                OnPropertyChanged(nameof(Comentarii));
            }
        }

        public ICommand TrimiteProgramareCommand { get; }

        private void ExecuteTrimiteProgramare(object parameter)
        {
            if (!DataProgramare.HasValue)
            {
                MessageBox.Show("Te rugăm să selectezi o dată pentru programare.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ConsultatieModel consultatie = new ConsultatieModel
            {
                IdDoctor = _medicID,
                DataConsultatie = _dataProgramare.Value
            };
            
            

            MessageBox.Show($"Programare trimisă pentru {NumeMedic} la data de {DataProgramare.Value.ToShortDateString()}.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private bool CanExecuteTrimiteProgramare(object parameter)
        {
            return DataProgramare.HasValue;
        }
    }
}
