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

        // GET: Ordines
        public ActionResult Index(Utente utente)
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Ordine.ToList());
            }
            else
            {
                return View(db.Ordine.Where(o => o.IDUtenteFk == utente.ID).ToList());
            }
            
        }

        // GET: Ordines/Details/5
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

        // GET: Ordines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ordines/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IndirizzoConsegna,DataOrdine,Note,IDUtenteFk")] Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                db.Ordine.Add(ordine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ordine);
        }

        // GET: Ordines/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Ordines/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IndirizzoConsegna,DataOrdine,Note,IDUtenteFk")] Ordine ordine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ordine);
        }

        // GET: Ordines/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Ordines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordine ordine = db.Ordine.Find(id);
            db.Ordine.Remove(ordine);
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
