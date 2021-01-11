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

namespace OgrenciDersPanosu.Areas.Ogrenci.Controllers
{
    [Authorize(Roles = "Ogrenci")]
    public class DerslikController : BaseController
    {
        // GET: Ogrenci/Derslik
        //Öğrencinin kayıt olduğu derslerin dersliklerine girişi sağlar ve gönderiler listelenir. Aynı zamanda öğrenci yeni gönderi gönderebilir.
        public ActionResult Index(string dersId)
        {
            ViewBag.dersId = dersId;
            OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(User.Identity.Name);
            ViewBag.currentUserId = ogrenci.OgrenciId;
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

                OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(User.Identity.Name);
                gonderi.gonderenIsmi = ogrenci.Ad + " " + ogrenci.Soyad;
                gonderi.gonderenID = ogrenci.OgrenciId;
                Ders ders = dbcontext.Dersler.Find(model.dersId);
                gonderi.Ders = ders;
                ders.Gonderiler.Add(gonderi);
                dbcontext.Gonderiler.Add(gonderi);
                dbcontext.SaveChanges();

                ViewBag.currentUserId = ogrenci.OgrenciId;
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
            if(gonderi != null)
            {
                dbcontext.Gonderiler.Remove(gonderi);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index", new { dersId = dersId});
        }

        //Giriş yapılan derslikte kayıtlı olan öğrencileri gösterir
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