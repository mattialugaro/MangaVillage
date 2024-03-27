using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace MangaVillage.Models
{
    public class DomandaQuiz
    {
        public int ID { get; set; }
        public string Domanda {  get; set; }
        public List<Risposta> Risposte { get; set; }
        public int RispostaCorrettaID { get; set; }
    }

    public class Risposta
    {
        public int ID { get; set; }
        public string Testo { get; set; }
    }
}