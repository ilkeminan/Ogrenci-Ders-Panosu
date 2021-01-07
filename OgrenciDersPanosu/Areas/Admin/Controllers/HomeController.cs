using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OgrenciDersPanosu.Areas.Admin.Models;
using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.Filters;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using static OgrenciDersPanosu.Areas.Admin.Models.RoleModel;

namespace OgrenciDersPanosu.Areas.Admin.Controllers
{
    [AdminAuthFilter]
    public class HomeController : BaseController
    {
        // GET: Admin/Home

        //Giriş yapan admin bilgilerini listeler
        public ActionResult Index()
        {
            var id = HttpContext.User.Identity.Name;
            var user = userManager.FindByName(id);
            return View(user);
        }
        //Oluşturulan rolleri listeler
        public ActionResult Roles()
        {
            return View(roleManager.Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //Rol oluşturmayı sağlar
        public ActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(name));
                if (result.Succeeded)
                {
                    MessageBox.Show("Kaydınız başarılı bir şekilde gerçekleşmiştir", "Bilgilendirme");
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        //Rol silmeyi sağlar
        public ActionResult Delete(string id)
        {
            var role = roleManager.FindById(id);
            if (role != null)
            {
                var result = roleManager.Delete(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Role Bulunamadı" });
            }
        }

        [HttpGet]
        //Roller kullanıcı ekleyebilmeyi ve rollerden kullanıcı çıkarabilmeyi sağlar
        public ActionResult Edit(string id)
        {
            var role = roleManager.FindById(id);
            var members = new List<ApplicationUser>();
            var nonMembers = new List<ApplicationUser>();
            foreach (var user in userManager.Users.ToList())
            {
                var list = userManager.IsInRole(user.Id, role.Name) ?
                    members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }
        
        [HttpPost]
        public ActionResult Edit(RoleUpdateModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    result = userManager.AddToRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    result = userManager.RemoveFromRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Aranılan rol yok." });
        }

        //Kullanıcı listelemeyi sağlar
        public ActionResult UserList()
        {
            var roles = new List<ApplicationRole>();

            var users = userManager.Users.ToList().Select(i => new UserWithRole
            {
                user = i,
                Roles = userManager.GetRoles(i.Id)
            });
            return View(users);

        }

        public ActionResult RegisterAdmin()
        {
            return View();
        }

        //Yeni admin oluşturmayı sağlar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAdmin(RegisterAdmin model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = model.AdminId;
                user.Name = model.AdminIsim;
                user.Surname = model.AdminSoyisim;
                var result = userManager.Create(user, model.Sifre);

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    MessageBox.Show("Kaydınız başarılı bir şekilde gerçekleşmiştir", "Bilgilendirme");
                    return RedirectToAction("RegisterAdmin", new { id = User.Identity.Name });
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

        //Sistemden çıkış yapılmasını sağlar
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            Session["admin"] = null;         //Sessionda admin yoksa admin sayfalarına ulaşılamayacak.
            return RedirectToAction("index", "Home", new { Area = "" });
        }
    }
}