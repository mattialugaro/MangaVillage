using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MangaVillage;

namespace MangaVillage.Controllers
{
    public class MangaController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Manga
        public ActionResult Index()
        {
            return View(db.Manga.ToList());
        }

        // GET: Manga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        // GET: Manga/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manga/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Titolo,Autore,AnnoUscita,Nazionalita,StatoPubblicazione,Copertina,Trama")] Manga manga)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var copertina = Request.Files[0];
                    if (copertina != null && copertina.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(copertina.FileName);
                        var path = Path.Combine("~/Content/Img", fileName);
                        var absolutePath = Server.MapPath(path);
                        copertina.SaveAs(absolutePath);

                        manga.Copertina = fileName;
                    }
                }

                db.Manga.Add(manga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manga);
        }

        // GET: Manga/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        // POST: Manga/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Titolo,Autore,AnnoUscita,Nazionalita,StatoPubblicazione,Copertina,Trama")] Manga manga)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var copertina = Request.Files[0];
        //        if (copertina != null && copertina.ContentLength > 0)
        //        {
        //            var fileName = Path.GetFileName(copertina.FileName);
        //            var path = Path.Combine("~/Content/img", fileName);
        //            var absolutePath = Server.MapPath(path);
        //            copertina.SaveAs(absolutePath);

        //            manga.Copertina = fileName;
        //        }

        //        db.Entry(manga).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(manga);
        //}

        public ActionResult Edit([Bind(Include = "ID,Titolo,Autore,AnnoUscita,Nazionalita,StatoPubblicazione,Copertina,Trama")] Manga manga)
        {
            if (ModelState.IsValid)
            {
                // Controlla se è presente un file caricato
                if (Request.Files.Count > 0)
                {
                    var copertina = Request.Files[0];
                    if (copertina != null && copertina.ContentLength > 0)
                    {
                        // Elimina l'immagine precedente
                        var articoloSalvato = db.Manga.Where(a => a.ID == manga.ID).First();
                        if (!string.IsNullOrEmpty(articoloSalvato.Copertina))
                        {
                            var pathToDelete = Path.Combine("~/Content/Img", manga.Copertina);
                            var absolutePathToDelete = Server.MapPath(pathToDelete);
                            System.IO.File.Delete(absolutePathToDelete);
                            //if (System.IO.File.Exists(oldImagePath))
                            //{
                            //    System.IO.File.Delete(oldImagePath);
                            //}
                        }
                        db.Entry(articoloSalvato).State = EntityState.Detached;

                        // Salva la nuova immagine
                        var fileName = Path.GetFileName(copertina.FileName);
                        var path = Path.Combine("~/Content/img", fileName);
                        var absolutePath = Server.MapPath(path);
                        copertina.SaveAs(absolutePath);

                        manga.Copertina = fileName;
                    }
                }

                db.Entry(manga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manga);
        }

        // GET: Manga/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        // POST: Manga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manga manga = db.Manga.Find(id);
            db.Manga.Remove(manga);
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

        public ActionResult Biblioteca(string sortOrder)
        {
            var manga = db.Manga.ToList();

            switch (sortOrder)
            {
                //case "Titolo":
                //    manga = manga.OrderBy(m => m.Titolo).ToList();
                //    break;
                case "TitoloDesc":
                    manga = manga.OrderByDescending(m => m.Titolo).ToList();
                    break;
                case "Anno":
                    manga = manga.OrderBy(m => m.AnnoUscita).ToList();
                    break;
                case "AnnoDesc":
                    manga = manga.OrderByDescending(m => m.AnnoUscita).ToList();
                    break;
                case "Completato":
                    manga = manga.Where(m => m.StatoPubblicazione == "completato").ToList();
                    break;
                case "InCorso":
                    manga = manga.Where(m => m.StatoPubblicazione == "in corso").ToList();
                    break;
                default:
                    manga = manga.OrderBy(m => m.Titolo).ToList();
                    break;
            }

            return View(manga);
            //return View(db.Manga.ToList());
        }
    }
}
