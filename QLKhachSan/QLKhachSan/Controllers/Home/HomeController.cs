using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;

namespace QLKhachsan.Controllers
{
    public class HomeController : Controller
    {
        private WEBSITEEntities db = new WEBSITEEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[HttpPost]
        //public ActionResult Contact(String ho_ten, String mail, String noi_dung)
        //{
        //    if (noi_dung == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    KHACHHANG kh = (KHACHHANG)Session["KH"];
        //    if (kh == null)
        //    {
        //        if (ho_ten == null || mail == null)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (noi_dung.Length >= 500)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //}
           
        [HttpGet]
        public ActionResult FindRoom()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult FindRoom(String datestart, String dateend)
        {
            List<PHONG> li = new List<PHONG>();
            if (datestart.Equals("") || dateend.Equals(""))
            {
                li = db.PHONGs.ToList();
            }
            else
            {
                Session["ds_ma_phong"] = null;
                Session["ngay_vao"] = datestart;
                Session["ngay_ra"] = dateend;

                datestart = DateTime.ParseExact(datestart, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");
                dateend = DateTime.ParseExact(dateend, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                DateTime dateS = (DateTime.Parse(datestart)).AddHours(12);
                DateTime dateE = (DateTime.Parse(dateend)).AddHours(12);
                li = db.PHONGs.Where(t => !(db.PHIEUDATPHONGs.Where(m => (m.TINHTRANGPHIEU == 1 || m.TINHTRANGPHIEU == 2)
                    && m.NGAYRA > dateS && m.NGAYVAO < dateE))
                    .Select(m => m.MAPHONG).ToList().Contains(t.MAPHONG)).ToList();
            }
            return View(li);
        }

        public ActionResult ChonPhong(string id)
        {
           
            try
            {
                List<int> ds;
                ds = (List<int>)Session["ds_ma_phong"];
                if (ds == null)
                    ds = new List<int>();
                ds.Add(Int32.Parse(id));
                Session["ds_ma_phong"] = ds;
                ViewBag.result = "success";
            }
            catch
            {
                ViewBag.result = "error";
            }
            return View();
            //return RedirectToAction("BookRoom", "Home");
        }

        public ActionResult HuyChon(string id)
        {
            try
            {
                List<int> ds;
                ds = (List<int>)Session["ds_ma_phong"];
                if (ds == null)
                    ds = new List<int>();
                ds.Remove(Int32.Parse(id));
                Session["ds_ma_phong"] = ds;
                ViewBag.result = "success";
            }
            catch
            {
                ViewBag.result = "error";
            }
            return View();
        }

        public ActionResult BookRoom()
        {
            if (Session["KH"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            AutoHuyPhieuDatPhong();
            KHACHHANG kh = (KHACHHANG)Session["KH"];
            ViewBag.MAKH = kh.MAKH;
            ViewBag.TENKH = kh.HOTEN;
            ViewBag.NGAYDAT = DateTime.Now;
            ViewBag.NGAYVAO = (String)Session["ngay_vao"];
            ViewBag.NGAYRA = (String)Session["ngay_ra"];

            if (Session["ma_phong"] != null)
            {
                ViewBag.ma_phong = (String)Session["ma_phong"];
                int map = Int32.Parse((String)Session["ma_phong"]);
                PHONG p = (PHONG)db.PHONGs.Find(map);
                ViewBag.SOPHONG = p.SOPHONG;
            }
            String sp = "";
            List<int> ds;
            ds = (List<int>)Session["ds_ma_phong"];
            if (ds == null)
                ds = new List<int>();
            ViewBag.MAPHONG = JsonConvert.SerializeObject(ds);
            foreach (var item in ds)
            {
                PHONG p = (PHONG)db.PHONGs.Find(Int32.Parse(item.ToString()));
                sp += p.SOPHONG.ToString() + ", ";
            }
            ViewBag.SOPHONG = sp;
            var liP = db.PHIEUDATPHONGs.Where(u => u.MAKH == kh.MAKH && u.TINHTRANGPHIEU == 1).ToList();
            return View(liP);
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

        public ActionResult Result(String MAKH, String NGAYVAO, String NGAYRA, string MAPHONG)
        {
            if (MAKH == null || NGAYVAO == null || NGAYRA == null || MAPHONG == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                PHIEUDATPHONG tgd = new PHIEUDATPHONG();
                List<int> ds = JsonConvert.DeserializeObject<List<int>>(MAPHONG);
                tgd.MAKH =int.Parse(MAKH);
                tgd.TINHTRANGPHIEU = 1;
                tgd.NGAYDAT = DateTime.Now;
                tgd.NGAYVAO = (DateTime.ParseExact(NGAYVAO, "dd/MM/yyyy", CultureInfo.InvariantCulture)).AddHours(12);
                tgd.NGAYRA = (DateTime.ParseExact(NGAYRA, "dd/MM/yyyy", CultureInfo.InvariantCulture)).AddHours(12);
                try
                {
                    for (int i = 0; i < ds.Count; i++)
                    {
                        tgd.MAPHONG = ds[i];
                        db.PHIEUDATPHONGs.Add(tgd);
                        db.SaveChanges();
                        ViewBag.Result = "success";
                    }
                    ViewBag.ngay_vao = tgd.NGAYVAO;
                    setNull();
                }
                catch
                {
                    ViewBag.Result = "error";
                }
            }
            return View();
        }

        public ActionResult HuyPhieuDatPhong()
        {
            setNull();
            return RedirectToAction("BookRoom", "Home");
        }

        private void setNull()
        {
            Session["ngay_vao"] = null;
            Session["ngay_ra"] = null;
            Session["ma_phong"] = null;
            Session["ds_ma_phong"] = null;
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult Slider(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblPhong p = db.tblPhongs.Include(a => a.tblLoaiPhong).Where(a=>a.ma_phong==id).First();
            LOAIPHONG lp = db.LOAIPHONGs.Find(id);
            return View(lp);
        }

        //public ActionResult SMS(String ho_ten, String mail, String noi_dung)
        //{
        //    if (noi_dung == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    KHACHHANG kh = (KHACHHANG)Session["KH"];
        //    if (kh == null)
        //    {
        //        if (ho_ten == null || mail == null)
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    if (noi_dung.Length >= 500)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    tblTinNhan tn = new tblTinNhan();
        //    if (kh == null)
        //    {
        //        tn.ho_ten = ho_ten;
        //        tn.mail = mail;
        //    }
        //    else
        //    {
        //        tn.ma_kh = kh.ma_kh;
        //    }
        //    tn.noi_dung = noi_dung;
        //    try
        //    {
        //        db.tblTinNhans.Add(tn);
        //        db.SaveChanges();
        //        ViewBag.result = "success";
        //    }
        //    catch
        //    {
        //        ViewBag.result = "error";
        //    }
        //    return View();
        //}
    }
}