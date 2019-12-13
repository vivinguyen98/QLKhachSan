using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QLKhachSan.Models;
using QLKhachSan.Areas.Admin.Models;

namespace QLKhachsan.Areas.Admin.Controllers.Admin
{
    public class HoaDonController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();


        // GET: HoaDon
        public ActionResult Index()
        {
            var tblHoaDons = db.HOADONs.Where(t => t.TINHTRANGHD == 2).Include(t => t.NHANVIEN).Include(t => t.PHIEUDATPHONG);
            double tong = 0;
            foreach (var item in tblHoaDons.ToList())
            {
                if (item.TINHTRANGHD == 2)
                {
                    tong += (double)item.TONGTIEN;
                }
            }
            ViewBag.TONGTIEN = String.Format("{0:0,0.00}", tong);
            return View(tblHoaDons.ToList());
        }

        [HttpPost]
        public ActionResult Index(String beginDate, String endDate)
        {
            System.Diagnostics.Debug.WriteLine("your message here " + beginDate);
            List<HOADON> dshd = new List<HOADON>();
            String query = "select * from HOADON where TINHTRANGHD = 2 ";
            if (!beginDate.Equals(""))
                query += " and NGAYTRAPHONG >= '" + beginDate + "'";
            if (!endDate.Equals(""))
                query += " and NGAYTRAPHONG <= '" + endDate + "'";

            dshd = db.HOADONs.SqlQuery(query).ToList();
            double tong = 0;
            foreach (var item in dshd)
            {
                if (item.TINHTRANGHD == 2)
                {
                    tong += (double)item.TONGTIEN;
                }
            }
            ViewBag.TONGTIEN = tong.ToString("C");
            return View(dshd);
        }

        // GET: HoaDon/Details/5
        public ActionResult Details(int? id)
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
            return View(tblHoaDon);
        }

        // GET: HoaDon/Create
        public ActionResult Create()
        {
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "HOTEN");
            ViewBag.MAPHIEUDAT = new SelectList(db.PHIEUDATPHONGs, "MAPHIEUDAT", "MAKH");
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta");
            return View();
        }

        // POST: HoaDon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAPHIEUDAT,NGAYTRAPHONG,TINHTRANGHD")] HOADON tblHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.HOADONs.Add(tblHoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "HOTEN", tblHoaDon.MANV);
            ViewBag.MAPHIEUDAT = new SelectList(db.PHIEUDATPHONGs, "MAPHIEUDAT", "MAKH", tblHoaDon.MAHD);

            return View(tblHoaDon);
        }
        public ActionResult Add(int? id)
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
        // GET: HoaDon/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "HOTEN", tblHoaDon.MANV);
            ViewBag.MAPHIEUDAT = new SelectList(db.PHIEUDATPHONGs, "MAPHIEUDAT", "MAKH", tblHoaDon.PHIEUDATPHONG);
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta", tblHoaDon.ma_tinh_trang);
            return View(tblHoaDon);
        }

        // POST: HoaDon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NGAYTRAPHONG,TINHTRANGHD,TIENPHONG,TIENDICHVU,PHUTHU,TONGTIEN")] HOADON tblHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblHoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "HOTEN", tblHoaDon.MANV);
            ViewBag.MAPHIEUDAT = new SelectList(db.PHIEUDATPHONGs, "MAPHIEUDAT", "MAKH", tblHoaDon.MAPHIEUDAT);
            //ViewBag.ma_tinh_trang = new SelectList(db.tblTinhTrangHoaDons, "ma_tinh_trang", "mo_ta", tblHoaDon.ma_tinh_trang);
            return View(tblHoaDon);
        }

        // GET: HoaDon/Delete/5
        public ActionResult Delete(int? id)
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
            return View(tblHoaDon);
        }

        // POST: HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HOADON tblHoaDon = db.HOADONs.Find(id);
            db.HOADONs.Remove(tblHoaDon);
            db.SaveChanges();
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
        public ActionResult Result(String MAPHIEUDAT/*, String hoten1, String hoten2, String hoten3, String hoten4, String tuoi1, String tuoi2, String tuoi3, String tuoi4*/)
        {
            if (MAPHIEUDAT == null)
            {
                return RedirectToAction("Index", "Index");
            }
            else
            {

                //List<KhachHang> likh;
                PHIEUDATPHONG pt = db.PHIEUDATPHONGs.Find(Int32.Parse(MAPHIEUDAT));
                //if (pt.THONGTINKHACHTHUE == null)
                //{
                //    likh = new List<KhachHang>();
                //    likh.Add(new KhachHang("", ""));
                //}
                //else
                //{
                //    likh = JsonConvert.DeserializeObject<List<KhachHang>>(pt.THONGTINKHACHTHUE);
                //}
                //if (!hoten1.Equals(""))
                //    likh.Add(new KhachHang(hoten1, tuoi1));
                //if (!hoten2.Equals(""))
                //    likh.Add(new KhachHang(hoten2, tuoi2));
                //if (!hoten3.Equals(""))
                //    likh.Add(new KhachHang(hoten3, tuoi3));
                //if (!hoten4.Equals(""))
                //    likh.Add(new KhachHang(hoten4, tuoi4));
                //pt.THONGTINKHACHTHUE = JsonConvert.SerializeObject(likh);
                //db.Entry(pt).State = EntityState.Modified;
                //db.SaveChanges();

                HOADON hd = new HOADON();
                hd.MAPHIEUDAT = Int32.Parse(MAPHIEUDAT);
                hd.TINHTRANGHD = 1;
                try
                {
                    db.HOADONs.Add(hd);
                    PHIEUDATPHONG tgd = db.PHIEUDATPHONGs.Find(Int32.Parse(MAPHIEUDAT));
                    if (tgd == null)
                    {
                        return HttpNotFound();
                    }
                    PHONG p = db.PHONGs.Find(tgd.MAPHONG);
                    if (p == null)
                    {
                        return HttpNotFound();
                    }
                    tgd.TINHTRANGPHIEU = 2;
                    db.Entry(tgd).State = EntityState.Modified;
                    p.TINHTRANGPHONG = 2;
                    db.Entry(p).State = EntityState.Modified;
                    ViewBag.NGAYRA = tgd.NGAYRA;
                    db.SaveChanges();
                    ViewBag.Result = "success";
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }
        public ActionResult ThanhToan(int? id)
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
            DateTime dateEdukien = new DateTime(ngay_du_kien.Year, ngay_du_kien.Month, ngay_du_kien.Day, 12, 0, 0);

            Double gia = (Double)tblHoaDon.PHIEUDATPHONG.PHONG.LOAIPHONG.GIA;
            var songay = 0;
            var tien_phong = (double)0;
            if (DateTime.Compare(dateS, dateE) == 0)
            {
                TimeSpan sn = (ngay_ra - ngay_vao);
                if (sn.Hours < 1)
                {
                    tien_phong = gia / 2;
                }
                else
                {
                    songay = 1;
                    tien_phong = gia;
                }

            }
            else
            {
                songay = (int)(dateE - dateS).TotalDays;
                tien_phong = songay * gia;
            }

            //var songay = (dateE - dateS).TotalDays;
            //if (dateS > ngay_vao)
            //    songay++;
            //if (ngay_ra > dateE)
            //    songay++;
            var ti_le_phu_thu = tblHoaDon.PHIEUDATPHONG.PHONG.LOAIPHONG.TILEPHUTHU;
            
            //var so_ngay_phu_thu=0;
            //var so_ngay_phu_thu = (ngay_ra - ngay_du_kien).TotalDays;
            //if (ngay_ra < ngay_du_kien)
            //{
            //    so_ngay_phu_thu = 0;
            //}
            //else
            //{
            //    TimeSpan diff = ngay_ra - ngay_du_kien;
            //    so_ngay_phu_thu = (int)(diff.TotalDays);
            //}
            var so_ngay_phu_thu = 0;
            if (ngay_du_kien > dateE)
                so_ngay_phu_thu = 0;
            else
            {
                TimeSpan diff = dateE - ngay_du_kien;
                so_ngay_phu_thu = (int)(diff.TotalDays);
            }
            System.Diagnostics.Debug.WriteLine("So ngay: " + so_ngay_phu_thu);
            System.Diagnostics.Debug.WriteLine("Gia: " + gia);
            System.Diagnostics.Debug.WriteLine("Ti le: :" + ti_le_phu_thu);

            var phuthu = so_ngay_phu_thu * gia * ti_le_phu_thu / 100;
            ViewBag.PHUTHU = phuthu;

            System.Diagnostics.Debug.WriteLine("Phu thu:" + phuthu);

            ViewBag.SONGAYTHUPHU = so_ngay_phu_thu;
            
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
        public ActionResult GoiDichVu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoiDichVuModel mod = new GoiDichVuModel();
            mod.dsDichVu = db.DICHVUs.Where(t => t.TONKHO == null||t.TONKHO > 0).ToList();
            mod.dsDvDaDat = db.DICHVUDADATs.Where(u => u.MAHD == id).ToList();
            ViewBag.MAHD = id;
            ViewBag.SOPHONG = db.HOADONs.Find(id).PHIEUDATPHONG.PHONG.SOPHONG;
            return View(mod);
        }
        public ActionResult XacNhanGoiDichVu(String MAHD, String MADV, String SOLUONG)
        {
            if (MAHD == null || MADV == null || SOLUONG == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int mahd = Int32.Parse(MAHD);
            int madv = Int32.Parse(MADV);
            int sol = Int32.Parse(SOLUONG);
            var ds = db.DICHVUDADATs.Where(t => t.MAHD == mahd).ToList();
            try
            {
                bool check = false;
                foreach (var item in ds)
                {
                    if (item.MADV == madv)
                    {
                        item.SOLUONG += sol;
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    DICHVUDADAT dv = new DICHVUDADAT();
                    dv.MAHD = Int32.Parse(MAHD);
                    dv.MADV = Int32.Parse(MADV);
                    dv.SOLUONG = Int32.Parse(SOLUONG);
                    db.DICHVUDADATs.Add(dv);
                }
                DICHVU dichvu = db.DICHVUs.Find(madv);
                if (dichvu.TONKHO==null) {
                    dichvu.TONKHO =null;
                }
                else
                {
                    dichvu.TONKHO -= sol;
                }
              
                db.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = MAHD });
        }
        public ActionResult SuaDichVu(String MAHD, String edit_id, String edit_so_luong)
        {
            if (MAHD == null || edit_id == null || edit_so_luong == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DICHVUDADAT dsdv = db.DICHVUDADATs.Find(Int32.Parse(MAHD),Int32.Parse(edit_id));
            int sol = Int32.Parse(edit_so_luong);
            DICHVU dv = db.DICHVUs.Find(dsdv.MADV);
            int del = (int)(sol - dsdv.SOLUONG);
            if (del > dv.TONKHO)
            {
                return RedirectToAction("GoiDichVu", "HoaDon", new { id = MAHD });
            }
            else
            {
                dsdv.SOLUONG = sol;
                dv.TONKHO -= del;
                db.Entry(dsdv).State = EntityState.Modified;
                db.Entry(dv).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("GoiDichVu", "HoaDon", new { id = MAHD });
        }
        public ActionResult XoaDichVu(String MAHD, String MADV)
        {
            if (MAHD == null || MADV == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else { 
            //DICHVUDADAT d = db.DICHVUDADATs.Find(Int32.Parse(MAHD),Int32.Parse(MADV));
            var d = db.DICHVUDADATs.Where(u => u.MAHD == Int32.Parse(MAHD)&&  u.MADV==Int32.Parse(MADV));
                DICHVUDADAT dv = (DICHVUDADAT)d;
            db.DICHVUDADATs.Remove(dv);
            db.SaveChanges();
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = MAHD });
            }
        }


        /// <summary>
        /// ///////////////////

        /// <returns></returns>
        /// 

        public ActionResult XacNhanThanhToan(String MAHD, String TIENPHONG, String TIENDICHVU, String PHUTHU, String TONGTIEN)
        {
            if (MAHD == null || TIENPHONG == null || TIENDICHVU == null || PHUTHU == null || TONGTIEN == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                HOADON hd = db.HOADONs.Find(Int32.Parse(MAHD));
                NHANVIEN nv = (NHANVIEN)Session["NV"];
                if (nv != null)
                    hd.MANV = nv.MANV;
                hd.TIENPHONG = int.Parse(TIENPHONG);
                hd.TIENDICHVU = int.Parse(TIENDICHVU);
                hd.PHUTHU = int.Parse(PHUTHU);
                hd.TONGTIEN = int.Parse(TONGTIEN);
                hd.TINHTRANGHD = 2;
                hd.NGAYTRAPHONG = DateTime.Now;
                db.Entry(hd).State = EntityState.Modified;

                PHONG p = db.PHONGs.Find(hd.PHIEUDATPHONG.MAPHONG);
                p.TINHTRANGPHONG = 1;
                PHIEUDATPHONG pd = db.PHIEUDATPHONGs.Find(hd.PHIEUDATPHONG.MAPHIEUDAT);
                pd.TINHTRANGPHIEU = 4;
                db.Entry(p).State = EntityState.Modified;
                db.Entry(pd).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.result = "success";
            }
            catch
            {
                ViewBag.result = "error";
            }
            ViewBag.MAHD = MAHD;
            return View();
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
            var tien_phong = 0;
            //int tien_phong =((tblHoaDon.PHIEUDATPHONG.NGAYRA - tblHoaDon.PHIEUDATPHONG.NGAYVAO).Value.TotalDays * tblHoaDon.PHIEUDATPHONG.PHONG.LOAIPHONG.GIA);
            ViewBag.TIENPHONG = tien_phong;

            ViewBag.time_now = DateTime.Now.ToString();

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
            ViewBag.list_tt = tt;
            ViewBag.TIENDICHVU = tongtiendv;
            ViewBag.TONGTIEN = tien_phong + tongtiendv;
            return View(tblHoaDon);
        }
        public ActionResult GiaHanPhong(int? id)
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
            PHIEUDATPHONG pdp = db.PHIEUDATPHONGs.Find(tblHoaDon.MAPHIEUDAT);
            String dt = null;
            try
            {
                DateTime d = (DateTime)db.PHIEUDATPHONGs.Where(t => t.TINHTRANGPHIEU == 1 && t.MAPHONG == pdp.PHONG.MAPHONG).Select(t => t.NGAYVAO).OrderBy(t => t.Value).First();
                dt = d.ToString();
            }
            catch
            {

            }
            ViewBag.dateMax = dt;
            return View(pdp);
        }
        public ActionResult ResultGiaHan(String MAPHIEUDAT, String NGAYRA)
        {
            if (MAPHIEUDAT == null || NGAYRA == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                PHIEUDATPHONG pdp = db.PHIEUDATPHONGs.Find(Int32.Parse(MAPHIEUDAT));
                DateTime ngayra = DateTime.Parse(NGAYRA);
                pdp.NGAYRA = ngayra;

                ViewBag.result = "success";
                ViewBag.NGAYRA = NGAYRA;
            }
            catch (Exception e)
            {
                ViewBag.result = "error: " + e;
            }
            return View();
        }


        public ActionResult DoiPhong(int? id)
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
            PHIEUDATPHONG pdp = db.PHIEUDATPHONGs.Find(tblHoaDon.MAPHIEUDAT);

            var li = db.PHONGs.Where(t => t.TINHTRANGPHONG == 1 && !(db.PHIEUDATPHONGs.Where(m => (m.TINHTRANGPHIEU == 1 || m.TINHTRANGPHIEU == 2) && m.NGAYRA > DateTime.Now && m.NGAYVAO < pdp.NGAYRA)).Select(m => m.MAPHONG).ToList().Contains(t.MAPHONG));
            ViewBag.ma_phong_moi = new SelectList(li, "MAPHONG", "SOPHONG");
            return View(pdp);
        }

        public ActionResult ResultDoiPhong(String MAPHIEUDAT, String ma_phong_cu, String ma_phong_moi)
        {
            if (MAPHIEUDAT == null || ma_phong_cu == null || ma_phong_moi == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                PHIEUDATPHONG pdp = db.PHIEUDATPHONGs.Find(Int32.Parse(MAPHIEUDAT));
                PHONG p = db.PHONGs.Find(pdp.PHONG.MAPHONG);      // lấy thông tin phòng cũ
                p.TINHTRANGPHONG = 1;                                        // set phòng cũ về đang dọn
                db.Entry(p).State = EntityState.Modified;
                pdp.MAPHONG = Int32.Parse(ma_phong_moi);                   // đổi phòng cũ sang mới
                p = db.PHONGs.Find(Int32.Parse(ma_phong_moi));           // lấy thông tin phòng mới
                p.TINHTRANGPHONG = 2;                                        // set phòng mới về đang sd
                db.Entry(p).State = EntityState.Modified;
                db.Entry(pdp).State = EntityState.Modified;
                db.SaveChanges();
                @ViewBag.NGAYRA = pdp.NGAYRA;
                ViewBag.result = "success";
            }
            catch (Exception e)
            {
                ViewBag.result = "error: " + e;
            }
            return View();
        }
    }
}
