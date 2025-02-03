using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    internal class IncadrareDepartamentModel
    {
        public int id_incadrare { get; set; }
        public int? id_departament { get; set; }
        public int? id_angajat { get; set; }
        public string intrare_tura { get; set; }
        public string iesire_tura { get; set; }

        private readonly CliniciEntities _context;

        public IncadrareDepartamentModel()
        {
            _context = new CliniciEntities();
        }

        public IncadrareDepartamentModel GetByIdDepartament(int idIncadrare)
        {
            var entity = _context.Incadrare_Departament
                                  .FirstOrDefault(i => i.id_incadrare == idIncadrare);

            if (entity == null)
                return null;

            return new IncadrareDepartamentModel
            {
                id_incadrare = entity.id_incadrare,
                id_departament = entity.id_departament,
                id_angajat = entity.id_angajat,
                intrare_tura = entity.intrare_tura,
                iesire_tura = entity.iesire_tura
            };
        }

        public IncadrareDepartamentModel GetByIdMedic(int idMedic)
        {
            var entity = _context.Incadrare_Departament
                                  .FirstOrDefault(i => i.id_angajat == idMedic);

            if (entity == null)
                return null;

            return new IncadrareDepartamentModel
            {
                id_incadrare = entity.id_incadrare,
                id_departament = entity.id_departament,
                id_angajat = entity.id_angajat,
                intrare_tura = entity.intrare_tura,
                iesire_tura = entity.iesire_tura
            };
        }

        public ObservableCollection<IncadrareDepartamentModel> GetAll()
        {
            var entities = _context.Incadrare_Departament.ToList();

            ObservableCollection<IncadrareDepartamentModel> ret = new ObservableCollection<IncadrareDepartamentModel>();

            foreach (var item in entities)
            {
                ret.Add(new IncadrareDepartamentModel
                {
                    id_incadrare = item.id_incadrare,
                    id_departament = item.id_departament,
                    id_angajat = item.id_angajat,
                    intrare_tura = item.intrare_tura,
                    iesire_tura = item.iesire_tura
                    
                });
            }

            return ret;
        }

        public void AdaugaIncadrare()
        {
            var entity = new Incadrare_Departament
            {
                id_departament = this.id_departament,
                id_angajat = this.id_angajat,
                intrare_tura = this.intrare_tura,
                iesire_tura = this.iesire_tura
                // Setează alte proprietăți dacă este necesar
            };

            _context.Incadrare_Departament.Add(entity);
            _context.SaveChanges();

            this.id_incadrare = entity.id_incadrare; 
        }

        public void ActualizeazaIncadrare()
        {
            var entity = _context.Incadrare_Departament
                                  .FirstOrDefault(i => i.id_incadrare == this.id_incadrare);

            if (entity == null)
                throw new Exception("Înregistrarea nu a fost găsită.");

            entity.id_departament = this.id_departament;
            entity.id_angajat = this.id_angajat;
            entity.intrare_tura = this.intrare_tura;
            entity.iesire_tura = this.iesire_tura;
           
            _context.SaveChanges();
        }

        public void StergeIncadrare(int idIncadrare)
        {
            var entity = _context.Incadrare_Departament
                                  .FirstOrDefault(i => i.id_incadrare == idIncadrare);

            if (entity == null)
                throw new Exception("Înregistrarea nu a fost găsită.");

            _context.Incadrare_Departament.Remove(entity);
            _context.SaveChanges();
        }

    }
}
