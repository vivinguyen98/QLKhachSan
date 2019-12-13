using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLKhachSan.Models;

namespace QLKhachsan.Areas.Admin.Controllers
{
    public class IndexController : Controller
    {
        // GET: Admin
        WEBSITEEntities db = new WEBSITEEntities();
        public ActionResult Index()
        {
            int so_phong_trong = 0, so_phong_sd = 0, so_phong_don = 0;
            var listPhongs = db.PHONGs.Where(t => t.TINHTRANGPHONG < 3).ToList();
            foreach (var item in listPhongs)
            {
                if (item.TINHTRANGPHONG == 1)
                    so_phong_trong++;
                else if (item.TINHTRANGPHONG == 2)
                    so_phong_sd++;
                else
                    so_phong_don++;
            }
            ViewBag.so_phong_trong = so_phong_trong;
            ViewBag.so_phong_sd = so_phong_sd;
            ViewBag.so_phong_don = so_phong_don;
            return View(listPhongs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(NHANVIEN objUser)
        {
            if (ModelState.IsValid)
            {
                var obj = db.NHANVIENs.Where(a => a.TAIKHOAN.Equals(objUser.TAIKHOAN) && a.MATKHAU.Equals(objUser.MATKHAU)).FirstOrDefault();
                if (obj != null)
                {
                    Session["NV"] = obj;
                    return RedirectToAction("Index", "ThongKe");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập sai tài khoản hoặc mật khẩu");
                }
            }
            return View(objUser);
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["NV"] != null)
                return RedirectToAction("Index", "ThongKe");
            return View();
        }
        public ActionResult Logout()
        {
            Session["NV"] = null;
            return RedirectToAction("Login", "Index");
        }


        public ActionResult ChonCachDatPhong()
        {
            return View();
        }
        public ActionResult ListPhongDangHoatDong()
        {
            var list = db.HOADONs.Where(u => u.TINHTRANGHD == 1).Include(t => t.NHANVIEN).Include(t => t.PHIEUDATPHONG);
            return View(list.ToList());
        }

        public ActionResult DSPhongGoiDV()
        {
            var list = db.HOADONs.Where(u => u.TINHTRANGHD == 1).Include(t => t.NHANVIEN).Include(t => t.PHIEUDATPHONG);
            return View(list.ToList());
        }
        public ActionResult TraPhong(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
        public ActionResult FindHdById(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ma_hd = db.HOADONs.Where(u => u.PHIEUDATPHONG.MAPHONG == id && u.TINHTRANGHD == 1).First().MAHD;
            return RedirectToAction("ThanhToan", "HoaDon", new { id = ma_hd });
        }
        public ActionResult FindHdById2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ma_hd = db.HOADONs.Where(u => u.PHIEUDATPHONG.MAPHONG == id && u.TINHTRANGHD == 1).First().MAHD;
            return RedirectToAction("GoiDichVu", "HoaDon", new { id = ma_hd });
        }
        public ActionResult DonPhongXong(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHONG p = db.PHONGs.Where(u => u.MAPHONG == id).First();
            p.TINHTRANGPHONG = 1;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Index");
        }
        public ActionResult FindRoom()
        {
            return View();
        }

        public ActionResult CaNhan()
        {
            NHANVIEN nv = (NHANVIEN)Session["NV"];
            if (nv != null)
            {
                nv = db.NHANVIENs.Find(nv.MANV);
               // ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", nv.ma_chuc_vu);
                return View(nv);
            }
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CaNhan([Bind(Include = "MANV,HOTEN,NGAYSINH,DIACHI,SDT,TAIKHOAI,MATKHAU,CV")] NHANVIEN tblNhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblNhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ma_chuc_vu = new SelectList(db.tblChucVus, "ma_chuc_vu", "chuc_vu", tblNhanVien.ma_chuc_vu);
            return View(tblNhanVien);
        }


        [HttpPost]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    string code = "";
                    List<String> dsImg = new List<string>();
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        String filename = Path.Combine(Server.MapPath("~/Content/Images/Phong/"), fname);
                        file.SaveAs(filename);
                        dsImg.Add("/Content/Images/Phong/" + fname);
                    }
                    // Returns message that successfully uploaded
                    code = Newtonsoft.Json.JsonConvert.SerializeObject(dsImg);
                    return Json(code);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}