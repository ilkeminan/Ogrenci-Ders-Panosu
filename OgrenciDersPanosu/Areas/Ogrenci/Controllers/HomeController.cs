﻿using Microsoft.AspNet.Identity;
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

        //Öğrencinin kayıt olduğu derslerin dersliklerine girişi sağlar, ve gönderiler listelenir. Aynı zamanda öğrenci yeni gönderi gönderebilir
        public ActionResult Derslik(string dersId)
        {
            ViewBag.dersId = dersId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Derslik(Derslik_Gonderi model, string dersId)
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
                Ders ders = dbcontext.Dersler.Find(model.dersId);
                gonderi.Ders = ders;
                ders.Gonderiler.Add(gonderi);
                dbcontext.Gonderiler.Add(gonderi);
                dbcontext.SaveChanges();
            }
            ViewBag.dersId = dersId;
            return View(model);
        }

        //Giriş yapılan derslikte kayıtlı olan öğrencileri gösterir
        public ActionResult Derslik_Uyeleri(string dersId)
        {
            var notlar = dbcontext.Notlar.Where(i => i.DersId == dersId).ToList();
            List<OgrenciModel> ogrencilist = new List<OgrenciModel>();
            foreach (Not not in notlar)
            {
                ogrencilist.Add(dbcontext.Ogrenciler.Find(not.OgrenciId));
            }
            ViewBag.dersId = dersId;
            return View(ogrencilist);
        }
    }
}