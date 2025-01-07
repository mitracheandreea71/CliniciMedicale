using Project.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.Windows;
using System.Windows;

namespace Project.ViewModel
{
    internal class AnalizeViewModel : BaseViewModel
    {
        private string _CodUnicAnalize;
        private string _CodUnicPacient;

        public ICommand TrimiteAnalizeBttn { get; }

        public AnalizeViewModel()
        {
            _CodUnicAnalize = "Cod Unic Analize";
            _CodUnicPacient = "Cod Unic Pacient";

            TrimiteAnalizeBttn = new BaseCommand(_ => OpenWindowForRezultate());
        }

        public void OpenWindowForRezultate()
        {
            if (!int.TryParse(_CodUnicPacient, out _) || !int.TryParse(_CodUnicAnalize, out _))
            {
                MessageBox.Show("Datele introduse nu sunt valide", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            RezultatAnalizeWindow window = new RezultatAnalizeWindow(int.Parse(_CodUnicAnalize),int.Parse(_CodUnicPacient));
            window.Show();
        }

        public string CodUnicAnalize
        { 
            get => _CodUnicAnalize;
            set 
            { 
                _CodUnicAnalize = value;
                OnPropertyChanged(nameof(CodUnicAnalize));
            }
        }

        public string CodUnicPacient
        {
            get => _CodUnicPacient;
            set
            {
                _CodUnicPacient = value;
                OnPropertyChanged(nameof(CodUnicAnalize));
            }
        }

    }
}
