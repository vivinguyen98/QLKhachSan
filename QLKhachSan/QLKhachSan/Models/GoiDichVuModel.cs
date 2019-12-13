using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSan.Models
{
    public class GoiDichVuModel
    {
        public IEnumerable<DICHVU> dsDichVu { get; set; }
        public IEnumerable<DICHVUDADAT> dsDvDaDat { get; set; }
    }
}