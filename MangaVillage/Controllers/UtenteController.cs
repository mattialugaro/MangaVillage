using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

        // GET: Utente
        public ActionResult Index()     // solo admin
        {
            return View(db.Utente.ToList());
        }

        // GET: Utente/Create
        public ActionResult Create()    // possibile eliminare
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
                return RedirectToAction("Index");
            }

            return View(utente);
        }

        // GET: Utente/Edit/5
        public ActionResult Edit(int? id)   // cambio ruolo se necessario
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);

            var avatars = new List<string>();
            var files = Directory.GetFiles(Server.MapPath("~/Content/Avatar"));
            foreach (var file in files)
            {
                avatars.Add(Path.GetFileName(file));
            }
            utente.listaAvatars = avatars;

            if (utente == null)
            {
                return HttpNotFound();
            }
            return View(utente);
        }

        // POST: Utente/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Cognome,DataNascita,Email,Username,Password,Ruolo,Avatar")]string SelectedAvatar, Utente utente)
        {
            if (ModelState.IsValid)
            {
                utente.Avatar = SelectedAvatar;
                db.Entry(utente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utente);
        }

        // GET: Utente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);
            if (utente == null)
            {
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

            string connectionstring = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionstring);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Utente WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("Username", u.Username);
                cmd.Parameters.AddWithValue("Password", u.Password);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(u.Username, false);
                    Utente utente = db.Utente.FirstOrDefault(x => x.Username == u.Username && x.Password == u.Password);

                    if (utente == null)
                    {
                        // TODO: gestire errore di login, cioe', utente o pwd sbagliata
                    }

                    // string idUtente = Response.Cookies["ID"].Value;
                    Response.Cookies.Add(new HttpCookie("ID", utente.ID.ToString()));
                    Response.Cookies.Add(new HttpCookie("Avatar", utente.Avatar));

                    TempData["Message"] = "Login effettuato con successo";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    conn.Close();
                    TempData["Errore"] = "Nome utente o password non corretti.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Errore: " + ex.Message;
                //Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
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

            string connectionstring = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionstring);

            try
            {
                conn.Open();
                string query = "INSERT INTO Utente(Nome, Cognome, DataNascita, Email, Username, Password, Ruolo, Avatar) VALUES(@Nome, @Cognome, @DataNascita, @Email, @Username, @Password, @Ruolo, @Avatar)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", u.Nome);
                cmd.Parameters.AddWithValue("@Cognome", u.Cognome);
                cmd.Parameters.AddWithValue("@DataNascita", u.DataNascita);
                cmd.Parameters.AddWithValue("@Email", u.Email);
                cmd.Parameters.AddWithValue("@Username", u.Username);
                cmd.Parameters.AddWithValue("@Password", u.Password);
                cmd.Parameters.AddWithValue("@Ruolo", "Utente");
                cmd.Parameters.AddWithValue("@Avatar", "default.jpeg");
                cmd.ExecuteNonQuery();

                TempData["Message"] = "Registrazione effettuata con successo";
                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Errore: " + ex.Message;
                //Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["Message"] = "Logout effettuato con successo";
            return RedirectToAction("Index", "Home");
        }

        //public ActionResult GetAvatars()
        //{
        //    var avatars = new List<SelectListItem>();
        //    var files = Directory.GetFiles(Server.MapPath("~/Content/Avatar"));
        //    foreach (var file in files)
        //    {
        //        var fileName = Path.GetFileNameWithoutExtension(file);
        //        avatars.Add(new SelectListItem { Value = fileName, Text = fileName });
        //    }

        //    return Json(avatars, JsonRequestBehavior.AllowGet);
        //}

        // GET: Utente/Edit/5
        public ActionResult Profilo(int? id)   // cambio ruolo se necessario
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utente utente = db.Utente.Find(id);

            var avatars = new List<string>();
            var files = Directory.GetFiles(Server.MapPath("~/Content/Avatar"));
            foreach (var file in files)
            {
                avatars.Add(Path.GetFileName(file));
            }
            utente.listaAvatars = avatars;

            if (utente == null)
            {
                return HttpNotFound();
            }
            return View(utente);
        }

        // POST: Utente/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profilo([Bind(Include = "ID,Nome,Cognome,DataNascita,Email,Username,Password,Ruolo,Avatar")] string SelectedAvatar, Utente utente)
        {
            if (ModelState.IsValid)
            {
                utente.Avatar = SelectedAvatar;
                db.Entry(utente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utente);
        }
    }
}
