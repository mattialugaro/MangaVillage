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
        public ActionResult Index(Utente utente)
        {
            var dettaglioOrdine = db.DettaglioOrdine.Include(d => d.Manga).Include(d => d.Ordine);
            if (User.IsInRole("Admin"))
            {
                return View(dettaglioOrdine.ToList());
            }
            else
            {
                return View(db.DettaglioOrdine.Include(d => d.Manga).Include(d => d.Ordine).Where(d => d.Ordine.IDUtenteFk == utente.ID).ToList());
            }
        }

        // GET: DettaglioOrdine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DettaglioOrdine dettaglioOrdine = db.DettaglioOrdine.Find(id);
            if (dettaglioOrdine == null)
            {
                return HttpNotFound();
            }
            return View(dettaglioOrdine);
        }

        // GET: DettaglioOrdine/Create
        public ActionResult Create()
        {
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo");
            ViewBag.IDOrdineFk = new SelectList(db.Ordine, "ID", "IndirizzoConsegna");
            return View();
        }

        // POST: DettaglioOrdine/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDOrdineFk,IDMangaFk,NumeroVolume,Quantita")] DettaglioOrdine dettaglioOrdine)
        {
            if (ModelState.IsValid)
            {
                db.DettaglioOrdine.Add(dettaglioOrdine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", dettaglioOrdine.IDMangaFk);
            ViewBag.IDOrdineFk = new SelectList(db.Ordine, "ID", "IndirizzoConsegna", dettaglioOrdine.IDOrdineFk);
            return View(dettaglioOrdine);
        }

        // GET: DettaglioOrdine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DettaglioOrdine dettaglioOrdine = db.DettaglioOrdine.Find(id);
            if (dettaglioOrdine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", dettaglioOrdine.IDMangaFk);
            ViewBag.IDOrdineFk = new SelectList(db.Ordine, "ID", "IndirizzoConsegna", dettaglioOrdine.IDOrdineFk);
            return View(dettaglioOrdine);
        }

        // POST: DettaglioOrdine/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDOrdineFk,IDMangaFk,NumeroVolume,Quantita")] DettaglioOrdine dettaglioOrdine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dettaglioOrdine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", dettaglioOrdine.IDMangaFk);
            ViewBag.IDOrdineFk = new SelectList(db.Ordine, "ID", "IndirizzoConsegna", dettaglioOrdine.IDOrdineFk);
            return View(dettaglioOrdine);
        }

        // GET: DettaglioOrdine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DettaglioOrdine dettaglioOrdine = db.DettaglioOrdine.Find(id);
            if (dettaglioOrdine == null)
            {
                return HttpNotFound();
            }
            return View(dettaglioOrdine);
        }

        // POST: DettaglioOrdine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DettaglioOrdine dettaglioOrdine = db.DettaglioOrdine.Find(id);
            db.DettaglioOrdine.Remove(dettaglioOrdine);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
