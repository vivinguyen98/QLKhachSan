using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QLKhachSan.Areas.Admin.Models;
using QLKhachSan.Models;

namespace QLKhachsan.Areas.Admin.Controllers.Admin
{
    public class PhieuDatPhongController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();

        // GET: PhieuDatPhong
        public ActionResult Index()
        {
            AutoHuyPhieuDatPhong();
            var tblPhieuDatPhongs = db.PHIEUDATPHONGs.Include(t => t.KHACHHANG).Include(t => t.PHONG);
            return View(tblPhieuDatPhongs.ToList());
        }

        private void AutoHuyPhieuDatPhong()
        {
            var datenow = DateTime.Now;
            var tblPhieuDatPhongs = db.PHIEUDATPHONGs.Where(u => u.TINHTRANGPHIEU == 1).Include(t => t.KHACHHANG).Include(t => t.PHONG).ToList();
            foreach (var item in tblPhieuDatPhongs)
            {
                System.Diagnostics.Debug.WriteLine((item.NGAYVAO - datenow).Value.Days);
                if ((item.NGAYVAO - datenow).Value.Days < 0)
                {
                    item.TINHTRANGPHIEU = 2;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }


        public ActionResult List()
        {
            AutoHuyPhieuDatPhong();
            var tblPhieuDatPhongs = db.PHIEUDATPHONGs.Where(t => t.TINHTRANGPHIEU == 1 && t.NGAYVAO.Value.Day == DateTime.Now.Day && t.NGAYVAO.Value.Month == DateTime.Now.Month && t.NGAYVAO.Value.Year == DateTime.Now.Year).Include(t => t.KHACHHANG).Include(t => t.PHONG);
            return View(tblPhieuDatPhongs.ToList());
        }

        // GET: PhieuDatPhong/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }

        // GET: PhieuDatPhong/Create

        public ActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewBag.select_ma_phong = id;
            }
            ViewBag.ma_kh = new SelectList(db.KHACHHANGs, "MAKH", "MAKH");
            ViewBag.ma_phong = new SelectList(db.PHONGs.Where(u => u.TINHTRANGPHONG == 1), "MAPHONG", "SOPHONG");
            return View();
        }


        public ActionResult SelectRoom(String dateE)
        {
            if (dateE == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ma_kh = new SelectList(db.KHACHHANGs, "MAKH", "MAKH");
            DateTime ngay_ra = (DateTime.Parse(dateE)).AddHours(12);
            ViewBag.ngay_ra = ngay_ra;
            var s = db.PHONGs.Where(t => !(db.PHIEUDATPHONGs.Where(m => (m.TINHTRANGPHIEU == 1 || m.TINHTRANGPHIEU == 2) && (m.NGAYRA > DateTime.Now && m.NGAYRA < ngay_ra))).Select(m => m.MAPHONG).ToList().Contains(t.MAPHONG) && t.TINHTRANGPHONG == 1);
            ViewBag.ma_phong = new SelectList(s, "ma_phong", "so_phong");
            return View();
        }


        // POST: PhieuDatPhong/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(String radSelect, [Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] PHIEUDATPHONG tblPhieuDatPhong, [Bind(Include = "hoten,socmt,tuoi,sodt")] KhachHang kh)
        {
            System.Diagnostics.Debug.WriteLine("SS :" + radSelect);
            if (radSelect.Equals("rad2"))
            {
                tblPhieuDatPhong.MAKH = null;
                List<KhachHang> likh = new List<KhachHang>();
                likh.Add(kh);
                String ttkh = JsonConvert.SerializeObject(likh);
                tblPhieuDatPhong.THONGTINKHACHTHUE = ttkh;
            }

            tblPhieuDatPhong.TINHTRANGPHIEU = 1;
            tblPhieuDatPhong.NGAYVAO = DateTime.Now;
            tblPhieuDatPhong.NGAYDAT = DateTime.Now;
            db.PHIEUDATPHONGs.Add(tblPhieuDatPhong);
            db.SaveChanges();
            int ma = tblPhieuDatPhong.MAPHIEUDAT;
            return RedirectToAction("Add", "HoaDon", new { id = ma });

           // ViewBag.MAKH = new SelectList(db.KHACHHANGs, "MAKH", "MAKH", PHIEUDATPHONG.MAKH);
           // ViewBag.MAPHONG = new SelectList(db.PHONGs, "MAPHONG", "SOPHONG", PHIEUDATPHONG.MAPHONG);
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return View(tblPhieuDatPhong);
        }

        // GET: PhieuDatPhong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            ViewBag.ma_kh = new SelectList(db.KHACHHANGs, "MAKH", "MATKHAU", tblPhieuDatPhong.MAKH);
            ViewBag.ma_phong = new SelectList(db.PHONGs, "MAPHONG", "SOPHONG", tblPhieuDatPhong.MAPHONG);
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] PHIEUDATPHONG tblPhieuDatPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ma_kh = new SelectList(db.KHACHHANGs, "MAKH", "MATKHAU", tblPhieuDatPhong.MAKH);
            ViewBag.ma_phong = new SelectList(db.PHONGs, "MAPHONG", "SOPHONG", tblPhieuDatPhong.MAPHONG);
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return View(tblPhieuDatPhong);
        }

        // GET: PhieuDatPhong/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
                db.PHIEUDATPHONGs.Remove(tblPhieuDatPhong);
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
