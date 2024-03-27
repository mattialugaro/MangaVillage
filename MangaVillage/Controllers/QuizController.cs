using MangaVillage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MangaVillage.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult Index()
        {
            // Crea un modello per le opzioni di difficolta'
            var model = new QuizViewModel();
            // Restituisce la vista con il modello
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Start(int difficolta)
        {
            // Genera le domande in base alla difficolta'
            var domande = GetDomande(difficolta);
            // Crea un modello per il quiz
            var quizModel = new QuizModel(domande);
            // Imposta il timer
            quizModel.StartTimer();
            // Restituisce la vista con il modello del quiz
            return View("Quiz", quizModel);
        }

        private List<DomandaQuiz> GetDomande(int difficolta)
        {
            throw new NotImplementedException();
        }
    }
}