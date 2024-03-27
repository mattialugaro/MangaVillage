using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MangaVillage.Models
{
    public class QuizViewModel
    {
        public Difficolta Difficolta { get; set; }
    }

    public enum Difficolta
    {
        Facile,
        Normale,
        Difficile
    }
}