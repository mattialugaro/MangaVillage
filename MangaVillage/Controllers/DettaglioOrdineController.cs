using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    [Authorize]
    public class DettaglioOrdineController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: DettaglioOrdine
        public ActionResult Index()
        {
            var dettaglioOrdine = db.DettaglioOrdine.Include(d => d.Manga).Include(d => d.Ordine).OrderBy(d => d.IDOrdineFk);
            if (User.IsInRole("Admin"))
            {
                return View(dettaglioOrdine.ToList());
            }
            else
            {
                return View(db.DettaglioOrdine.Include(d => d.Manga)
                    .Include(d => d.Ordine)
                    .Where(d => d.Ordine.Utente.Username == User.Identity.Name && d.Ordine.Pagato == true).ToList());
            }
        }
    }
}
