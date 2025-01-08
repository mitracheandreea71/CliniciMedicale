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
    internal class FormularAnalizeModel
    {
        public int IDFormular { get; set; }
        public string TipAnalize { get; set; }
        public decimal Pret { get; set; }
        public string Decontabile { get; set; }
        public string NumeFormular { get; set; }
        public string CaleImagine {  get; set; }
        public ObservableCollection<AnalizeModel> ListaAnalize { get; set; }

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

        public FormularAnalizeModel GetFormularByBuletinAnalizeID(int buletinAnalizeID)
        {
            FormularAnalizeModel formular = new FormularAnalizeModel();

            var f = (from form in _context.Formular_Analizes
                     join buletin in _context.Buletin_Analizes on form.id_formular equals buletin.id_formular_analize
                     select form).First();
            formular.IDFormular = f.id_formular;
            formular.TipAnalize = f.tip_analize;
            formular.Pret = (decimal)f.pret;
            formular.Decontabile = f.decontabile;
            formular.NumeFormular = f.nume_formular;
            formular.CaleImagine = f.cale_imagine;
            formular.ListaAnalize = (new AnalizeModel()).GetAllAnalizeForFormular(formular.IDFormular);

            return formular;
        }
    }
}
