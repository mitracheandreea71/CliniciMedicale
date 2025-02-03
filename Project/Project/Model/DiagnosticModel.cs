#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    internal class DiagnosticModel
    {
        private int IdDiagnostic;
        public int? IdConsultatie { get; set; }
        public string? Diagnostic { get; set; }
        public string? Observatii { get;set;}
        public string? Tratament { get;set;}

        public string Activ { set; get; }

        private readonly CliniciEntities _context;
        public DiagnosticModel()
        {
            _context = new CliniciEntities();
        }

        public DiagnosticModel? GetDiagnosticForConsultatie(int idConsultatie)
        {
            var diag = _context.Diagnostics.Where(d => d.id_consultatie == idConsultatie).FirstOrDefault();
            if (diag == null)
                return null;
            DiagnosticModel diagRet = new DiagnosticModel
            {
                IdConsultatie = diag.id_consultatie,
                Diagnostic = diag.diagnostic1,
                Observatii = diag.observatii,
                Tratament = diag.tratament,
                IdDiagnostic = diag.id_diagnostic
            };
            return diagRet;
        }
        public void SaveDiagnostic()
        {
            var diagFromDB = _context.Diagnostics.Where(d => d.id_diagnostic == IdDiagnostic).FirstOrDefault();
            if (diagFromDB != null)
            {
                diagFromDB.diagnostic1 = Diagnostic;
                diagFromDB.observatii = Observatii;
                diagFromDB.tratament = Tratament;
                diagFromDB.id_consultatie = IdConsultatie;

                _context.SaveChanges();
                return;
            }
            var diagInDB = new Diagnostic()
            {
                id_consultatie = IdConsultatie,
                diagnostic1 = Diagnostic,
                observatii = Observatii,
                tratament = Tratament
            };
            _context.Diagnostics.Add(diagInDB);
            _context.SaveChanges();
        }

    }
}
