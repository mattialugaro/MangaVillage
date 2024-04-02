$(document).ready(function () {
    // Array di frasi
    var frasi = ["Il villaggio dove i manga prendono vita",
        "Il tuo riferimento per il mondo dei manga",
        "Il sito che ti fa sognare con le storie da sfogliare"];

    // Indice della frase corrente
    var indiceCorrente = 0;

    // Funzione per mostrare la frase corrente
    function mostraFrase() {                                          // FUNZIONA BASE
        // Imposta il testo dell'elemento con l'ID "frase"
        $("#frase").text(frasi[indiceCorrente]);

        // Incrementa l'indice
        indiceCorrente = (indiceCorrente + 1) % frasi.length;
    }

    // Mostra la frase iniziale
    mostraFrase();

    // Intervallo per l'alternanza delle frasi
    setInterval(mostraFrase, 4000); // 4 secondi
});




//function mostraFrase() {                                      // SCOMPARE TITOLO E NON FRASI
//    // Imposta il testo dell'elemento con l'ID "frase"
//    $("#frase").text(frasi[indiceCorrente]).fadeIn(300);

//    // Incrementa l'indice
//    indiceCorrente = (indiceCorrente + 1) % frasi.length;

//    // Dissolvenza in uscita la frase precedente
//    if (indiceCorrente > 0) {
//        $("#frase").prev().fadeOut(300);
//    }
//}

//function mostraFrase() {                                      // UGUALE A QUELLO BASE MA SENZA ANIMAZIONI
//    // Imposta il testo dell'elemento con l'ID "frase"
//    $("#frase").text(frasi[indiceCorrente]).animate({
//        top: "0px"
//    }, 300);

//    // Incrementa l'indice
//    indiceCorrente = (indiceCorrente + 1) % frasi.length;

//    // Scorri verso l'alto la frase precedente
//    if (indiceCorrente > 0) {
//        $("#frase").prev().animate({
//            top: "-100px"
//        }, 300);
//    }
//}

//function mostraFrase() {                                          // BASE FUNZIONA
//    // Imposta il testo dell'elemento con l'ID "frase"
//    $("#frase").text(frasi[indiceCorrente]);

//    // Incrementa l'indice
//    indiceCorrente = (indiceCorrente + 1) % frasi.length;
//}