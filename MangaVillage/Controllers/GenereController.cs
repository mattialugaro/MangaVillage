using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    [Authorize(Roles = "admin")]
    public class GenereController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Genere
        public ActionResult Index(string sortOrder)
        {
            var genere = db.Genere.ToList();

            switch (sortOrder)
            {
                default:
                    genere = genere.OrderBy(m => m.Nome).ToList();
                    break;
            }

            return View(genere);
        }

        // GET: Genere/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genere/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome")] Genere genere)
        {
            if (ModelState.IsValid)
            {
                db.Genere.Add(genere);
                db.SaveChanges();
                TempData["messaggio"] = "Genere creato con successo";
                return RedirectToAction("Index");
            }

            return View(genere);
        }

        // GET: Genere/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["errore"] = "Errore ID Genere";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genere genere = db.Genere.Find(id);
            if (genere == null)
            {
                TempData["errore"] = "Errore modifica genere";
                return HttpNotFound();
            }
            return View(genere);
        }

        // POST: Genere/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome")] Genere genere)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genere).State = EntityState.Modified;
                db.SaveChanges();
                TempData["errore"] = "Genere modficato con successo";
                return RedirectToAction("Index");
            }
            return View(genere);
        }

        // GET: Genere/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["errore"] = "Errore ID genere";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genere genere = db.Genere.Find(id);
            if (genere == null)
            {
                TempData["errore"] = "Errore eliminazione genere";
                return HttpNotFound();
            }
            return View(genere);
        }

        // POST: Genere/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Genere genere = db.Genere.Find(id);
            db.Genere.Remove(genere);
            db.SaveChanges();
            TempData["errore"] = "Genere eliminato con successo";
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
