using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Project.Model
{
    internal class FormularAnalizeModel : INotifyPropertyChanged
    {
        public int IDFormular { get; set; }
        public string TipAnalize { get; set; }
        public decimal Pret { get; set; }
        public string Decontabile { get; set; }
        public string NumeFormular { get; set; }
        public string CaleImagine {  get; set; }
        public ObservableCollection<AnalizeModel> ListaAnalize { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CliniciDataContext _context;

        public FormularAnalizeModel()
        { 
            _context = new CliniciDataContext();
        }

        public ObservableCollection<FormularAnalizeModel> GetAllFormulare()
        {
            ObservableCollection<FormularAnalizeModel> formulare = new ObservableCollection<FormularAnalizeModel>();

            foreach (var formular in _context.Formular_Analizes)
            {
                formulare.Add(
                    new FormularAnalizeModel
                    {
                        IDFormular = formular.id_formular,
                        TipAnalize = formular.tip_analize,
                        Pret = (decimal)formular.pret,
                        Decontabile = $"Decontabile: {formular.decontabile}",
                        NumeFormular = formular.nume_formular,
                        CaleImagine = formular.cale_imagine
                    }
                    );
            }

            foreach (var formular in formulare)
            {
                formular.ListaAnalize = (new AnalizeModel()).GetAllAnalizeForFormular(formular.IDFormular);
            }

            return formulare;
        }
    }
}
