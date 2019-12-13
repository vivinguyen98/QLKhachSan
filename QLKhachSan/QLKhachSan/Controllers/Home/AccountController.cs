using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;

namespace QLKhachsan.Controllers.Home
{
    public class AccountController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(db.KHACHHANGs.ToList());
        }

        // GET: KhachHang/Details/5
        public ActionResult Details(string id)
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

        // GET: KhachHang/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "HOTEN, EMAIL, MATKHAU, CMT, SDT")] KHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.KHACHHANGs.Find(tblKhachHang.MAKH) == null)
                {
                    if (tblKhachHang.SDT.Length == 10 && (tblKhachHang.CMT.Length == 9 || tblKhachHang.CMT.Length == 12))
                    {
                        if (tblKhachHang.DIEM == null)
                            tblKhachHang.DIEM = 0;
                        db.KHACHHANGs.Add(tblKhachHang);
                        db.SaveChanges();
                        Session["KH"] = tblKhachHang;
                        return RedirectToAction("BookRoom", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nhập sai định dạng số điện thoại hoặc chứng minh thư !");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản này đã được sử dụng !");
                }
            }

            return View(tblKhachHang);
        }

        public ActionResult Add()
        {
            return View();
        }

        // POST: KhachHang/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "HOTEN,EMAIL,MATKHAU,CMT,SDT")] KHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                if (db.KHACHHANGs.Find(tblKhachHang.MAKH) == null)
                {
                    db.KHACHHANGs.Add(tblKhachHang);
                    db.SaveChanges();
                    return RedirectToAction("FindRoom", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản này đã được sử dụng !");
                }
            }

            return View(tblKhachHang);
        }

        // GET: KhachHang/Edit/5
        public ActionResult Edit(string id)
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

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HOTEN,EMAIL,MATKHAU,CMT,SDT")] KHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblKhachHang);
        }


        public ActionResult CaNhan()
        {
            KHACHHANG kh = new KHACHHANG();
            if (Session["KH"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                kh = (KHACHHANG)Session["KH"];
                if (kh.DIEM == null)
                {
                    kh.DIEM = 0;
                }
                else
                { }
            }
            return View(kh);
        }

        // POST: KhachHang/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "MAKH,HOTEN,EMAIL,MATKHAU,CMT,SDT,DIEM")] KHACHHANG tblKhachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblKhachHang).State = EntityState.Modified;
                db.SaveChanges();
                Session["KH"] = tblKhachHang;
                return RedirectToAction("Index", "Home");
            }
            return View(tblKhachHang);
        }

        // GET: KhachHang/Delete/5
        public ActionResult Delete(string id)
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

        // POST: KhachHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                KHACHHANG tblKhachHang = db.KHACHHANGs.Find(id);
                db.KHACHHANGs.Remove(tblKhachHang);
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(KHACHHANG objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.KHACHHANGs.Where(a => a.EMAIL.Equals(objUser.EMAIL) && a.MATKHAU.Equals(objUser.MATKHAU)).FirstOrDefault();
                if (obj != null)
                {
                    Session["KH"] = obj;
                    return RedirectToAction("BookRoom", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập sai tài khoản hoặc mật khẩu!");
                }
            }
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Login()
        {
            Session["KH"] = null;
            KHACHHANG kh = (KHACHHANG)Session["KH"];
            if (kh != null)
                return RedirectToAction("BookRoom", "Home");
            return View();
        }




        public ActionResult SuaPhieuDatPhong(int? id)
        {
            KHACHHANG kh = new KHACHHANG();
            if (Session["KH"] != null)
                kh = (KHACHHANG)Session["KH"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.MAKH != kh.MAKH)
                return RedirectToAction("Index", "Home");
            ViewBag.ma_kh = new SelectList(db.KHACHHANGs, "EMAIL", "MATKHAU", tblPhieuDatPhong.MAKH);
            ViewBag.ma_phong = new SelectList(db.PHONGs, "MAPHONG", "SOPHONG", tblPhieuDatPhong.MAPHONG);
            // ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaPhieuDatPhong([Bind(Include = "ma_pdp,ma_kh,ngay_dat,ngay_vao,ngay_ra,ma_phong,ma_tinh_trang")] PHIEUDATPHONG tblPhieuDatPhong)
        {
            if (ModelState.IsValid)
            {
                tblPhieuDatPhong.TINHTRANGPHIEU = 1;
                db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("BookRoom", "Home");
            }
            ViewBag.ma_kh = new SelectList(db.KHACHHANGs, "ma_kh", "mat_khau", tblPhieuDatPhong.MAKH);
            ViewBag.ma_phong = new SelectList(db.PHONGs, "ma_phong", "so_phong", tblPhieuDatPhong.MAPHONG);
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangPhieuDatPhongs, "ma_tinh_trang", "tinh_trang", tblPhieuDatPhong.ma_tinh_trang);
            return RedirectToAction("BookRoom", "Home");
        }

        // GET: PhieuDatPhong/Delete/5
        public ActionResult XoaPhieuDatPhong(int? id)
        {
            KHACHHANG kh = new KHACHHANG();
            if (Session["KH"] != null)
                kh = (KHACHHANG)Session["KH"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
            if (tblPhieuDatPhong == null)
            {
                return HttpNotFound();
            }
            if (tblPhieuDatPhong.MAKH != kh.MAKH)
                return RedirectToAction("Index", "Home");
            return View(tblPhieuDatPhong);
        }

        // POST: PhieuDatPhong/Delete/5
        [HttpPost, ActionName("XoaPhieuDatPhong")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmXoaPhieuDatPhong(int id)
        {
            PHIEUDATPHONG tblPhieuDatPhong = db.PHIEUDATPHONGs.Find(id);
            tblPhieuDatPhong.TINHTRANGPHIEU = 3;
            db.Entry(tblPhieuDatPhong).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("BookRoom", "Home");
        }

        public ActionResult Logout()
        {
            Session["KH"] = null;
            return RedirectToAction("Login", "Account");
        }






        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult HoaDon()
        {
            KHACHHANG kh = new KHACHHANG();
            if (Session["KH"] != null)
                kh = (KHACHHANG)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsHoaDon = db.HOADONs.Where(t => t.PHIEUDATPHONG.MAKH == kh.MAKH).ToList();
            return View(dsHoaDon);
        }
        public ActionResult PhieuDatPhong()
        {
            AutoHuyPhieuDatPhong();
            KHACHHANG kh = new KHACHHANG();
            if (Session["KH"] != null)
                kh = (KHACHHANG)Session["KH"];
            else
                return RedirectToAction("Index", "Home");

            var dsPDP = db.PHIEUDATPHONGs.Where(t => t.MAKH == kh.MAKH).ToList();
            return View(dsPDP);
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
                    item.TINHTRANGPHIEU = 3;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public ActionResult ChiTietHoaDon(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOADON tblHoaDon = db.HOADONs.Find(id);
            if (tblHoaDon == null)
            {
                return HttpNotFound();
            }
            DateTime ngay_ra;
            if (tblHoaDon.NGAYTRAPHONG == null)
            {
                ngay_ra = DateTime.Now;
            }
            else
            {
                ngay_ra = (DateTime)tblHoaDon.NGAYTRAPHONG;
            }
            DateTime ngay_vao = (DateTime)tblHoaDon.PHIEUDATPHONG.NGAYVAO;
            DateTime ngay_du_kien = (DateTime)tblHoaDon.PHIEUDATPHONG.NGAYRA;


            DateTime dateS = new DateTime(ngay_vao.Year, ngay_vao.Month, ngay_vao.Day, 12, 0, 0);
            DateTime dateE = new DateTime(ngay_ra.Year, ngay_ra.Month, ngay_ra.Day, 12, 0, 0);

            Double gia = (Double)tblHoaDon.PHIEUDATPHONG.PHONG.LOAIPHONG.GIA;

            var songay = (dateE - dateS).TotalDays;
            //if (dateS > ngay_vao)
            //    songay++;
            //if (ngay_ra > dateE)
            //    songay++;
            var ti_le_phu_thu = tblHoaDon.PHIEUDATPHONG.PHONG.LOAIPHONG.TILEPHUTHU;
            var so_ngay_phu_thu = Math.Abs(Math.Ceiling((ngay_ra - ngay_du_kien).TotalDays));

            System.Diagnostics.Debug.WriteLine("So ngay: " + so_ngay_phu_thu);
            System.Diagnostics.Debug.WriteLine("Gia: " + gia);
            System.Diagnostics.Debug.WriteLine("Ti le: :" + ti_le_phu_thu);

            var phuthu = so_ngay_phu_thu * gia * ti_le_phu_thu / 100;
            ViewBag.PHUTHU = phuthu;

            System.Diagnostics.Debug.WriteLine("Phu thu:" + phuthu);

            ViewBag.SONGAYTHUPHU = so_ngay_phu_thu;
            var tien_phong = songay * gia;
            ViewBag.TIENPHONG = tien_phong;
            ViewBag.SONGAY = songay;

            NHANVIEN nv = (NHANVIEN)Session["NV"];
            if (nv != null)
            {
                ViewBag.HOTEN = nv.HOTEN;
            }
            List<DICHVUDADAT> dsdv = db.DICHVUDADATs.Where(u => u.MAHD == id).ToList();
            ViewBag.list_dv = dsdv;
            double tongtiendv = 0;
            List<double> tt = new List<double>();
            foreach (var item in dsdv)
            {
                double t = (double)(item.SOLUONG * item.DICHVU.GIA);
                tongtiendv += t;
                tt.Add(t);
            }
            ViewBag.NGAYTRAPHONG = ngay_ra;
            ViewBag.list_tt = tt;
            ViewBag.TIENDICHVU = tongtiendv;
            ViewBag.TONGTIEN = tien_phong + tongtiendv + phuthu;
            return View(tblHoaDon);
        }

        internal class ChallengeResult : ActionResult
        {
            private string provider;
            private string v1;
            private string v2;

            public ChallengeResult(string provider, string v1, string v2)
            {
                this.provider = provider;
                this.v1 = v1;
                this.v2 = v2;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
