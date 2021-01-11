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

namespace OgrenciDersPanosu.Areas.Ogretmen.Controllers
{
    [Authorize(Roles = "Ogretmen")]
    public class HomeController : BaseController
    {
        // GET: Ogretmen/Home

        //Giriş yapan öğretmen bilgilerinin görüntülenmesini sağlar
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

        //Seçilen derste kayıtlı öğrencileri ve öğrencilerin notlarını görüntülemeyi sağlar
        public ActionResult OgrenciNotlariniGoruntule(string dersId)
        {
            var notlar = (from not in dbcontext.Notlar where not.Ders.DersId == dersId select not).ToList();
            var ogretmen = dbcontext.Ogretmenler.FirstOrDefault(i => i.OgretmenId == User.Identity.Name);
            if (ogretmen.Dersler.Count(i => i.DersId == dersId) == 0)
            {
                return RedirectToAction("DersListele", "Home");
            }

            return View(notlar);
        }

        //Öğretmenin oluşturduğu dersleri listeler
        public ActionResult DersListele()
        {
            string id = User.Identity.Name;
            OgretmenModel aUser = dbcontext.Ogretmenler.Find(User.Identity.Name);
            return View(aUser);
        }

        //Seçilen öğreninin notlarının düzenlenmesini sağlar
        public ActionResult OgrenciNotlariniGuncelle(string notId, string dersId)
        {
            Not not = dbcontext.Notlar.Find(notId);
            Ders ders = dbcontext.Dersler.Find(dersId);

            if (ders.Notlar.Count(i => i.NotId == notId) == 0)
            {
                return RedirectToAction("DersListele", "Home");
            }
            return View(not);
        }

        [HttpPost]
        public ActionResult OgrenciNotlariniGuncelle(Not model)
        {
            var notupdate = dbcontext.Notlar.FirstOrDefault(i => i.NotId == model.NotId);
            Ders ders = dbcontext.Dersler.Find(notupdate.DersId);
            if (notupdate != null)
            {
                notupdate.Sinav1 = model.Sinav1;
                notupdate.Sinav2 = model.Sinav2;
                notupdate.Sinav3 = model.Sinav3;
                notupdate.Sozlu1 = model.Sozlu1;
                notupdate.Sozlu2 = model.Sozlu2;
                notupdate.Sozlu3 = model.Sozlu3;
                dbcontext.SaveChanges();
            }
            MessageBox.Show("Notlar Başarılı Bir Şekilde Güncellenmiştir...", "Bilgilendirme");
            return RedirectToAction("OgrenciNotlariniGoruntule", new { dersId = notupdate.DersId });
        }

        //Yeni ders oluşturulmasını sağlar
        public ActionResult DersOlustur()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DersOlustur(Ders model)
        {
            if (ModelState.IsValid)
            {
                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                Ders ders = new Ders();
                ders.DersId = model.DersId;
                ders.DersAdi = model.DersAdi;
                ders.Ogretmen = ogretmen;
                ders.OgretmenId = ogretmen.OgretmenId;
                dbcontext.Dersler.Add(ders);
                dbcontext.SaveChanges();
            }
            MessageBox.Show("Ders başarılı bir şekilde oluşturulmuştur", "Bilgilendirme");
            return View(model);
        }
    }
}