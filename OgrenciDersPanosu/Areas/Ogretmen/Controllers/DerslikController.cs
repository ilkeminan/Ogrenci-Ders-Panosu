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
            //Kullanıcının beğendiği gönderilerin id'sini çekme
            List<string> liked_posts = dbcontext.Begeniler.Join(dbcontext.Gonderiler,
                                 begeni => begeni.GonderiId,
                                 gonderi => gonderi.GonderiId,
                                 (begeni, gonderi) => begeni)
                                 .Where(begeni => begeni.begenenID == ogretmen.OgretmenId)
                                 .Select(begeni => begeni.GonderiId).ToList();
            ViewBag.begenilen_gonderiler = liked_posts;
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
                //Kullanıcının beğendiği gönderilerin id'sini çekme
                List<string> liked_posts = dbcontext.Begeniler.Join(dbcontext.Gonderiler,
                                     begeni => begeni.GonderiId,
                                     post => post.GonderiId,
                                     (begeni, post) => begeni)
                                     .Where(begeni => begeni.begenenID == ogretmen.OgretmenId)
                                     .Select(begeni => begeni.GonderiId).ToList();
                ViewBag.begenilen_gonderiler = liked_posts;
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
                Ders ders = dbcontext.Dersler.Find(dersId);
                List<Derslik_Gonderi> gonderiler = ders.Gonderiler.ToList();
                foreach (var gonderi_yorum in gonderiler)
                {
                    if(gonderi_yorum.UstGonderiID == gonderi.GonderiId)
                    {
                        //beğenileri silme
                        List<Begeni> likes = gonderi_yorum.begeniler.ToList();
                        foreach (var begeni in likes)
                        {
                            gonderi_yorum.begeniler.Remove(begeni);
                            dbcontext.Begeniler.Remove(begeni);
                        }
                        //yorumları silme
                        dbcontext.Gonderiler.Remove(gonderi_yorum);
                    }
                }
                //beğenileri silme
                List<Begeni> begeniler = gonderi.begeniler.ToList();
                foreach (var begeni in begeniler)
                {
                    gonderi.begeniler.Remove(begeni);
                    dbcontext.Begeniler.Remove(begeni);
                }
                //gönderi silme
                dbcontext.Gonderiler.Remove(gonderi);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index", new { dersId = dersId });
        }

        [HttpPost]
        public ActionResult Comment(string ustGonderiId, string dersId, string comment_text)
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
                gonderi.Gonderi = comment_text;
                gonderi.zaman = DateTime.Now;
                gonderi.dersId = dersId;
                gonderi.UstGonderiID = ustGonderiId;

                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                gonderi.gonderenIsmi = ogretmen.Ad + " " + ogretmen.Soyad;
                gonderi.gonderenID = ogretmen.OgretmenId;
                Ders ders = dbcontext.Dersler.Find(dersId);
                gonderi.Ders = ders;
                ders.Gonderiler.Add(gonderi);
                dbcontext.Gonderiler.Add(gonderi);
                dbcontext.SaveChanges();

                ViewBag.currentUserId = ogretmen.OgretmenId;
            }
            ViewBag.dersId = dersId;
            return RedirectToAction("Index", new { dersId = dersId });
        }

        [HttpPost]
        public ActionResult Like(string dersId, string gonderiId)
        {
            Derslik_Gonderi gonderi = dbcontext.Gonderiler.Find(gonderiId);
            if (gonderi != null)
            {
                Begeni begeni = new Begeni();
                int id = 0;
                if (dbcontext.Begeniler.Count() != 0)
                {
                    var son_begeni = dbcontext.Begeniler.OrderByDescending(w => w.zaman).First();  //id'ye göre son beğeniyi belirleme
                    id = int.Parse(son_begeni.BegeniId) + 1;                               //id son beğeninin id sinin 1 fazlası olmalı
                }
                else
                {
                    id = 0;
                }
                begeni.BegeniId = id.ToString();
                begeni.GonderiId = gonderiId;
                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                begeni.begenenID = ogretmen.OgretmenId;
                begeni.zaman = DateTime.Now;
                dbcontext.Begeniler.Add(begeni);
                gonderi.begeniSayisi++;
                gonderi.begeniler.Add(begeni);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index", new { dersId = dersId });
        }

        [HttpPost]
        public ActionResult Unlike(string dersId, string gonderiId)
        {
            Derslik_Gonderi gonderi = dbcontext.Gonderiler.Find(gonderiId);
            if (gonderi != null)
            {
                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                Begeni begeni = dbcontext.Begeniler.Where(b => b.GonderiId == gonderiId && b.begenenID == ogretmen.OgretmenId).FirstOrDefault();
                if (begeni != null)
                {
                    dbcontext.Begeniler.Remove(begeni);
                    gonderi.begeniSayisi--;
                    gonderi.begeniler.Remove(begeni);
                    dbcontext.SaveChanges();
                }
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