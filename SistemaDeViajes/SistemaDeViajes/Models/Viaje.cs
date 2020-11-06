//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaDeViajes.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Viaje
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Viaje()
        {
            this.Asiento = new HashSet<Asiento>();
            this.DETALLEVENTA = new HashSet<DETALLEVENTA>();
        }
    
        public int IIDVIAJE { get; set; }
        public Nullable<int> IIDLUGARORIGEN { get; set; }
        public Nullable<int> IIDLUGARDESTINO { get; set; }
        public Nullable<decimal> PRECIO { get; set; }
        public Nullable<System.DateTime> FECHAVIAJE { get; set; }
        public Nullable<int> IIDBUS { get; set; }
        public Nullable<int> NUMEROASIENTOSDISPONIBLES { get; set; }
        public Nullable<int> BHABILITADO { get; set; }
        public byte[] FOTO { get; set; }
        public string nombrefoto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asiento> Asiento { get; set; }
        public virtual Bus Bus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETALLEVENTA> DETALLEVENTA { get; set; }
        public virtual Lugar Lugar { get; set; }
        public virtual Lugar Lugar1 { get; set; }
    }
}
