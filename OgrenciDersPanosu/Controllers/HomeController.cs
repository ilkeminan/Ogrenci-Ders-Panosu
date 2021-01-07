using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace OgrenciDersPanosu.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home

        //Kullanıcı kaydı kriterlerinin belirlenmesini sağlar
        public HomeController()
        {
            userManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = true,
                RequiredLength = 7,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true
            };
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        // GET: Home
        //Admin, öğretmen ve öğrenci rollerinin giriş yapabileceği giriş sayfasıdır. Her rol için farklı giriş butonu bulunur.
        public ActionResult Index()
        {
            //Sisteme giriş yapan kullanıcının kendi sayfasına yönlendirilmesini sağlar
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            else if (User.IsInRole("Ogrenci"))
            {
                return RedirectToAction("index", "Home", new { Area = "Ogrenci" });
            }
            else if (User.IsInRole("Ogretmen"))
            {
                return RedirectToAction("index", "Home", new { Area = "Ogretmen" });
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        //Admin girişidir.
        public ActionResult LoginAdmin(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            string name = User.Identity.Name;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginAdmin(LoginAdmin model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Yapılan giriş kullanıcı listesinde bulunuyorsa ve rolü adminse giriş geçerlidir. Aksi taktirde hatalı giriş uyarısı gözükecektir.
                var user = userManager.Find(model.AdminId, model.Sifre);
                if (user != null)
                {
                    var roles = userManager.GetRoles(user.Id);

                    if (roles.Count(i => i == "admin") != 0)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;

                        var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identity);
                        Session["admin"] = model;         //Sessionda admin yoksa admin sayfalarına ulaşılamayacak.
                        return RedirectToAction("index", "home", new { area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        //Öğrenci girişidir.
        public ActionResult LoginOgrenci(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("OgrenciBilgisi", "Home");
            }
            string name = User.Identity.Name;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginOgrenci(LoginOgrenci model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Yapılan giriş kullanıcı listesinde bulunuyorsa ve rolü öğrenciyse giriş geçerlidir. Aksi taktirde hatalı giriş uyarısı gözükecektir.
                var user = userManager.Find(model.OgrenciNo, model.Sifre);
                if (user != null)
                {
                    var roles = userManager.GetRoles(user.Id);

                    if (roles.Count(i => i == "Ogrenci") != 0)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;

                        var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identity);

                        return RedirectToAction("OgrenciBilgisi", "home", new { area = "Ogrenci" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        //Öğretmen girişi
        public ActionResult LoginOgretmen(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("DersListele", "Home");
            }
            string name = User.Identity.Name;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginOgretmen(LoginOgretmen model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Yapılan giriş kullanıcı listesinde bulunuyorsa ve rolü öğretmense giriş geçerlidir. Aksi taktirde hatalı giriş uyarısı gözükecektir.
                var user = userManager.Find(model.OgretmenId, model.Sifre);
                if (user != null)
                {
                    var roles = userManager.GetRoles(user.Id);

                    if (roles.Count(i => i == "Ogretmen") != 0)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;

                        var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identity);
                        return RedirectToAction("DersListele", "home", new { area = "Ogretmen" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult RegisterOgrenci()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Öğrenci yeni kayıt sayfasıdır.
        public ActionResult RegisterOgrenci(RegisterOgrenci model)
        {
            if (ModelState.IsValid)
            {
                //Kayıt bilgileri geçerliyse ve öğrenci sistemde kayıtlı değilse kayıt başarıyla gerçekleşir, aksi taktirde kayıt gerçekleşmez ve hata mesajı gözükür.
                var user = new ApplicationUser();
                user.UserName = model.OgrenciNo;
                user.Name = model.OgrenciIsim;
                user.Surname = model.OgrenciSoyisim;
                var result = userManager.Create(user, model.Sifre);

                if (result.Succeeded)
                {
                    //Öğrenci başarılı bir şekilde sisteme kaydedildiyse, diğer bilgilerine erişebilmek için öğrenciler tablosuna kaydı eklenir.
                    OgrenciModel aOgrenci = new OgrenciModel();
                    aOgrenci.Ad = model.OgrenciIsim;
                    aOgrenci.Soyad = model.OgrenciSoyisim;
                    aOgrenci.OgrenciId = model.OgrenciNo;
                    dbcontext.Ogrenciler.Add(aOgrenci);
                    dbcontext.SaveChanges();
                    
                    userManager.AddToRole(user.Id, "Ogrenci");
                    MessageBox.Show("Kaydınız başarılı bir şekilde gerçekleşmiştir","Bilgilendirme");
                    return RedirectToAction("Index", new { id = User.Identity.Name });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        public ActionResult RegisterOgretmen()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterOgretmen(RegisterOgretmen model)
        {
            if (ModelState.IsValid)
            {
                //Kayıt bilgileri geçerliyse ve öğretmen sistemde kayıtlı değilse kayıt başarıyla gerçekleşir, aksi taktirde kayıt gerçekleşmez ve hata mesajı gözükür.
                var user = new ApplicationUser();
                user.UserName = model.OgretmenId;
                user.Name = model.OgretmenIsim;
                user.Surname = model.OgretmenSoyisim;
                var result = userManager.Create(user, model.Sifre);

                if (result.Succeeded)
                {
                    //Öğretmen başarılı bir şekilde sisteme kaydedildiyse, diğer bilgilerine erişebilmek için öğretmenler tablosuna kaydı eklenir.
                    OgretmenModel aOgretmen = new OgretmenModel();
                    aOgretmen.Ad = model.OgretmenIsim;
                    aOgretmen.Soyad = model.OgretmenSoyisim;
                    aOgretmen.OgretmenId = model.OgretmenId;
                    dbcontext.Ogretmenler.Add(aOgretmen);
                    dbcontext.SaveChanges();
                    userManager.AddToRole(user.Id, "Ogretmen");
                    MessageBox.Show("Kaydınız başarılı bir şekilde gerçekleşmiştir", "Bilgilendirme");
                    return RedirectToAction("Index", new { id = User.Identity.Name });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        //Yeni kayıt işleminin hangi rol için yapılacağını belirler.
        public ActionResult SelectRoleForRegister()
        {
            return View();
        }
    }
}