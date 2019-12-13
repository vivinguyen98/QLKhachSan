using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;

namespace QLKhachsan.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(db.KHACHHANGs.ToList());
        }

        // GET: KhachHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG tblKhachHang = db.KHACHHANGs.Find(id);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG tblKhachHang = db.KHACHHANGs.Find(id);
            if (tblKhachHang == null)
            {
                return HttpNotFound();
            }
            return View(tblKhachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAKH,HOTEN,EMAIL,MATKHAU,CMT,SDT,DIEM")] KHACHHANG tblKhachHang)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;

                if (tblKhachHang.SDT.Length == 10 && (tblKhachHang.CMT.Length == 9 || tblKhachHang.CMT.Length == 12))
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Nhập sai định dạng số điện thoại hoặc chứng minh thư !");
                }

            }
            return View(tblKhachHang);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
