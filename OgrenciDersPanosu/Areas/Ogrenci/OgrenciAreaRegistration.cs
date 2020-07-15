using System.Web.Mvc;

namespace OgrenciDersPanosu.Areas.Ogrenci
{
    public class OgrenciAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ogrenci";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ogrenci_default",
                "Ogrenci/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}