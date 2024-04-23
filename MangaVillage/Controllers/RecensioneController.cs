using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RecensioneController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Recensione
        public ActionResult Index()
        {
            var recensione = db.Recensione.Include(r => r.Manga).Include(r => r.Utente);
            return View(recensione.ToList());
        }

        // GET: Recensione/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["messaggio"] = "Utente non trovato";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recensione recensione = db.Recensione.Find(id);
            if (recensione == null)
            {
                TempData["messaggio"] = "Errore eliminazione recensione";
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
            TempData["messaggio"] = "Recensione eliminata con successo";
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
