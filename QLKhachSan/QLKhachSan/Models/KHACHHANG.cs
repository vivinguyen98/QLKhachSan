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
    
    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            this.PHIEUDATPHONGs = new HashSet<PHIEUDATPHONG>();
        }
    
        public int MAKH { get; set; }
        public string HOTEN { get; set; }
        public string EMAIL { get; set; }
        public string MATKHAU { get; set; }
        public string CMT { get; set; }
        public string SDT { get; set; }
        public Nullable<decimal> DIEM { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PHIEUDATPHONG> PHIEUDATPHONGs { get; set; }
    }
}
