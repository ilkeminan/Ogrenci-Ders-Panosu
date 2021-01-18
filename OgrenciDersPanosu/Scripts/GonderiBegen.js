function Like(btn, btnId, controllerName, begeniSayisi) {
    var bttnFromId = document.getElementById(btnId);
    var button = $(btn);
    var span = button.find("span");
    var ders_id = button.data("ders-id");
    var gonderi_id = button.data("gonderi-id");
    var liked = button.data("liked");
    var mode = button.data("liked-mode");
    if (!mode) {
        button.data("liked-mode", true);
        button.addClass("btn-success");
        button.removeClass("card_color");
        span.removeClass("fa-thumbs-o-up");
        span.addClass("fa-thumbs-up");
        if (liked == false) {
            begeniSayisi++;
        }
        bttnFromId.childNodes[1].nodeValue = " " + begeniSayisi.toString();

        $.ajax({
            method: "POST",
            url: "/" + controllerName + "/Derslik/Like/?dersId=" + ders_id + "&gonderiId=" + gonderi_id,
            data: {
                dersId: ders_id,
                gonderiId: gonderi_id
            }
        }).done(function (data) {

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
    else {
        button.data("liked-mode", false);
        button.addClass("card_color");
        button.removeClass("btn-success");
        span.removeClass("fa-thumbs-up");
        span.addClass("fa-thumbs-o-up");
        if (liked == true) {
            begeniSayisi--;
        }
        bttnFromId.childNodes[1].nodeValue = " " + begeniSayisi.toString();

        $.ajax({
            method: "POST",
            url: "/" + controllerName + "/Derslik/Unlike/?dersId=" + ders_id + "&gonderiId=" + gonderi_id,
            data: {
                dersId: ders_id,
                gonderiId: gonderi_id
            }
        }).done(function (data) {

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
}