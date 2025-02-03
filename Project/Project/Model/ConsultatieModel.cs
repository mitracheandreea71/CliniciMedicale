using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Linq;

namespace Project.Model
{
    public class ConsultatieModel
    {
        public int IdConsultatie { get; set; }
        public int? IdDoctor { get; set; }
        public int? IdAsistent { get; set; }
        public int? IdPacient { get; set; }
        public DateTime? DataConsultatie { get; set; }
        public string Ora { get; set; }
        public decimal? Pret { get; set; }
        public string Pacient { get; set;}
        public string Asistent { get; set; }

        public string Activ { set; get; }


        private readonly CliniciEntities _context;

        public ConsultatieModel()
        {
            _context = new CliniciEntities();
        }

        public string Data
        { 
            get => DataConsultatie.ToString();
        }
        public ConsultatieModel GetConsultatieByID(int idConsultatie)
        {
            var consultatieEntity = _context.Consultaties
                                           .FirstOrDefault(c => c.id_consultatie == idConsultatie);

            if (consultatieEntity == null)
                return null;

            var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient == consultatieEntity.id_pacient);
            var asistent = _context.Angajats.FirstOrDefault(a => a.id_angajat == consultatieEntity.id_asistent);

            ConsultatieModel consultatieModel = new ConsultatieModel
            {
                IdConsultatie = consultatieEntity.id_consultatie,
                IdDoctor = consultatieEntity.id_doctor,
                IdAsistent = consultatieEntity.id_asistent,
                IdPacient = consultatieEntity.id_pacient,
                DataConsultatie = consultatieEntity.data_consultatie,
                Ora = consultatieEntity.ora,
                Pret = consultatieEntity.pret,
                Pacient = $"{pacient.nume} {pacient.prenume}",
                Asistent = $"{asistent.nume} {asistent.prenume}"
            };

            return consultatieModel;
        }
        public void SaveChanges()
        {
            var consult = _context.Consultaties.Where(c => c.id_consultatie == IdConsultatie).FirstOrDefault();
            if (consult != null)
            {
                consult.pret = Pret;
                consult.ora = Ora;
                consult.data_consultatie = DataConsultatie;
                consult.id_asistent = IdAsistent;
            }
            _context.SaveChanges();
        }

        public ObservableCollection<ConsultatieModel> GetAllConsultatii()
        {
            var consultatii = _context.Consultaties.ToList();

            ObservableCollection<ConsultatieModel> consultatiiRet = new ObservableCollection<ConsultatieModel>();

            foreach (var iterator in consultatii)
            { 
                var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient == iterator.id_pacient);
                var asistent = _context.Angajats.FirstOrDefault(a => a.id_angajat == iterator.id_asistent);
                consultatiiRet.Add(
                    new ConsultatieModel
                    {
                        IdConsultatie = iterator.id_consultatie,
                        IdDoctor = iterator.id_doctor,
                        IdAsistent = iterator.id_asistent,
                        IdPacient = iterator.id_pacient,
                        DataConsultatie = iterator.data_consultatie,
                        Ora = iterator.ora,
                        Pret = iterator.pret,
                        Pacient = $"{pacient.nume} {pacient.prenume}",
                        Asistent = $"{asistent.nume} {asistent.prenume}"
                    }
                );
            }

            return consultatiiRet;
        }

        public void AdaugaConsultatie()
        {
            var consultatieEntity = new Consultatie
            {
                id_doctor = this.IdDoctor,
                id_asistent = this.IdAsistent,
                id_pacient = this.IdPacient,
                data_consultatie = this.DataConsultatie,
                ora = this.Ora,
            };
            _context.Consultaties.Add(consultatieEntity);
            _context.SaveChanges();


            this.IdConsultatie = consultatieEntity.id_consultatie;
        }

        
        public void ActualizeazaConsultatie()
        {
            var consultatieEntity = _context.Consultaties.FirstOrDefault(c => c.id_consultatie == this.IdConsultatie);
            if (consultatieEntity == null)
                throw new Exception("Consultatia nu a fost gasita.");

            consultatieEntity.id_doctor = this.IdDoctor;
            consultatieEntity.id_asistent = this.IdAsistent;
            consultatieEntity.id_pacient = this.IdPacient;
            consultatieEntity.data_consultatie = this.DataConsultatie;
            consultatieEntity.ora = this.Ora;
            consultatieEntity.pret = this.Pret;
            _context.SaveChanges();
        }
        public void StergeConsultatie(int idConsultatie)
        {
            var consultatieEntity = _context.Consultaties.FirstOrDefault(c => c.id_consultatie == idConsultatie);
            if (consultatieEntity == null)
                throw new Exception("Consultația nu a fost găsită.");

            _context.Consultaties.Remove(consultatieEntity);
            _context.SaveChanges();
        }
        public ObservableCollection<ConsultatieModel> GetAllConsultatiiForMedic(int medicID)
        {
            var consultatii = _context.Consultaties.Where(c=>c.id_doctor==medicID).ToList();

            ObservableCollection<ConsultatieModel> consultatiiRet = new ObservableCollection<ConsultatieModel>();

            foreach (var iterator in consultatii)
            {
                var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient == iterator.id_pacient);
                string numePacient = "";
                if (pacient == null)
                    numePacient = "Nu a fost atribuit!";
                else
                    numePacient = $"{pacient.nume} {pacient.prenume}";
                var asistent = _context.Angajats.FirstOrDefault(a => a.id_angajat == iterator.id_asistent);
                string numeAsist = "";
                if (asistent == null)
                    numeAsist = "Nu a fost atribuit!";
                else
                    numeAsist = $"{asistent.nume} {asistent.prenume}";

                consultatiiRet.Add(
                    new ConsultatieModel
                    {
                        IdConsultatie = iterator.id_consultatie,
                        IdDoctor = iterator.id_doctor,
                        IdAsistent = iterator.id_asistent,
                        IdPacient = iterator.id_pacient,
                        DataConsultatie = iterator.data_consultatie,
                        Ora = iterator.ora,
                        Pret = iterator.pret,
                        Pacient = numePacient,
                        Asistent = numeAsist
                    }
                );
            }

            return consultatiiRet;
        }

        public ObservableCollection<ConsultatieModel> GetAllConsultatiiForAsistent(int idAsistent)
        {
            var consultatii = _context.Consultaties.Where(c => c.id_asistent == idAsistent).ToList();

            ObservableCollection<ConsultatieModel> consultatiiRet = new ObservableCollection<ConsultatieModel>();

            foreach (var iterator in consultatii)
            {
                var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient == iterator.id_pacient);
                string numePacient = "";
                if (pacient == null)
                    numePacient = "Nu a fost atribuit!";
                else
                    numePacient = $"{pacient.nume} {pacient.prenume}";
                var asistent = _context.Angajats.FirstOrDefault(a => a.id_angajat == iterator.id_asistent);
                string numeAsist = "";
                if (asistent == null)
                    numeAsist = "Nu a fost atribuit!";
                else
                    numeAsist = $"{asistent.nume} {asistent.prenume}";

                consultatiiRet.Add(
                    new ConsultatieModel
                    {
                        IdConsultatie = iterator.id_consultatie,
                        IdDoctor = iterator.id_doctor,
                        IdAsistent = iterator.id_asistent,
                        IdPacient = iterator.id_pacient,
                        DataConsultatie = iterator.data_consultatie,
                        Ora = iterator.ora,
                        Pret = iterator.pret,
                        Pacient = numePacient,
                        Asistent = numeAsist
                    }
                );
            }

            return consultatiiRet;
        }

        public List<ConsultatieModel> GetConsultatiiByPacientID(int idPacient)
        {
            var consultatii = _context.Consultaties
                .Where(c => c.id_pacient == idPacient)
                .ToList();

            List<ConsultatieModel> consultatiiList = new List<ConsultatieModel>();

            foreach (var iterator in consultatii)
            {
                consultatiiList.Add(new ConsultatieModel
                {
                    IdConsultatie = iterator.id_consultatie,
                    IdDoctor = iterator.id_doctor,
                    IdAsistent = iterator.id_asistent,
                    IdPacient = iterator.id_pacient,
                    DataConsultatie = iterator.data_consultatie,
                    Ora = iterator.ora,
                    Pret = iterator.pret
                });
            }

            return consultatiiList;
        }

        public List<string> GetOreProgramate(int medicID, DateTime data)
        {
            return _context.Consultaties
                .Where(c => c.id_doctor == medicID &&
                            c.data_consultatie.HasValue &&
                            DbFunctions.TruncateTime(c.data_consultatie) == data.Date)
                .Select(c => c.ora)
                .ToList();
        }

        public void AdaugaConsultatie(int medicID, int pacientID, DateTime dataConsultatie, string ora)
        {
            var consultatie = new Consultatie
            {
                id_doctor = medicID,
                id_pacient = pacientID,
                data_consultatie = dataConsultatie,
                ora = ora
            };

            _context.Consultaties.Add(consultatie);
            _context.SaveChanges();
        }



    }
}
