//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLKhachSan.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DICHVU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DICHVU()
        {
            this.DICHVUDADATs = new HashSet<DICHVUDADAT>();
        }
    
        public int MADV { get; set; }
        public string TENDV { get; set; }
        public Nullable<decimal> GIA { get; set; }
        public string DVT { get; set; }
        public string ANH { get; set; }
        public Nullable<int> TONKHO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DICHVUDADAT> DICHVUDADATs { get; set; }
    }
}