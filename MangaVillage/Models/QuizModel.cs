using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;


namespace MangaVillage.Models
{
    public class QuizModel
    {
        public List<DomandaQuiz> Domande { get; set; }
        public int TempoRimanente { get; set; }
        public int Punteggio { get; set; }

        private Timer _timer;

        public QuizModel(List<DomandaQuiz> domande)
        {
            Domande = domande;
            TempoRimanente = 60;
            Punteggio = 0;

            _timer = new Timer(OnTimerTick, null, 1000, 1000);
            //_timer.Start();
        }

        public void StartTimer()
        {
            //_timer.Start();
        }

        public void StopTimer()
        {
            //_timer.Stop();
        }

        private void OnTimerTick(object state)
        {
            TempoRimanente--;
            if (TempoRimanente == 0)
            {
                // fine quiz
            }
        }

        public void CalcolaPunteggio(int domandaId, int rispostaId)
        {
            var domanda = Domande.Find(d => d.ID == domandaId);
            if (domanda != null)
            {
                if (domanda.RispostaCorrettaID == rispostaId)
                {
                    Punteggio += 3;
                }
                else
                {
                    Punteggio += 0;
                }
            }
        }
    }
}