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
    public class DettaglioOrdineController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: DettaglioOrdine
        public ActionResult Index()
        {
            var dettaglioOrdine = db.DettaglioOrdine.Include(d => d.Manga).Include(d => d.Ordine);
            if (User.IsInRole("Admin"))
            {
                return View(dettaglioOrdine.ToList());
            }
            else
            {
                return View(db.DettaglioOrdine.Include(d => d.Manga)
                    .Include(d => d.Ordine)
                    .Where(d => d.Ordine.Utente.Username == User.Identity.Name && d.Ordine.Pagato == true).ToList());
            }
        }
    }
}
