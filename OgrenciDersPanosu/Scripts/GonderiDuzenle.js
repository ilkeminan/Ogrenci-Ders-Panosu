function Edit(btn, spanid, deleteButtonId, controllerName) {
    var button = $(btn);
    var ders_id = button.data("ders-id");
    var gonderi_id = button.data("gonderi-id");
    var mode = button.data("edit-mode");
    if (!mode) {
        button.data("edit-mode", true);
        button.removeClass("btn-warning");
        button.addClass("btn-success");
        var btnSpan = button.find("span");
        btnSpan.removeClass("fa-edit");
        btnSpan.addClass("fa-check");

        $(spanid).addClass("editable");
        $(spanid).attr("contenteditable", true);
        $(spanid).focus();

        document.getElementById(deleteButtonId).style.display = "none";
    }
    else {
        button.data("edit-mode", false);
        button.addClass("btn-warning");
        button.removeClass("btn-success");
        var btnSpan = button.find("span");
        btnSpan.addClass("fa-edit");
        btnSpan.removeClass("fa-check");

        $(spanid).removeClass("editable");
        $(spanid).attr("contenteditable", false);

        document.getElementById(deleteButtonId).style.display = "inline-block";

        var txt = $(spanid).text();

        $.ajax({
            method: "POST",
            url: "/" + controllerName + "/Derslik/Edit/?dersId=" + ders_id + "&gonderiId=" + gonderi_id + "&text=" + txt,
            data: {
                dersId: ders_id,
                gonderiId: gonderi_id,
                text: txt
            }
        }).done(function (data) {

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
}