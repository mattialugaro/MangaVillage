using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MangaVillage;
using MangaVillage.Models;

namespace MangaVillage.Controllers
{
    public class UtenteController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

        // GET: Utente
        public ActionResult Index()
        {
            return View(db.Utente.ToList());
        }

        // GET: Utente/Create
        public ActionResult Create()
        {
            Utente utente = new Utente();
            var avatars = new List<string>();
            var files = Directory.GetFiles(Server.MapPath("~/Content/Avatar"));
            foreach (var file in files)
            {
                avatars.Add(Path.GetFileName(file));
            }
            utente.listaAvatars = avatars;

            return View(utente);
        }

        // POST: Utente/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Cognome,DataNascita,Email,Username,Password,Ruolo,Avatar")] string SelectedAvatar, Utente utente)
        {
            if (ModelState.IsValid)
            {
                utente.Avatar = SelectedAvatar;

                db.Utente.Add(utente);
                db.SaveChanges();
                TempData["messaggio"] = "Utente creato con successo";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["errore"] = "Errore creazione utente";
            }

            return View(utente);
        }

        // Metodo per la formattazione della data, con solo un numero aggiunge lo 0
        private string parseData(DateTime dataNascita)          
        {
            string result = dataNascita.Year.ToString() + "-" ;
            if(dataNascita.Month.ToString().Length == 1)
            {
                result += "0" + dataNascita.Month.ToString();
            }
            else
            {
                result += dataNascita.Month.ToString();
            }
            result += "-";
            if(dataNascita.Day.ToString().Length == 1)
            {
                result += "0" + dataNascita.Day.ToString();
            }
            else
            {
                result += dataNascita.Day.ToString();
            }
            return result;
        }

        // GET: Utente/Edit/5
        public ActionResult Edit(int? id)   // cambio ruolo se necessario
        {
            if (id == null)
            {
                TempData["errore"] = "Errore ID utente";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);
            utente.DataNascitaString = parseData(utente.DataNascita);
            var avatars = new List<string>();
            var files = Directory.GetFiles(Server.MapPath("~/Content/Avatar"));
            foreach (var file in files)
            {
                avatars.Add(Path.GetFileName(file));
            }
            utente.listaAvatars = avatars;

            if (utente == null)
            {
                TempData["errore"] = "Errore utente";
                return HttpNotFound();
            }
            return View(utente);
        }

        // POST: Utente/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Cognome,DataNascita,Email,Username,Password,Ruolo,Avatar")] string SelectedAvatar, Utente utente)
        {
            if (ModelState.IsValid)
            {
                utente.DataNascita = DateTime.Parse(utente.DataNascitaString);
                utente.Avatar = SelectedAvatar;
                utente.Password = HashPassword(utente.Password);
                if(User.Identity.Name == utente.Username)
                {
                    Request.Cookies.Remove("Avatar");
                    Response.Cookies.Add(new HttpCookie("Avatar", utente.Avatar));
                }

                db.Entry(utente).State = EntityState.Modified;
                db.SaveChanges();

                if (User.IsInRole("Admin"))
                {
                    TempData["messaggio"] = "Utente modificato con successo";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["messaggio"] = "Profilo modificato con successo";
                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }

        // GET: Utente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["errore"] = "Errore ID utente";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);
            if (utente == null)
            {
                TempData["errore"] = "Errore utente";
                return HttpNotFound();
            }
            return View(utente);
        }

        // POST: Utente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utente utente = db.Utente.Find(id);
            db.Utente.Remove(utente);
            db.SaveChanges();
            TempData["messaggio"] = "Utente eliminato con successo";
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

        [AllowAnonymous]
        public ActionResult Login()
        {
            string sfondo = "home";
            ViewBag.Sfondo = sfondo;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Utente u)
        {
            string sfondo = "home";
            ViewBag.Sfondo = sfondo;

            try
            {                
                Utente utente = db.Utente.FirstOrDefault(x => x.Username == u.Username);

                if (utente != null)
                {
                    var verifica = VerifyPasswordHash(u.Password, utente.Password);
                    if (!verifica)
                    {
                        TempData["errore"] = "Nome utente o password non corretti.";
                        return View();
                    }

                    FormsAuthentication.SetAuthCookie(u.Username, false);
                    Response.Cookies.Add(new HttpCookie("ID", utente.ID.ToString()));
                    Response.Cookies.Add(new HttpCookie("Avatar", utente.Avatar));

                    TempData["messaggio"] = "Accesso effettuato con successo";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["errore"] = "Nome utente o password non corretti.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errore"] = "Errore: " + ex.Message;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Registrazione()
        {
            string sfondo = "home";
            ViewBag.Sfondo = sfondo;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registrazione(Utente u)
        {
            string sfondo = "home";
            ViewBag.Sfondo = sfondo;

            try
            {
                // Hash the password before saving
                string hashPassword = HashPassword(u.Password);
                u.Password = hashPassword;

                // Set default role and avatar if not provided
                u.Ruolo = u.Ruolo ?? "Utente";
                u.Avatar = u.Avatar ?? "default.jpeg";

                // Add new user using Entity Framework
                db.Utente.Add(u);
                db.SaveChanges();

                TempData["messaggio"] = "Registrazione effettuata con successo";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["errore"] = "Errore: " + ex.Message;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["messaggio"] = "Logout effettuato con successo";
            return RedirectToAction("Index", "Home");
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        private bool UtenteExist(int id) // MEtodi controllo Utente DA GUARDARE
        {
            return db.Utente.Any(e => e.ID == id);
        }
    }
}
