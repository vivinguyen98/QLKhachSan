using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;

namespace QLKhachsan.Areas.Admin.Controllers.Admin
{
    public class PhongController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();

        // GET: Phong
        public ActionResult Index()
        {
            var PHONGs = db.PHONGs.Where(t => t.TINHTRANGPHONG < 4).Include(t => t.LOAIPHONG).Include(t => t.TANG);
            return View(PHONGs.ToList());
        }

        // GET: Phong/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHONG PHONG = db.PHONGs.Find(id);
            if (PHONG == null)
            {
                return HttpNotFound();
            }
            return View(PHONG);
        }

        // GET: Phong/Create
        public ActionResult Create()
        {
            ViewBag.MALOAI = new SelectList(db.LOAIPHONGs, "MALOAI", "MOTA");
            ViewBag.MATANG = new SelectList(db.TANGs, "MATANG", "TENTANG");
            return View();
        }

        // POST: Phong/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SOPHONG,MALOAI,MATANG,TINHTRANGPHONG")] PHONG PHONG)
        {
            if (ModelState.IsValid)
            {
                PHONG.TINHTRANGPHONG = 1;
                db.PHONGs.Add(PHONG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MALOAI = new SelectList(db.LOAIPHONGs, "MALOAI", "MOTA");
            ViewBag.MATANG = new SelectList(db.TANGs, "MATANG", "TENTANG");
            return View(PHONG);
        }

        // GET: Phong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHONG PHONG = db.PHONGs.Find(id);
            if (PHONG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALOAI = new SelectList(db.LOAIPHONGs, "MALOAI", "MOTA");
            ViewBag.MATANG = new SelectList(db.TANGs, "MATANG", "TENTANG");
            return View(PHONG);
        }

        // POST: Phong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAPHONG,SOPHONG,MALOAI,MATANG,TINHTRANGPHONG")] PHONG PHONG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(PHONG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MALOAI = new SelectList(db.LOAIPHONGs, "MALOAI", "MOTA");
            ViewBag.MATANG = new SelectList(db.TANGs, "MATANG", "TENTANG");
            return View(PHONG);
        }

        // GET: Phong/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHONG PHONG = db.PHONGs.Find(id);
            if (PHONG == null)
            {
                return HttpNotFound();
            }
            return View(PHONG);
        }

        // POST: Phong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                PHONG PHONG = db.PHONGs.Find(id);
                PHONG.TINHTRANGPHONG = 4;
                db.Entry(PHONG).State = EntityState.Modified;
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
