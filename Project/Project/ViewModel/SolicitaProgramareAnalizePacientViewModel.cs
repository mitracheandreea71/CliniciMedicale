using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PdfSharp.Charting;
using Project.Commands;
using Project.Model;


namespace Project.ViewModel
{
    internal class SolicitaProgramareAnalizePacientViewModel : BaseViewModel
    {
        public string NumeFormular { get; }
        public string NumeClinica { get; }

        private DateTime? _dataProgramare;

        private string _cnp;
        private string _nume;
        private string _prenume;
        private string _judet;
        private string _adresa;

        private readonly CliniciDataContext _context;
        private readonly ClinicaModel _clinicaModel;
        private readonly FormularAnalizeModel _formularModel;
        public ICommand TrimiteProgramareCommand { get; }
        public SolicitaProgramareAnalizePacientViewModel(string numeFormular, string numeClinica)
        {
            NumeFormular = numeFormular;
            NumeClinica = numeClinica;
            _clinicaModel = new ClinicaModel();
            _context = new CliniciDataContext();
            _formularModel = new FormularAnalizeModel();
            TrimiteProgramareCommand = new BaseCommand(ExecuteTrimiteProgramare, CanExecuteTrimiteProgramare);
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

        public string CNP
        {
            get => _cnp;
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(CNP));
            }
        }

        public string Nume
        {
            get => _nume;
            set
            {
                _nume = value;
                OnPropertyChanged(nameof(Nume));
            }
        }

        public string Prenume
        {
            get => _prenume;
            set
            {
                _prenume = value;
                OnPropertyChanged(nameof(Prenume));
            }
        }
        public string Judet
        {
            get => _judet;
            set
            {
                _judet = value;
                OnPropertyChanged(nameof(Judet));
            }
        }
        public string Adresa
        {
            get => _adresa;
            set
            {
                _adresa = value;
                OnPropertyChanged(nameof(Adresa));
            }
        }

        private void ExecuteTrimiteProgramare(object parameter)
        {
            if (string.IsNullOrWhiteSpace(CNP) ||
                string.IsNullOrWhiteSpace(Nume) ||
                string.IsNullOrWhiteSpace(Prenume) ||
                string.IsNullOrWhiteSpace(Judet) ||
                string.IsNullOrWhiteSpace(Adresa) ||
                !DataProgramare.HasValue)
            {
                MessageBox.Show("Te rugăm să completezi toate câmpurile.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                //verifica existenta pacient daca nu exista este adaugat in tabela Pacient
                
                var pacientExistent = _context.Pacients.FirstOrDefault(p => p.id_pacient.ToString() == CNP);
                int pacientID;
                if (pacientExistent == null)
                {
                    var pacientNou = new Pacient
                    {
                        id_pacient = int.Parse(CNP),
                        nume = Nume,
                        prenume = Prenume,
                        judet = Judet,
                        adresa = Adresa
                    };

                    _context.Pacients.InsertOnSubmit(pacientNou);
                    _context.SubmitChanges();

                    pacientID = pacientNou.id_pacient;
                }
                else
                {
                    pacientID = pacientExistent.id_pacient;
                }

                int idFormular = _formularModel.GetIdByName(NumeFormular);
                int idClinica= _clinicaModel.GetIdByName(NumeClinica);
                var sefLab = _context.Functies.FirstOrDefault(s => s.nume_functie == "Sef Lab" && s.id_clinica == idClinica);

                int idSefLab = (int)sefLab.id_angajat;

                TimeSpan hour = new TimeSpan(7, 0, 0);
                DataProgramare = DataProgramare + hour;

                var buletinAnalizeNou = new Buletin_Analize
                {
                    id_formular_analize =idFormular,
                    id_pacient= int.Parse(CNP),
                    data_recoltare=DataProgramare,
                    id_seflab=idSefLab
                };

                _context.Buletin_Analizes.InsertOnSubmit(buletinAnalizeNou);
                _context.SubmitChanges();

                if (parameter is Window window)
                {
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la trimiterea programării: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteTrimiteProgramare(object parameter)
        {
            return true;
        }


    }
}
