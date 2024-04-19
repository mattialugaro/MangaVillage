using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MangaVillage;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    public class OrdineController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordine
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Ordine.ToList());
            }
            else
            {
                return View(db.Ordine.Where(o => o.Utente.Username == User.Identity.Name && o.Pagato == true).ToList());
            }

        }

        // GET: Ordine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordine ordine = db.Ordine.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            return View(ordine);
        }
    }
}
