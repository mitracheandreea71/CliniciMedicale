using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    internal class BuletinAnalizeModel
    {
        public int IDBuletin { get; set; }
        public DateTime DataRecoltare { get; set; }
        public PacientModel Pacient { get; set; }
        public ObservableCollection<RezultatAnalizeModel> RezultateAnalize { get; set; }
        public FormularAnalizeModel FormularAnalize { get; set; }
        public string NumeSefLab { get; set; }

        private CliniciDataContext _context;

        public BuletinAnalizeModel()
        {
            _context = new CliniciDataContext();
        }

        public BuletinAnalizeModel GetBuletinAnalizeByID(int idBuletin,int idpacient)
        { 
            BuletinAnalizeModel buletinAnalize = new BuletinAnalizeModel ();

            var buletine= _context.Buletin_Analizes.Where(ba => ba.id_buletin == idBuletin && ba.id_pacient == idpacient);

            if (!buletine.Any())
                return null;
            
            var buletin = buletine.First();

            buletinAnalize.IDBuletin = buletin.id_buletin;
            buletinAnalize.DataRecoltare = (DateTime)buletin.data_recoltare;
            buletinAnalize.Pacient = (new PacientModel()).GetPacientByID((int)buletin.id_pacient);
            buletinAnalize.FormularAnalize = (new FormularAnalizeModel()).GetFormularByBuletinAnalizeID(buletinAnalize.IDBuletin);
            buletinAnalize.RezultateAnalize = (new RezultatAnalizeModel()).GetRezultateForBuletin(buletinAnalize.IDBuletin);
            var sefLab = _context.Angajats.Where(angajat => angajat.id_angajat == buletin.id_seflab).First();
            buletinAnalize.NumeSefLab = $"{sefLab.titulatura} {sefLab.nume} {sefLab.prenume}";
            return buletinAnalize;
        }

    }
}
