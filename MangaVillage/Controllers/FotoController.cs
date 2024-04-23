using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FotoController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Foto
        public ActionResult Index()
        {
            var foto = db.Foto.Include(f => f.Manga);
            return View(foto.ToList());
        }

        // GET: Foto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foto foto = db.Foto.Find(id);
            if (foto == null)
            {
                return HttpNotFound();
            }
            return View(foto);
        }

        // GET: Foto/Create
        public ActionResult Create()
        {
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo");
            return View();
        }

        // POST: Foto/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,IDMangaFk")] Foto foto)
        {
            if (ModelState.IsValid)
            {
                db.Foto.Add(foto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", foto.IDMangaFk);
            return View(foto);
        }

        // GET: Foto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foto foto = db.Foto.Find(id);
            if (foto == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", foto.IDMangaFk);
            return View(foto);
        }

        // POST: Foto/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,IDMangaFk")] Foto foto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDMangaFk = new SelectList(db.Manga, "ID", "Titolo", foto.IDMangaFk);
            return View(foto);
        }

        // GET: Foto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foto foto = db.Foto.Find(id);
            if (foto == null)
            {
                return HttpNotFound();
            }
            return View(foto);
        }

        // POST: Foto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Foto foto = db.Foto.Find(id);
            db.Foto.Remove(foto);
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
