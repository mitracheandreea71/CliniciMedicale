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
    
    public partial class Functie
    {
        public int id_functie { get; set; }
        public Nullable<int> id_angajat { get; set; }
        public Nullable<int> id_clinica { get; set; }
        public string nume_functie { get; set; }
        public Nullable<System.DateTime> data_incadrare { get; set; }
        public Nullable<System.DateTime> data_expirare_contract { get; set; }
        public string activ { get; set; }
    
        public virtual Angajat Angajat { get; set; }
        public virtual Clinica Clinica { get; set; }
    }
}
