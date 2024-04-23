﻿using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    
    public class OrdineController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Ordine
        [AllowAnonymous]
        public ActionResult Index()
        {
            string sfondo = "ordini";
            ViewBag.Sfondo = sfondo;

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
        [Authorize]
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
