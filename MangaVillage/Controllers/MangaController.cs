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
using System.Data.SqlClient;
using System.Configuration;

namespace MangaVillage.Controllers
{
    public class MangaController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // Metodo che controlla che Categoria e Genere non siano vuoti/null e li concatena avendone piu' di uno
        private static void LoadCategoriaGenere(Manga manga)
        {
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
        }

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

        // GET: Manga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manga manga = db.Manga.Find(id);
            manga.Recensione = db.Recensione.Where(r => r.IDMangaFk == manga.ID).ToList();
            LoadCategoriaGenere(manga);
            if (manga == null)
            {
                return HttpNotFound();
            }
            return View(manga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggiungiRecensione([Bind(Include = "Voto,Descrizione,IDMangaFk,IDUtenteFk")] Recensione recensione)
        {
            if (ModelState.IsValid)
            {
                if (recensione != null)
                {
                    recensione.IDUtenteFk = int.Parse(Request.Cookies.Get("ID").Value);
                    db.Recensione.Add(recensione);
                    TempData["messaggio"] = "Recensione creata con successo";
                    db.SaveChanges();
                }
                return RedirectToAction("Details/" + recensione.IDMangaFk);
            }

            return View("Details", "Manga");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AggiungiArticolo([Bind(Include = "IDMangaFk,NumeroVolume,Quantita")] DettaglioOrdine dettaglioOrdine)
        {
            var carrello = Session["carrello"] as Ordine;
            if (carrello == null)
            {
                carrello = new Ordine();
                Session["carrello"] = carrello;
                carrello.IDUtenteFk = Convert.ToInt32(Request.Cookies["ID"].Value);
                carrello.Pagato = false;
            }

            if(carrello.DettaglioOrdine != null && carrello.DettaglioOrdine.Count > 0)
            {
                bool inCarrello = false;
                foreach (var articolo in carrello.DettaglioOrdine)
                {
                    if (articolo.Equals(dettaglioOrdine))
                    {
                        articolo.Quantita += dettaglioOrdine.Quantita;
                        inCarrello = true;
                        TempData["messaggio"] = "Manga aggiunto al carrello con successo";
                    }
                }
                if (!inCarrello)
                {
                    dettaglioOrdine.Manga = db.Manga.Find(dettaglioOrdine.IDMangaFk);
                    carrello.DettaglioOrdine.Add(dettaglioOrdine);
                    TempData["messaggio"] = "Manga aggiunto al carrello con successo";
                }
            }
            else
            {
                dettaglioOrdine.Manga = db.Manga.Find(dettaglioOrdine.IDMangaFk);
                carrello.DettaglioOrdine.Add(dettaglioOrdine);
                TempData["messaggio"] = "Manga aggiunto al carrello con successo";
            }

            Session["carrello"] = carrello;

            return RedirectToAction("Details/" + dettaglioOrdine.IDMangaFk, "Manga");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult NuovoVolume(int id)
        //{
        //    Manga manga = db.Manga.Find(id);
        //    manga.UltimoVolume += 1;
        //    db.Entry(manga).State = EntityState.Modified;
        //    TempData["messaggio"] = "Ultimo Volume aggiornato con successo";
        //    db.SaveChanges();

        //    return View("Index");
        //}


        // GET: Manga/Create
        public ActionResult Create()
        {
            Manga manga = new Manga();
            manga.CategoriaTendina = db.Categoria.ToList();
            manga.GenereTendina = db.Genere.ToList();
            return View(manga);
        }

        // POST: Manga/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Titolo,Autore,AnnoUscita,Nazionalita,StatoPubblicazione,Copertina,Trama,UltimoVolume,Prezzo,CategoriaTendinaSelezione,GenereTendinaSelezione")] Manga manga)
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

                if (manga.CategoriaTendinaSelezione != null && manga.CategoriaTendinaSelezione.Count > 0)
                {
                    manga.Categoria = new List<Categoria>();
                    foreach (var categoriaSelezione in manga.CategoriaTendinaSelezione)
                    {
                        Categoria categoria = db.Categoria.Find(int.Parse(categoriaSelezione));
                        manga.Categoria.Add(categoria);
                    }
                }

                if (manga.GenereTendinaSelezione != null && manga.GenereTendinaSelezione.Count > 0)
                {
                    manga.Genere = new List<Genere>();
                    foreach (var genereSelezione in manga.GenereTendinaSelezione)
                    {
                        Genere genere = db.Genere.Find(int.Parse(genereSelezione));
                        manga.Genere.Add(genere);
                    }
                }

                db.Manga.Add(manga);
                db.SaveChanges();
                TempData["messaggio"] = "Manga creato con successo";

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
            manga.CategoriaTendina = db.Categoria.ToList();
            manga.GenereTendina = db.Genere.ToList();
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
        public ActionResult Edit([Bind(Include = "ID,Titolo,Autore,AnnoUscita,Nazionalita,StatoPubblicazione,Copertina,Trama,UltimoVolume,Prezzo,CategoriaTendinaSelezione,GenereTendinaSelezione")] Manga manga)
        {
            if (ModelState.IsValid)
            {
                var mangaDaAggiornare = db.Manga
                    .Include(m => m.Categoria)
                    .Include(m => m.Genere)
                    .Where(m => m.ID == manga.ID)
                    .First();

                // 1. Elimino possibili categorie vecchie                   
                foreach (var categoriaRimuovere in mangaDaAggiornare.Categoria.ToList())
                {
                    mangaDaAggiornare.Categoria.Remove(categoriaRimuovere);
                }

                // 2. Inserisco nuove categorie scelte                    
                foreach (var categoriaSelezione in manga.CategoriaTendinaSelezione)
                {
                    Categoria categoria = db.Categoria.Find(int.Parse(categoriaSelezione));
                    mangaDaAggiornare.Categoria.Add(categoria);
                }
                 
                foreach (var genereRimuovere in mangaDaAggiornare.Genere.ToList())
                {
                    mangaDaAggiornare.Genere.Remove(genereRimuovere);
                }

                foreach (var genereSelezione in manga.GenereTendinaSelezione)
                {
                    int genereId = int.Parse(genereSelezione);

                    Genere genere = db.Genere
                        .Where(g => g.ID == genereId)
                        .SingleOrDefault();

                    mangaDaAggiornare.Genere.Add(genere);
                }

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
                //else                              DA CONTROLLARE 
                //{
                //    var mangaSalvato = db.Manga.Where(a => a.ID == manga.ID).First();
                //    var copertinaEsistente = mangaSalvato.Copertina;
                //    manga.Copertina = copertinaEsistente;
                //}

                mangaDaAggiornare.Titolo = manga.Titolo;
                mangaDaAggiornare.Autore = manga.Autore;
                mangaDaAggiornare.AnnoUscita = manga.AnnoUscita;
                mangaDaAggiornare.Nazionalita = manga.Nazionalita;
                mangaDaAggiornare.StatoPubblicazione = manga.StatoPubblicazione;
                mangaDaAggiornare.Copertina = manga.Copertina;
                mangaDaAggiornare.Trama = manga.Trama;
                mangaDaAggiornare.UltimoVolume = manga.UltimoVolume;
                mangaDaAggiornare.Prezzo = manga.Prezzo;

                db.SaveChanges();
                TempData["messaggio"] = "Manga modificato con successo";
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
            TempData["messaggio"] = "Manga eliminato con successo";
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

        public ActionResult Archivio(string sortOrder)
        {
            string sfondo = "archivio";
            ViewBag.Sfondo = sfondo;

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
                case "Prezzo":
                    manga = manga.OrderBy(m => m.Prezzo).ToList();
                    break;
                case "PrezzoDesc":
                    manga = manga.OrderByDescending(m => m.Prezzo).ToList();
                    break;
                default:
                    manga = manga.OrderBy(m => m.Titolo).ToList();
                    break;
            }
            return View(manga);
        }

        public ActionResult Ricerca()
        {
            string sfondo = "archivio";
            ViewBag.Sfondo = sfondo;

            return View();
        }

        [HttpPost]
        public ActionResult Ricerca(string titolo, string autore, string annoUscita, string nazionalita, string statoPubblicazione, string categoria, string genere)
        {
            string sfondo = "archivio";
            ViewBag.Sfondo = sfondo;

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
                query = query.Where(m => m.Nazionalita.Contains(nazionalita));
            }

            if (!string.IsNullOrEmpty(statoPubblicazione))
            {
                query = query.Where(m => m.StatoPubblicazione == statoPubblicazione);
            }

            //if (!string.IsNullOrEmpty(categoria))
            //{
            //    query = query.Where(m => m.CategoriaString.Contains(categoria));
            //}

            //if (!string.IsNullOrEmpty(genere))
            //{
            //    query = query.Where(m => m.GenereString == genere);
            //}

            var results = query.ToList();

            return View(results);
        }

    }
}
