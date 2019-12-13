using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;
using System.IO;

namespace QLKhachsan.Areas.Admin.Controllers
{
    public class DichVuController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();

        // GET: DichVu
        public ActionResult Index()
        {
            return View(db.DICHVUs.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "MADV,TENDV,GIA,DVT,ANH,TONKHO")] DICHVU tblDichVu)
        {
            if (ModelState.IsValid)
            {
                String anh = "/Content/Images/DichVu/default.png";
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/DichVu"), pic);
                    // file is uploaded
                    file.SaveAs(path);
                    anh = "/Content/Images/DichVu/" + pic;
                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                tblDichVu.ANH = anh;
                db.DICHVUs.Add(tblDichVu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblDichVu);
        }

        // GET: DichVu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU tblDichVu = db.DICHVUs.Find(id);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "MADV,TENDV,GIA,DVT,ANH,TONKHO")] DICHVU tblDichVu)
        {
            DICHVU dv = db.DICHVUs.Find(tblDichVu.MADV);
            if (ModelState.IsValid)
            {
                String anh = dv.ANH;
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    String path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/DichVu"), pic);
                    // file is uploaded
                    file.SaveAs(path);
                    anh = "/Content/Images/DichVu/" + pic;
                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                dv.ANH = anh;
                dv.DVT = tblDichVu.DVT;
                dv.GIA = tblDichVu.GIA;
                if (dv.TENDV.Equals("Giặt ủi") || dv.TENDV.Equals("Message"))
                {
                    dv.TONKHO = null;
                }
                else
                {
                    dv.TONKHO = tblDichVu.TONKHO;
                }
                // dv.TENDV = tblDichVu.TENDV;
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblDichVu);
        }

        // GET: DichVu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVU tblDichVu = db.DICHVUs.Find(id);
            if (tblDichVu == null)
            {
                return HttpNotFound();
            }
            return View(tblDichVu);
        }

        // POST: DichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                DICHVU tblDichVu = db.DICHVUs.Find(id);
                db.DICHVUs.Remove(tblDichVu);
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
