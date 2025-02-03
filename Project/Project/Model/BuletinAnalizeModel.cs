using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class BuletinAnalizeModel
    {
        public int IDBuletin { get; set; }
        public DateTime DataRecoltare { get; set; }
        public PacientModel Pacient { get; set; }
        public ObservableCollection<RezultatAnalizeModel> RezultateAnalize { get; set; }
        public FormularAnalizeModel FormularAnalize { get; set; }
        public string NumeSefLab { get; set; }

        private CliniciEntities _context;

        public BuletinAnalizeModel()
        {
            _context = new CliniciEntities();
        }

        public string Data
        {
            get => DataRecoltare.ToString("yyyy-MM-dd");
        }
        public string NumePacient
        {
            get => $"{Pacient.Nume} {Pacient.Prenume}";
        }
        public string Ora
        {
            get => DataRecoltare.ToString("HH:mm");
        }
        public ObservableCollection<BuletinAnalizeModel> GetBuletineAnalizaByClinicaId(int idClinica)
        {
            ObservableCollection<BuletinAnalizeModel> buletineAnalize=new ObservableCollection<BuletinAnalizeModel>();
            var id_sefilab = from clinica in _context.Clinicas
                             join functie in _context.Functies on clinica.id_clinica equals functie.id_clinica
                             where functie.nume_functie == "Sef Lab" && clinica.id_clinica == idClinica
                             select functie.id_angajat;

            foreach (var id in id_sefilab)
            {
                var buletine = _context.Buletin_Analize.Where(ba => ba.id_seflab == id);
                foreach (var buletin in buletine)
                {
                    BuletinAnalizeModel buletinNou = new BuletinAnalizeModel
                    {
                        IDBuletin = buletin.id_buletin,
                        DataRecoltare = (DateTime)buletin.data_recoltare,
                        Pacient = (new PacientModel()).GetPacientByID((int)buletin.id_pacient),
                        FormularAnalize = (new FormularAnalizeModel()).GetFormularByBuletinAnalizeID(buletin.id_buletin),
                        RezultateAnalize = (new RezultatAnalizeModel()).GetRezultateForBuletin(buletin.id_buletin)
                    };
                    var sefLab = _context.Angajats.FirstOrDefault(angajat => angajat.id_angajat == buletin.id_seflab);
                    if (sefLab != null)
                    {
                        buletinNou.NumeSefLab = $"{sefLab.titulatura} {sefLab.nume} {sefLab.prenume}";
                    }
                    buletineAnalize.Add(buletinNou);
                }
            }
            return buletineAnalize;
        }
        public BuletinAnalizeModel GetBuletinAnalizeByID(int idBuletin,int idpacient)
        { 
            BuletinAnalizeModel buletinAnalize = new BuletinAnalizeModel ();

            var buletine= _context.Buletin_Analize.Where(ba => ba.id_buletin == idBuletin && ba.id_pacient == idpacient);

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

        public ObservableCollection<BuletinAnalizeModel> GetBuletineByMedicID(int medicId)
        {
            var buletine = _context.Buletin_Analize
                .Where(ba => ba.id_seflab == medicId)
                .ToList();

            ObservableCollection<BuletinAnalizeModel> buletineAnalizeList = new ObservableCollection<BuletinAnalizeModel>();

            foreach (var buletin in buletine)
            {
                var buletinAnalize = new BuletinAnalizeModel
                {
                    IDBuletin = buletin.id_buletin,
                    DataRecoltare = (DateTime)buletin.data_recoltare,
                    Pacient = (new PacientModel()).GetPacientByID((int)buletin.id_pacient),
                    FormularAnalize = (new FormularAnalizeModel()).GetFormularByBuletinAnalizeID(buletin.id_buletin),
                    RezultateAnalize = (new RezultatAnalizeModel()).GetRezultateForBuletin(buletin.id_buletin)
                };

                var sefLab = _context.Angajats.FirstOrDefault(angajat => angajat.id_angajat == buletin.id_seflab);
                if (sefLab != null)
                {
                    buletinAnalize.NumeSefLab = $"{sefLab.titulatura} {sefLab.nume} {sefLab.prenume}";
                }

                buletineAnalizeList.Add(buletinAnalize);
            }

            return buletineAnalizeList;
        }

        public List<BuletinAnalizeModel> GetBuletineByPacientID(int idPacient)
        {
            var buletine = _context.Buletin_Analize
                .Where(ba => ba.id_pacient == idPacient)
                .ToList();

            List<BuletinAnalizeModel> buletineAnalizeList = new List<BuletinAnalizeModel>();

            foreach (var buletin in buletine)
            {
                var buletinAnalize = new BuletinAnalizeModel
                {
                    IDBuletin = buletin.id_buletin,
                    DataRecoltare = (DateTime)buletin.data_recoltare,
                    Pacient = (new PacientModel()).GetPacientByID((int)buletin.id_pacient),
                    FormularAnalize = (new FormularAnalizeModel()).GetFormularByBuletinAnalizeID(buletin.id_buletin),
                    RezultateAnalize = (new RezultatAnalizeModel()).GetRezultateForBuletin(buletin.id_buletin)
                };

                var sefLab = _context.Angajats.FirstOrDefault(angajat => angajat.id_angajat == buletin.id_seflab);
                if (sefLab != null)
                {
                    buletinAnalize.NumeSefLab = $"{sefLab.titulatura} {sefLab.nume} {sefLab.prenume}";
                }

                buletineAnalizeList.Add(buletinAnalize);
            }

            return buletineAnalizeList;
        }

    }
}
