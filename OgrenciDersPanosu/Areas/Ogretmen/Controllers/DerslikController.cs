using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace OgrenciDersPanosu.Areas.Ogretmen.Controllers
{
    [Authorize(Roles = "Ogretmen")]
    public class DerslikController : BaseController
    {
        // GET: Ogretmen/Derslik
        //Seçilen dersin derslik sayfasına gidilmesini sağlar, burada öğrencilerle bilgi paylaşımı yapılabilir.
        public ActionResult Index(string dersId)
        {
            ViewBag.dersId = dersId;
            OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
            ViewBag.currentUserId = ogretmen.OgretmenId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Derslik_Gonderi model, string dersId)
        {
            if (ModelState.IsValid)
            {
                Derslik_Gonderi gonderi = new Derslik_Gonderi();
                int id;
                if (dbcontext.Gonderiler.Count() != 0)
                {
                    var son_gonderi = dbcontext.Gonderiler.OrderByDescending(w => w.zaman).First();  //zamana göre son gönderiyi belirleme
                    id = int.Parse(son_gonderi.GonderiId) + 1;                               //id son gönderinin id sinin 1 fazlası olmalı
                }
                else
                {
                    id = 0;
                }
                gonderi.GonderiId = id.ToString();
                gonderi.Gonderi = model.Gonderi;
                gonderi.zaman = DateTime.Now;
                gonderi.dersId = dersId;

                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                gonderi.gonderenIsmi = ogretmen.Ad + " " + ogretmen.Soyad;
                gonderi.gonderenID = ogretmen.OgretmenId;
                Ders ders = dbcontext.Dersler.Find(model.dersId);
                gonderi.Ders = ders;
                ders.Gonderiler.Add(gonderi);
                dbcontext.Gonderiler.Add(gonderi);
                dbcontext.SaveChanges();

                ViewBag.currentUserId = ogretmen.OgretmenId;
            }
            ViewBag.dersId = dersId;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string dersId, string gonderiId, string text)
        {
            Derslik_Gonderi gonderi = dbcontext.Gonderiler.Find(gonderiId);
            if (gonderi != null)
            {
                gonderi.Gonderi = text;
                gonderi.zaman = DateTime.Now;
                dbcontext.Entry(gonderi).State = EntityState.Modified;
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index", new { dersId = dersId });
        }

        public ActionResult Delete(string dersId, string gonderiId)
        {
            Derslik_Gonderi gonderi = dbcontext.Gonderiler.Find(gonderiId);
            if (gonderi != null)
            {
                dbcontext.Gonderiler.Remove(gonderi);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index", new { dersId = dersId });
        }

        //İlgili derse kayıtlı öğrencilerin görüntülenmesini sağlar
        public ActionResult Derslik_Uyeleri(string dersId, string search)
        {
            var notlar = dbcontext.Notlar.Where(i => i.DersId == dersId).ToList();
            List<OgrenciModel> ogrencilist = new List<OgrenciModel>();
            foreach (Not not in notlar)
            {
                ogrencilist.Add(dbcontext.Ogrenciler.Find(not.OgrenciId));
            }

            if (search != null)
            {
                //Arama sonucunda filtrelenmiş listeyi getirir.
                List<OgrenciModel> filtrelenmis =
                ogrencilist.Where(x =>
                    x.OgrenciId.Contains(search) ||
                    x.Ad.Contains(search) ||
                    x.Soyad.Contains(search)).ToList();
                ViewBag.dersId = dersId;
                return View(filtrelenmis);
            }

            ViewBag.dersId = dersId;
            return View(ogrencilist);
        }
    }
}