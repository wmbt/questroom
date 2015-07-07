var Confirmed = 1,
    Canceled = 2;

$(document).ready(function () {
    $(".confirm").click(action);
    $(".cancel").click(action);
});

function action(event) {
    var button = $(this),
        row = button.parent().parent(),
        id = row.prop("id");
    $.ajax({
        url: "/backend/setbookingstatus",
        method: "GET",
        type: "JSON",
        data: {
            bookingId: id,
            status: button.hasClass("confirm") ? Confirmed : Canceled
        },
        success: function(data) {
            if (button.hasClass("confirm"))
                $(".cancel").prop("disabled", false);
            row.find(".status").text(data.Status);
            row.find(".processed").text(data.Processed);
        },
        error: function(data) {
            
        }
    });
    $(".confirm").prop("disabled", true);
    $(".cancel").prop("disabled", true);
    event.preventDefault();
}
