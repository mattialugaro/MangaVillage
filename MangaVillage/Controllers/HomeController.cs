using System.Web.Mvc;

namespace MangaVillage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            string sfondo = "home";
            ViewBag.Sfondo = sfondo;
            return View();
        }
    }
}