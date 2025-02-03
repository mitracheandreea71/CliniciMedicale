//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Project
{
    using System;
    using System.Collections.Generic;
    
    public partial class Angajat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Angajat()
        {
            this.Incadrare_Departament = new HashSet<Incadrare_Departament>();
            this.Functies = new HashSet<Functie>();
            this.Consultaties = new HashSet<Consultatie>();
            this.Consultaties1 = new HashSet<Consultatie>();
            this.Buletin_Analize = new HashSet<Buletin_Analize>();
        }
    
        public int id_angajat { get; set; }
        public string nume { get; set; }
        public string prenume { get; set; }
        public string username { get; set; }
        public string parola { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string specialitate { get; set; }
        public Nullable<decimal> rating { get; set; }
        public string imagine_cale { get; set; }
        public string titulatura { get; set; }
        public string activ { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Incadrare_Departament> Incadrare_Departament { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Functie> Functies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Consultatie> Consultaties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Consultatie> Consultaties1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Buletin_Analize> Buletin_Analize { get; set; }
    }
}
