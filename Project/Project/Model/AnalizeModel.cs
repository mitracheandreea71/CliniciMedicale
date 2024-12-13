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
    internal class AnalizeModel : INotifyPropertyChanged
    {
        public int IDAnaliza {  get; set; }
        public string DenumireAnaliza {  set; get; }
        public string ValoriReferinta {  set; get; }
        public string UnitateMasura { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly CliniciDataContext _context;

        public AnalizeModel()
        { 
            _context = new CliniciDataContext();
        }

        public ObservableCollection<AnalizeModel> GetAllAnalizeForFormular(int formularID)
        {
            ObservableCollection<AnalizeModel> analizeRet = new ObservableCollection<AnalizeModel>();
            
            var analizeT = from analize in _context.Analizes
                          join apartenta_formular in _context.Apartenenta_Formulars on analize.id_analiza equals apartenta_formular.id_analiza
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
