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
    
    public partial class Formular_Analize
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Formular_Analize()
        {
            this.Buletin_Analize = new HashSet<Buletin_Analize>();
            this.Analizes = new HashSet<Analize>();
        }
    
        public int id_formular { get; set; }
        public string tip_analize { get; set; }
        public Nullable<decimal> pret { get; set; }
        public string decontabile { get; set; }
        public string nume_formular { get; set; }
        public string cale_imagine { get; set; }
        public string activ { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Buletin_Analize> Buletin_Analize { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Analize> Analizes { get; set; }
    }
}
