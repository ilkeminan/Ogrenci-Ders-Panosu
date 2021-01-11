using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace OgrenciDersPanosu.Areas.Ogrenci.Controllers
{
    [Authorize(Roles = "Ogrenci")]
    public class HomeController : BaseController
    {
        // GET: Ogrenci/Home
        public ActionResult Index()
        {
            return View();
        }

        //Sistemden çıkış yapılmasını sağlar
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("index", "Home", new { Area = "" });
        }

        //Giriş yapan öğrenci bilgilerinin görüntülenmesini sağlar
        public ActionResult OgrenciBilgisi()
        {
            string id = User.Identity.Name;
            if (id != User.Identity.Name)
            {
                return RedirectToAction("OgrenciBilgisi", "Home", new { id = User.Identity.Name });
            }
            var OgrenciNo = User.Identity.Name;
            var ogrenci = dbcontext.Ogrenciler.FirstOrDefault(w => w.OgrenciId == OgrenciNo);
            return View(ogrenci);
        }

        //Öğrencinin kayıt olduğu derslerindeki ders notlarının görüntülenmesini sağlar
        public ActionResult OgrenciNotuListele()
        {
            var OgrenciNo = User.Identity.Name;
            var ogrenci = dbcontext.Ogrenciler.Find(OgrenciNo);
            return View(ogrenci);
        }

        //Ders ekle/sil işlemlerinin yapılmasını sağlar
        public ActionResult DersSecimi()
        {
           List<Ders> MevcutOlmayanDersler = new List<Ders>();
            var dersler = dbcontext.Dersler.ToList();
            OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(User.Identity.Name);
            var notlar = dbcontext.Notlar.Where(i => i.Ogrenci.OgrenciId == ogrenci.OgrenciId);
            List<Ders> mevcutDersler = new List<Ders>();
            foreach (Not not in notlar) {
                mevcutDersler.Add(dbcontext.Dersler.Find(not.DersId));
            }
            
            ViewBag.mevcut = mevcutDersler.ToList() ;
            return View(dersler);
        }

        //Seçilen dersi öğrencinin mevcut derslerine ekler
        public ActionResult SecilenDers(string dersId)
        {
            string ogrId = User.Identity.Name;
            string notId = string.Concat(dersId, ogrId);
            OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(ogrId);
            Ders ders = dbcontext.Dersler.Find(dersId);
            Not not = new Not();
            not.Ogrenci = ogrenci;
            not.OgrenciId = ogrenci.OgrenciId;
            not.Ders = ders;
            not.DersId = ders.DersId;
            not.NotId = notId;
            dbcontext.Notlar.Add(not);
            dbcontext.SaveChanges();
            return RedirectToAction("DersSecimi");
        }

        //Secilen dersi öğrencinin mevcut derslerinden çıkarır
        public ActionResult SilinecekDers(string dersId)
        {
            string ogrId = User.Identity.Name;
            Ders ders = dbcontext.Dersler.Find(dersId);

            DialogResult result = MessageBox.Show("Silme işlemini gerçekleştirmek istiyor musunuz?", "Dikkat", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                foreach (var not in dbcontext.Notlar.Where(o => o.DersId == dersId && o.OgrenciId == ogrId))
                    dbcontext.Notlar.Remove(not);
                MessageBox.Show("İşlem Başırıyla Gerçekleşmiştir");

            }
            dbcontext.SaveChanges();
            return RedirectToAction("DersSecimi");
        }
    }
}