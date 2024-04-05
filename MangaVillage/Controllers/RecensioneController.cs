using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    public class RecensioneController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Recensione
        public ActionResult Index()
        {
            var recensione = db.Recensione.Include(r => r.Manga).Include(r => r.Utente);
            return View(recensione.ToList());
        }

        // GET: Recensione/Create
        public ActionResult Create()
        {
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo");
            ViewBag.IDUtenteFk = new SelectList(db.Utente, "ID", "Username");
            return View();
        }

        // POST: Recensione/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Voto,Descrizione,IDMangaFk,IDUtenteFk")] Recensione recensione)
        {
            if (ModelState.IsValid)
            {
                db.Recensione.Add(recensione);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", recensione.IDMangaFk);
            ViewBag.IDUtenteFk = new SelectList(db.Utente, "ID", "Username", recensione.IDUtenteFk);
            return View(recensione);
        }

        // GET: Recensione/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recensione recensione = db.Recensione.Find(id);
            if (recensione == null)
            {
                return HttpNotFound();
            }

            return View(recensione);
        }

        // POST: Recensione/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Voto,Descrizione,IDMangaFk,IDUtenteFk")] Recensione recensione)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recensione).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", recensione.IDMangaFk);
            ViewBag.IDUtenteFk = new SelectList(db.Utente, "ID", "Username", recensione.IDUtenteFk);
            return View(recensione);
        }

        // GET: Recensione/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recensione recensione = db.Recensione.Find(id);
            if (recensione == null)
            {
                return HttpNotFound();
            }
            return View(recensione);
        }

        // POST: Recensione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recensione recensione = db.Recensione.Find(id);
            db.Recensione.Remove(recensione);
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
