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
    internal class ConsultatieModel
    {
        public int IdConsultatie { get; set; }
        public int? IdDoctor { get; set; }
        public int? IdAsistent { get; set; }
        public int? IdPacient { get; set; }
        public DateTime? DataConsultatie { get; set; }
        public string Ora { get; set; }
        public decimal? Pret { get; set; }


        private readonly CliniciDataContext _context;

        public ConsultatieModel()
        {
            _context = new CliniciDataContext();
        }


        public ConsultatieModel GetConsultatieByID(int idConsultatie)
        {
            var consultatieEntity = _context.Consultaties
                                           .FirstOrDefault(c => c.id_consultatie == idConsultatie);

            if (consultatieEntity == null)
                return null;

            ConsultatieModel consultatieModel = new ConsultatieModel
            {
                IdConsultatie = consultatieEntity.id_consultatie,
                IdDoctor = consultatieEntity.id_doctor,
                IdAsistent = consultatieEntity.id_asistent,
                IdPacient = consultatieEntity.id_pacient,
                DataConsultatie = consultatieEntity.data_consultatie,
                Ora = consultatieEntity.ora,
                Pret = consultatieEntity.pret
            };

            return consultatieModel;
        }
        public ObservableCollection<ConsultatieModel> GetAllConsultatii()
        {
            var consultatii = _context.Consultaties.ToList();

            ObservableCollection<ConsultatieModel> consultatiiRet = new ObservableCollection<ConsultatieModel>();

            foreach (var iterator in consultatii)
            {
                consultatiiRet.Add(
                    new ConsultatieModel
                    {
                        IdConsultatie = iterator.id_consultatie,
                        IdDoctor = iterator.id_doctor,
                        IdAsistent = iterator.id_asistent,
                        IdPacient = iterator.id_pacient,
                        DataConsultatie = iterator.data_consultatie,
                        Ora = iterator.ora,
                        Pret = iterator.pret
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
            _context.Consultaties.InsertOnSubmit(consultatieEntity);
            _context.SubmitChanges();


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
            _context.SubmitChanges();
        }
        public void StergeConsultatie(int idConsultatie)
        {
            var consultatieEntity = _context.Consultaties.FirstOrDefault(c => c.id_consultatie == idConsultatie);
            if (consultatieEntity == null)
                throw new Exception("Consultația nu a fost găsită.");

            _context.Consultaties.DeleteOnSubmit(consultatieEntity);
            _context.SubmitChanges();
        }
    }
}
