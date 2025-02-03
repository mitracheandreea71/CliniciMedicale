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
    public class AnalizeModel
    {
        public int IDAnaliza {  get; set; }
        public string DenumireAnaliza {  set; get; }
        public string ValoriReferinta {  set; get; }
        public string UnitateMasura { set; get; }

        private readonly CliniciEntities _context;

        public AnalizeModel()
        { 
            _context = new CliniciEntities();
        }

        public ObservableCollection<AnalizeModel> GetAllAnalizeForFormular(int formularID)
        {
            ObservableCollection<AnalizeModel> analizeRet = new ObservableCollection<AnalizeModel>();

            var formular = _context.Formular_Analize.Where(t => t.id_formular == formularID).ToList().First();

            var analizeT = formular.Analizes;

            foreach (var iterator in analizeT)
            {
                analizeRet.Add(
                    new AnalizeModel
                    {
                        IDAnaliza = iterator.id_analiza,
                        DenumireAnaliza = iterator.denumire_analiza,
                        ValoriReferinta = iterator.valori_referinta,
                        UnitateMasura = iterator.unitate_masura
                    }
                    );
            }

            return analizeRet;
        }
        public ObservableCollection<string> GetAllAnalizeType() 
        {
            var tipuriAnalize = _context.Formular_Analize.Select(formular => formular.tip_analize)
                                          .Distinct()
                                          .ToList();
            return new ObservableCollection<string>(tipuriAnalize);
        }
    }
}
