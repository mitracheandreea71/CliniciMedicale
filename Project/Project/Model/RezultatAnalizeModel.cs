using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Project.Model
{
    internal class RezultatAnalizeModel
    {
        public AnalizeModel TipAnalize { get; set; }

        public string ValoriRezultate { get; set; }

        public CliniciDataContext _context;

        public RezultatAnalizeModel()
        {
            TipAnalize = null;
            ValoriRezultate = null;
            _context = new CliniciDataContext();
        }

        public ObservableCollection<RezultatAnalizeModel> GetRezultateForBuletin(int buletinID)
        {
            ObservableCollection<RezultatAnalizeModel> rezultate = new ObservableCollection<RezultatAnalizeModel>();

            var rez = _context.Rezultat_Analizes.Where(ra => ra.id_buletin_analize == buletinID).ToList();

            foreach (var iterator in rez)
            {
                RezultatAnalizeModel rezultat = new RezultatAnalizeModel();
                rezultat.ValoriRezultate = iterator.valori_rezultate;

                var analiza = _context.Analizes.Where(an => an.id_analiza == iterator.id_analiza).First();
                rezultat.TipAnalize = new AnalizeModel();
                rezultat.TipAnalize.IDAnaliza = analiza.id_analiza;
                rezultat.TipAnalize.DenumireAnaliza = analiza.denumire_analiza;
                rezultat.TipAnalize.ValoriReferinta = analiza.valori_referinta;
                rezultat.TipAnalize.UnitateMasura = analiza.unitate_masura;
                rezultate.Add(rezultat);
            }

            return rezultate;
        }
    }
}
