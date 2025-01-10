using Project.Model;
using Project.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModel
{
    internal class EditProgramareMedicViewModel : BaseViewModel
    {
        private ConsultatieModel _consultatie { get; set; }
        private MediciModel _medic { get; set; }
        private ObservableCollection<AsistentiModel> _asistenti { get; set; }
        private ObservableCollection<string> _oreDisponbile { get; set; }
        private string _numeAsistentSelectat { get; set; }
        private DateTime? _dataNouaConsultatie { get; set; }
        private string _oraProgramare { get; set; }
        private string _pretProgramare { get; set; }

        public ICommand SaveChanges { get; }
        public EditProgramareMedicViewModel(ConsultatieModel consultatie, MediciModel medic)
        {
            _consultatie = consultatie;
            _medic = medic;
            _asistenti = new ObservableCollection<AsistentiModel>((new AsistentiModel()).GetAllAsistentiPentruDepartament(medic.getMedicDepartament(medic.IdAngajat))
                                                                                        .Where(asistent => asistent.IdClinica == medic.IdClinica));
            _oraProgramare = _consultatie.Ora;
            _pretProgramare = _consultatie.Pret.ToString();
            _dataNouaConsultatie = _consultatie.DataConsultatie;
            SaveChanges = new BaseCommand(_ => saveChangesMade());
        }

        private void saveChangesMade()
        {
            _consultatie.Ora = _oraProgramare;
            if (decimal.TryParse(_pretProgramare, out decimal pret))
                _consultatie.Pret = decimal.Parse(_pretProgramare);
            else
                _consultatie.Pret = null;
            _consultatie.DataConsultatie = _dataNouaConsultatie;
            _consultatie.IdAsistent = (new AsistentiModel()).GetAsistentIdByFullName(_numeAsistentSelectat);
            try
            {
                _consultatie.SaveChanges();
                MessageBox.Show("Modificarile au fost salvate cu succes!", "Salvare", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A aparut o eroare la salvarea modificarilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public ObservableCollection<string> ListaAsistentiNume
        {
            get
            {
                ObservableCollection<string> ListaNume = new ObservableCollection<string>();
                foreach (var item in _asistenti)
                {
                    ListaNume.Add(item.NumeComplet);
                }
                return ListaNume;
            }
        }
        public string OraProgramare
        {
            get => _oraProgramare;
            set
            {
                _oraProgramare = value;
                OnPropertyChanged(nameof(OraProgramare));
            }
        }
        public DateTime? DataConsultatie
        {
            get => _dataNouaConsultatie;
            set
            {
                _dataNouaConsultatie = value;
                OnPropertyChanged(nameof(DataConsultatie));
            }
        }

        public string PretProgramare
        {
            get => _pretProgramare;
            set
            {
                _pretProgramare = value;
                OnPropertyChanged(nameof(PretProgramare));
            }
        }
        public string NumeAsistent
        {
            get => _numeAsistentSelectat;
            set
            {
                _numeAsistentSelectat = value;
                _oreDisponbile = getListaOreDisponibile();
                OnPropertyChanged(nameof(NumeAsistent));
                OnPropertyChanged(nameof(ListaOreDisponibile));
            }
        }
        public ObservableCollection<string> ListaOreDisponibile
        {
            get => _oreDisponbile;
            set
            {
                _oreDisponbile = value;
                OnPropertyChanged(nameof(ListaOreDisponibile));
            }
        }
        private ObservableCollection<string> getListaOreDisponibile()
        {
            if (_numeAsistentSelectat == "")
            {
                MessageBox.Show("Trebuie sa alegi un asistent inainte!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            ObservableCollection<string> oreAsist = ObtineOreDisponibile(_asistenti.Where(asistent => asistent.NumeComplet == _numeAsistentSelectat).First().Program);
            ObservableCollection<string> oreMedic = ObtineOreDisponibile(_medic.Program);

            ObservableCollection<string> ListaOre = new ObservableCollection<string>(oreMedic.Intersect(oreAsist).ToList());
            return ListaOre;
        }
        private ObservableCollection<string> ObtineOreDisponibile(string program)
        {
            program = program.Replace("Program: ", "").Trim();
            string[] ore = program.Split(new[] { " - " }, StringSplitOptions.None);
            TimeSpan oraInceput = TimeSpan.Parse(ore[0]);
            TimeSpan oraSfarsit = TimeSpan.Parse(ore[1]);

            ObservableCollection<string> oreDisponibile = new ObservableCollection<string>();

            for (TimeSpan ora = oraInceput; ora <= oraSfarsit; ora = ora.Add(TimeSpan.FromHours(1)))
            {
                oreDisponibile.Add(ora.ToString(@"hh\:mm"));
            }

            return oreDisponibile;
        }
    }
}
