using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachsan.Controllers
{
    public class BangGiaController : Controller
    {
        // GET: BangGia
        QLKhachSan.Models.WEBSITEEntities db = new QLKhachSan.Models.WEBSITEEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GiaPhong()
        {
            return View(db.LOAIPHONGs.ToList());
        }
        public ActionResult GiaDichVu()
        {
            return View(db.DICHVUs.ToList());
        }
    }
}