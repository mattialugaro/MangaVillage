using MangaVillage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MangaVillage.Controllers
{
    public class CarrelloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        // GET: Carrello
        public ActionResult Index()
        {
            var carrello = Session["carrello"] as List<DettaglioOrdine>;
            if (carrello == null || !carrello.Any())
            {
                TempData["messaggio"] = "Il carrello è vuoto.";
                //return RedirectToAction("Menu", "Articolo");  DA CONTROLLARE
            }

            decimal totaleCarrello = 0;
            foreach (var articolo in carrello)
            {
                var dettagli = db.Manga.Where(m => m.ID == articolo.Manga.ID).First();
                articolo.Manga = dettagli;

                var PrezzoQuantita = articolo.Quantita * articolo.Manga.Prezzo;
                totaleCarrello += PrezzoQuantita ;
            }

            Ordine ordine = new Ordine();
            ordine.DettaglioOrdine = carrello;
            ordine.TotaleCarrello = totaleCarrello;
            return View(ordine);
        }

        public ActionResult Delete(int? id)
        {
            var carrello = Session["carrello"] as List<DettaglioOrdine>;
            if (carrello != null)
            {
                var rimuoviProdotto = carrello.FirstOrDefault(a => a.Manga.ID == id);
                if (rimuoviProdotto != null)
                {
                    if (rimuoviProdotto.Quantita > 1)
                    {
                        rimuoviProdotto.Quantita--;
                    }
                    else
                    {
                        carrello.Remove(rimuoviProdotto);
                    }
                }
            }

            return RedirectToAction("Index"); // DA CONTROLLARE
        }

        //public ActionResult AggiungiArticolo([Bind(Include = "IDOrdineFk,IDMangaFk,NumeroVolume,Quantita")] DettaglioOrdine dettaglioOrdine, int id)
        //{
        //    var carrello = Session["carrello"] as List<DettaglioOrdine>;
        //    if (carrello == null)
        //    {
        //        carrello = new List<DettaglioOrdine>();
        //        Session["carrello"] = carrello;
        //    }

        //    //var aggiungiProdotto = carrello.FirstOrDefault(a => a.IDMangaFk == id);
        //    //if (aggiungiProdotto != null && aggiungiProdotto.NumeroVolume == numeroVolume)
        //    //{
        //    //    aggiungiProdotto.Quantita++;
        //    //}
        //    //else
        //    //{
        //    //    aggiungiProdotto = new OrdineArticolo();
        //    //    aggiungiProdotto.IDArticolo = id;
        //    //    aggiungiProdotto.Quantita = 1;
        //    //    carrello.Add(aggiungiProdotto);
        //    //}

        //    return RedirectToAction("Details/" + id, "Manga");
        //}

        //public ActionResult AggiungiArticolo(int id, int numeroVolume)
        //{
        //    var carrello = Session["carrello"] as List<DettaglioOrdine>;
        //    if (carrello == null)
        //    {
        //        carrello = new List<DettaglioOrdine>();
        //        Session["carrello"] = carrello;
        //    }

        //    var aggiungiProdotto = carrello.FirstOrDefault(a => a.Manga.ID == id);
        //    if (aggiungiProdotto != null && aggiungiProdotto.NumeroVolume == numeroVolume)
        //    {
        //        aggiungiProdotto.Quantita++;
        //    }
        //    else
        //    {
        //        aggiungiProdotto = new DettaglioOrdine();
        //        aggiungiProdotto.IDMangaFk = id;
        //        aggiungiProdotto.NumeroVolume = numeroVolume;
        //        aggiungiProdotto.Quantita = 1;
        //        carrello.Add(aggiungiProdotto);
        //    }

        //    return RedirectToAction("Details/" + id, "Manga"); // DA CONTROLLARE
        //}

        [HttpPost]
        public ActionResult Confirm(Ordine confermaCarrello)
        {
            var IDUtente = db.Utente.FirstOrDefault(u => u.Username == User.Identity.Name).ID;

            var carrello = Session["carrello"] as List<DettaglioOrdine>;
            if (carrello != null && carrello.Any())
            {
                Ordine o = new Ordine();
                o.IndirizzoConsegna = confermaCarrello.IndirizzoConsegna;
                o.Note = confermaCarrello.Note;
                o.Pagato = false;
                o.IDUtenteFk = IDUtente;
                o.DataOrdine = DateTime.Now;

                db.Ordine.Add(o);
                db.SaveChanges();

                foreach (var articolo in carrello)
                {
                    DettaglioOrdine oa = new DettaglioOrdine();
                    oa.IDOrdineFk = o.ID;
                    oa.IDMangaFk = articolo.Manga.ID;
                    oa.Quantita = articolo.Quantita;
                    oa.NumeroVolume = articolo.NumeroVolume;
                    db.DettaglioOrdine.Add(oa);
                    db.SaveChanges();
                }
                carrello.Clear();

                TempData["messaggio"] = "Ordine creato con successo";
            }

            return RedirectToAction("Menu", "Articolo");   // DA CONTROLLARE
        }

        public ActionResult SvuotaCarrello()
        {
            var carrello = Session["carrello"] as List<DettaglioOrdine>;
            if (carrello != null)
            {
                carrello.Clear();
            }
            return RedirectToAction("Index");
        }
    }
}