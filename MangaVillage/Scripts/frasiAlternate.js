$(document).ready(function () {
    // Array di frasi
    var frasi = ["Il villaggio dove i manga prendono vita",
        "Il tuo riferimento per il mondo dei manga",
        "Il sito che ti fa sognare con le storie da sfogliare"];

    // Indice della frase corrente
    var indiceCorrente = 0;

    // Funzione per mostrare la frase corrente
    function mostraFrase() {
        // Imposta il testo dell'elemento con l'ID "frase"
        $("#frase").text(frasi[indiceCorrente]);

        // Incrementa l'indice
        indiceCorrente = (indiceCorrente + 1) % frasi.length;
    }

    // Mostra la frase iniziale
    mostraFrase();

    // Intervallo per l'alternanza delle frasi
    setInterval(mostraFrase, 3000);
});