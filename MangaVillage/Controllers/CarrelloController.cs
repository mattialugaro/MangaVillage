using MangaVillage.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MangaVillage.Controllers
{
    public class CarrelloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        // GET: Carrello
        public ActionResult Index()
        {
            var carrello = Session["carrello"] as Ordine;
            if (carrello == null)
            {
                TempData["messaggio"] = "Il carrello è vuoto.";
                return View(new Ordine());
            }
            else
            {
                carrello.TotaleCarrello = 0;
                foreach (var articolo in carrello.DettaglioOrdine)
                {
                    var PrezzoQuantita = articolo.Quantita * articolo.Manga.Prezzo;
                    carrello.TotaleCarrello += PrezzoQuantita;
                }
                return View(carrello);
            }
        }

        public ActionResult Delete(int idManga, int numeroVolume)
        {
            var carrello = Session["carrello"] as Ordine;
            if (carrello != null)
            {
                DettaglioOrdine dettaglioOrdine = null;
                foreach (var articolo in carrello.DettaglioOrdine)
                {
                    if (articolo.IDMangaFk == idManga && articolo.NumeroVolume == numeroVolume)
                    {
                        if(articolo.Quantita > 1)
                        {
                            articolo.Quantita--;
                            TempData["messaggio"] = "Rimosso un manga dal carrello con successo";
                        }
                        else
                        {
                            dettaglioOrdine = articolo;
                        }
                    }
                }
                if (dettaglioOrdine != null)
                {
                    carrello.DettaglioOrdine.Remove(dettaglioOrdine);
                    TempData["messaggio"] = "Rimosso il manga dal carrello con successo";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Confirm(Ordine confermaCarrello)
        {
            var carrello = Session["carrello"] as Ordine;
            if (carrello != null)
            {
                carrello.DataOrdine = DateTime.Now;
                carrello.IndirizzoConsegna = confermaCarrello.IndirizzoConsegna;
                carrello.Note = confermaCarrello.Note;
                carrello.Pagato = true;
                carrello.IDUtenteFk = Convert.ToInt32(Request.Cookies["ID"].Value);

                foreach (var articolo in carrello.DettaglioOrdine)
                {
                    articolo.Manga = null;
                }

                ICollection<DettaglioOrdine> dettaglioOrdine = carrello.DettaglioOrdine;

                carrello.DettaglioOrdine = null;
                carrello = db.Ordine.Add(carrello);
                TempData["messaggio"] = "Ordine creato con successo";
                db.SaveChanges();
                this.saveDettaglioOrdine(dettaglioOrdine, carrello.ID);
                Session["carrello"] = new Ordine();
            }
            else
            {
                TempData["messaggio"] = "Il carrello è vuoto.";
                return View(new Ordine());
            }

            return RedirectToAction("Archivio", "Manga");
        }

        public ActionResult SvuotaCarrello()
        {
            var carrello = Session["carrello"] as Ordine;
            if (carrello != null)
            {
                carrello.DettaglioOrdine.Clear();
                Session["carrello"] = carrello;
                TempData["messaggio"] = "Carrello svuotato con successo";
            }
            return RedirectToAction("Index");
        }

        public void saveDettaglioOrdine(ICollection<DettaglioOrdine> dettaglioOrdine, int id)
        {

            string connectionstring = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionstring);

            try
            {
                conn.Open();
                foreach (var articolo in dettaglioOrdine)
                {
                    string query = "INSERT INTO DettaglioOrdine(IDOrdineFk, IDMangaFk, NumeroVolume, Quantita) VALUES(@IDOrdineFk, @IDMangaFk, @NumeroVolume, @Quantita)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@IDOrdineFk", id);
                    cmd.Parameters.AddWithValue("@IDMangaFk", articolo.IDMangaFk);
                    cmd.Parameters.AddWithValue("@NumeroVolume", articolo.NumeroVolume);
                    cmd.Parameters.AddWithValue("@Quantita", articolo.Quantita);
                    cmd.ExecuteNonQuery();
                }
            
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}