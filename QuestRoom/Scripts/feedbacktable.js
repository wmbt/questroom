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
        cache: false,
        method: "GET",
        type: "JSON",
        data: {
            messageId: id,
            status: button.hasClass("published") ? Published : Banned
        },
        success: function(data) {
            if (button.hasClass("published")) 
                row.find(".banned").prop("disabled", false);
            row.find(".status").text(data.Status);
            row.find(".processed").text(data.Processed);
        },
        error: function(data) {
            
        }
    });
    row.find(".published").prop("disabled", true);
    row.find(".banned").prop("disabled", true);
    event.preventDefault();
}
