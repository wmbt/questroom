var Published = 1,
    Banned = 2;

$(document).ready(function () {
    $(".published").click(action);
    $(".banned").click(action);
});

function action(event) {
    var button = $(this),
        row = button.parent().parent(),
        id = row.prop("id");
    $.ajax({
        url: "/backend/setmessagestatus",
        method: "GET",
        type: "JSON",
        data: {
            messageId: id,
            status: button.hasClass("published") ? Published : Banned
        },
        success: function(data) {
            if (button.hasClass("published")) 
                $(".cancel").prop("banned", false);
            row.find(".status").text(data.Status);
            row.find(".processed").text(data.Processed);
        },
        error: function(data) {
            
        }
    });
    $(".published").prop("disabled", true);
    $(".banned").prop("disabled", true);
    event.preventDefault();
}
