using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Project.Model
{
    public class RezultatAnalizeModel : INotifyPropertyChanged
    {
        public int IdRezultat { get; set; }
        public AnalizeModel TipAnalize { get; set; }

        public string _valoriRezultate { get; set; }

        public CliniciDataContext _context;

        public RezultatAnalizeModel()
        {
            TipAnalize = null;
            ValoriRezultate = null;
            _context = new CliniciDataContext();
        }

        public string ValoriRezultate
        {
            get => _valoriRezultate;
            set
            {
                if (_valoriRezultate != value)
                {
                    _valoriRezultate = value;
                    OnPropertyChanged(nameof(ValoriRezultate));
                    SaveChanges();
                }
            }
        }
        private void SaveChanges()
        {
            try
            {
                var rezultatInDB = _context.Rezultat_Analizes.FirstOrDefault(r => r.id_rezultat == IdRezultat);
                if (rezultatInDB != null)
                {
                    rezultatInDB.valori_rezultate = ValoriRezultate;
                    _context.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea modificarii: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                rezultat.IdRezultat = iterator.id_rezultat;
                rezultate.Add(rezultat);
            }

            return rezultate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
