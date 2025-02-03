
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
    public class FormularAnalizeModel
    {
        public int IDFormular { get; set; }
        public string TipAnalize { get; set; }
        public decimal Pret { get; set; }
        public string Decontabile { get; set; }
        public string NumeFormular { get; set; }
        public string CaleImagine {  get; set; }
        public ObservableCollection<AnalizeModel> ListaAnalize { get; set; }

        private readonly CliniciEntities _context;

        public FormularAnalizeModel()
        { 
            _context = new CliniciEntities();
        }

        public ObservableCollection<FormularAnalizeModel> GetAllFormulare()
        {
            ObservableCollection<FormularAnalizeModel> formulare = new ObservableCollection<FormularAnalizeModel>();

            foreach (var formular in _context.Formular_Analize)
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

            var f = (from form in _context.Formular_Analize
                     join buletin in _context.Buletin_Analize on form.id_formular equals buletin.id_formular_analize
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

        public int GetIdByName(string name)
        {
            var formular = _context.Formular_Analize.FirstOrDefault(f => f.nume_formular == name);
            return formular?.id_formular ?? -1;
        }
    }
}
