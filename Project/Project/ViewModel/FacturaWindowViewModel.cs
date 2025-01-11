using Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class FacturaWindowViewModel<T> : BaseViewModel
    {
        private ClinicaModel _clinicaInfo;
        private T Model;
        private string _pacientInfo;
        private string _numarFactura;
        private DateTime _dataFactura;
        private ObservableCollection<FacturaModel> _serviciiList;
        private decimal _totalGeneral;
        private decimal _totalCuTVA;

        public FacturaWindowViewModel(T model)
        {
            Model = model;
            InitializeData();
        }

        private void InitializeData()
        {
            var clinicaModel = new ClinicaModel();
            decimal pret = 0;
            string denumire = "";
            if (Model is ConsultatieModel m)
            {
                ClinicaInfo = clinicaModel.GetClinicaByMedicId((int)m.IdDoctor);
                NumarFactura = $"F{DateTime.Now:yyyyMMdd}{m.IdConsultatie}";
                pret = m.Pret ?? 0;
                if (new AsigurareModel().isPacientInsured((int)m.IdPacient))
                    pret = 0;
                PacientInfo = m.Pacient;
                denumire = "Consultatie medicala";
            }
            if (Model is BuletinAnalizeModel b)
            {
                ClinicaInfo = clinicaModel.GetClinicaByMedicId((int)new MediciModel().GetMedicIdByNumeCompletFaraTitulatura(b.NumeSefLab));
                NumarFactura = $"F{DateTime.Now:yyyyMMdd}{b.DataRecoltare}";
                pret = b.FormularAnalize.Pret;
                if (new AsigurareModel().isPacientInsured(b.Pacient.IDPacient))
                    pret = 0;
                PacientInfo = $"{b.Pacient.Nume} {b.Pacient.Prenume}";
                denumire = "Analize medicale";
            }

            DataFactura = DateTime.Now;

            ServiciiList = new ObservableCollection<FacturaModel>
            {
                new FacturaModel
                {
                    Index = 1,
                    Denumire = denumire,
                    UM = "buc",
                    Cantitate = 1,
                    PretUnitar = pret,
                    TVAProcent = 19
                }
            };
            CalculateTotals();
        }

        private void CalculateTotals()
        {
            decimal total = 0;
            decimal totalTVA = 0;

            foreach (var serviciu in ServiciiList)
            {
                total += serviciu.Valoare;
                totalTVA += serviciu.ValoareTVA;
            }

            TotalGeneral = total;
            TotalCuTVA = total;
        }
        public ClinicaModel ClinicaInfo
        {
            get => _clinicaInfo;
            set
            {
                _clinicaInfo = value;
                OnPropertyChanged(nameof(ClinicaInfo));
            }
        }

        public string PacientInfo
        {
            get => _pacientInfo;
            set
            {
                _pacientInfo = value;
                OnPropertyChanged(nameof(PacientInfo));
            }
        }

        public string NumarFactura
        {
            get => _numarFactura;
            set
            {
                _numarFactura = value;
                OnPropertyChanged(nameof(NumarFactura));
            }
        }

        public DateTime DataFactura
        {
            get => _dataFactura;
            set
            {
                _dataFactura = value;
                OnPropertyChanged(nameof(DataFactura));
            }
        }

        public ObservableCollection<FacturaModel> ServiciiList
        {
            get => _serviciiList;
            set
            {
                _serviciiList = value;
                OnPropertyChanged(nameof(ServiciiList));
            }
        }

        public decimal TotalGeneral
        {
            get => _totalGeneral;
            set
            {
                _totalGeneral = value;
                OnPropertyChanged(nameof(TotalGeneral));
            }
        }

        public decimal TotalCuTVA
        {
            get => _totalCuTVA;
            set
            {
                _totalCuTVA = value;
                OnPropertyChanged(nameof(TotalCuTVA));
            }
        }
    }
}
