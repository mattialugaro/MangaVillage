function alternateChatAlignment() {
    var chatDivs = document.querySelectorAll('.col-9');
    var chatR = document.querySelectorAll('.right');
    for (var i = 0; i < chatDivs.length; i++) {
        if (i % 2 == 0) {
            chatDivs[i].classList.add('text-end');
        }
    }
    for (var i = 0; i < chatR.length; i++) {
        if (i % 2 == 0) {
            chatR[i].classList.add('ms-auto');
        }
    }
}
alternateChatAlignment();