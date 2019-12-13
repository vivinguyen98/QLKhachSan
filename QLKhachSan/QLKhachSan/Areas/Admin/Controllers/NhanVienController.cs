using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;

namespace QLKhachSan.Areas.Admin.Controllers.Admin
{
    public class NhanVienController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();

        // GET: NhanVien
        public ActionResult Index()
        {
            // var tblNhanViens = db.NHANVIENs.Include(t => t.CV);
            return View(db.NHANVIENs.ToList());
        }

        // GET: NhanVien/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN tblNhanVien = db.NHANVIENs.Find(id);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tblNhanVien);
        }

        // GET: NhanVien/Create
        public ActionResult Create()
        {
            // ViewBag.ma_chuc_vu = new SelectList(db.cv, "ma_chuc_vu", "chuc_vu");
            return View();
        }

        // POST: NhanVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANV,HOTEN,NGAYSINH,DIACHI,SDT,TAIKHOAN,MATKHAU,CV")] NHANVIEN tblNhanVien)
        {
            if (ModelState.IsValid)
            {

                if (tblNhanVien.SDT.Length == 10)
                {
                    if (tblNhanVien.CV.CompareTo("Quản lý") == 0 || tblNhanVien.CV.CompareTo("Lễ tân") == 0)
                    {
                        db.NHANVIENs.Add(tblNhanVien);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nhân viên chỉ có thể là Quản lý hoặc Lễ tân!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Số điện thoại phải đúng 10 số!");
                }
            }

            // ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

        // GET: NhanVien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN tblNhanVien = db.NHANVIENs.Find(id);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            //  ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

        // POST: NhanVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANV,HOTEN,NGAYSINH,DIACHI,SDT,TAIKHOAN,MATKHAU,CV")] NHANVIEN tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                if (tblNhanVien.SDT.Length == 10)
                {
                    if (tblNhanVien.CV.CompareTo("Quản lý") == 0 || tblNhanVien.CV.CompareTo("Lễ tân") == 0)
                    {
                        db.Entry(tblNhanVien).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nhân viên chỉ có thể là Quản lý hoặc Lễ tân!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Số điện thoại phải đúng 10 số!");
                }
            }
            //ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }

        // GET: NhanVien/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN tblNhanVien = db.NHANVIENs.Find(id);
            if (tblNhanVien == null)
            {
                return HttpNotFound();
            }
            return View(tblNhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                NHANVIEN tblNhanVien = db.NHANVIENs.Find(id);
                db.NHANVIENs.Remove(tblNhanVien);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
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
