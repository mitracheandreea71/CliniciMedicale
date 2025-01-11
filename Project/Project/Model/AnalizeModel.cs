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

        private readonly CliniciDataContext _context;

        public AnalizeModel()
        { 
            _context = new CliniciDataContext();
        }

        public ObservableCollection<AnalizeModel> GetAllAnalizeForFormular(int formularID)
        {
            ObservableCollection<AnalizeModel> analizeRet = new ObservableCollection<AnalizeModel>();

            var analizeT = from analize in _context.Analizes
                           join ap_formular in _context.Apartenenta_Formulars on analize.id_analiza equals ap_formular.id_analiza
                           where ap_formular.id_formular == formularID
                           select analize;

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
            var tipuriAnalize = _context.Formular_Analizes.Select(formular => formular.tip_analize)
                                          .Distinct()
                                          .ToList();
            return new ObservableCollection<string>(tipuriAnalize);
        }
    }
}
