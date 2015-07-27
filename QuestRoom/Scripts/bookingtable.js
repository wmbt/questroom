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
        cache : false,
        method: "GET",
        type: "JSON",
        data: {
            bookingId: id,
            status: button.hasClass("confirm") ? Confirmed : Canceled
        },
        success: function(data) {
            if (button.hasClass("confirm")) {
                if (data.Changed) {
                    row.find(".cancel").prop("disabled", false);
                } else {
                    alert("Невозможно подтвердить бронирование, время уже занято");
                }
            } else {
                row.find(".confirm").prop("disabled", false);
            }

            row.find(".status").text(data.Status);
            row.find(".processed").text(data.Processed);
        },
        error: function(data) {
            
        }
    });
    row.find(".confirm").prop("disabled", true);
    row.find(".cancel").prop("disabled", true);
    event.preventDefault();
}
