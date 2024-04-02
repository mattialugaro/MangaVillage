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
using Newtonsoft.Json;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    public class MangaController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Manga
        public ActionResult Index()
        {
            var mangaList = db.Manga.ToList();
            if (mangaList != null && mangaList.Count > 0)
            {
                foreach (var manga in mangaList)
                {
                    LoadCategoriaGenere(manga);
                }
            }

            return View(mangaList);
        }

        private static void LoadCategoriaGenere(Manga manga)
        {
            if (manga != null && manga.Genere != null && manga.Genere.Count > 0)
            {
                foreach (var genere in manga.Genere)
                {
                    if (genere != null && genere.Nome != null)
                    {
                        manga.GenereString += genere.Nome + ", ";
                    }
                }

                if (manga.GenereString != null && manga.GenereString.Length > 0)
                {
                    manga.GenereString = manga.GenereString.Substring(0, manga.GenereString.Length - 2);
                }
            }

            if (manga != null && manga.Categoria != null && manga.Categoria.Count > 0)
            {
                foreach (var categoria in manga.Categoria)
                {
                    if (categoria != null && categoria.Nome != null)
                    {
                        manga.CategoriaString += categoria.Nome + ", ";
                    }
                }

                if (manga.CategoriaString != null && manga.CategoriaString.Length > 0)
                {
                    manga.CategoriaString = manga.CategoriaString.Substring(0, manga.CategoriaString.Length - 2);
                }
            }
        }

        // GET: Manga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            LoadCategoriaGenere(manga);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        // GET: Manga/Create
        public ActionResult Create()
        {
            Manga manga = new Manga();
            manga.CategoriaTendina = db.Categoria.ToList();
            manga.GenereTendina = db.Genere.ToList();
            //var selectListGeneri = manga.GenereTendina.Select(g => new SelectListItem { Text = g.Nome, Value = g.ID.ToString() }).ToList();
            //ViewBag.Generi = selectListGeneri;
            return View(manga);
        }

        // POST: Manga/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Titolo,Autore,AnnoUscita,Nazionalita,StatoPubblicazione,Categoria,Genere,Copertina,Trama")] Manga manga)
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

                //manga.Categoria = new List<Categoria>();
                //Categoria categoria = db.Categoria.Find(1);
                //manga.Categoria.Add(categoria);

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
            LoadCategoriaGenere(manga);
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
            LoadCategoriaGenere(manga);
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
            LoadCategoriaGenere(manga);
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
        }

        public ActionResult Ricerca()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ricerca(string titolo, string autore, string annoUscita, string nazionalita, string statoPubblicazione)
        {
            var query = db.Manga.AsQueryable();

            if (!string.IsNullOrEmpty(titolo))
            {
                query = query.Where(m => m.Titolo.Contains(titolo));
            }

            if (!string.IsNullOrEmpty(autore))
            {
                query = query.Where(m => m.Autore.Contains(autore));
            }

            if (!string.IsNullOrEmpty(annoUscita))
            {
                query = query.Where(m => m.AnnoUscita.Contains(annoUscita));
            }

            if (!string.IsNullOrEmpty(nazionalita))
            {
                query = query.Where(m => m.Nazionalita == nazionalita);
            }

            if (!string.IsNullOrEmpty(statoPubblicazione))
            {
                query = query.Where(m => m.StatoPubblicazione == statoPubblicazione);
            }

            var results = query.ToList();

            return View(results);
        }

    }
}
