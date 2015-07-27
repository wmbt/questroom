$(document).ready(function() {
    var input = $(".input-group.date");
    
    input.datepicker({
        autoclose: true,
        language: window.lang.split("-")[0],
        startDate: window.startDate,
        endDate: window.endDate
    });
    input.each(function() {
        $(this).datepicker("setDate", window.selectedDate);
    });

    input.datepicker().on("changeDate", onDateChanged);
    
});

function onDateChanged(e) {
    var date = e.format(0, "ddmmyy"),
        id = $(this).prop("id");
    /*window.location.href = "/" + date;*/

    $.ajax({
        method: "GET",
        cache: false,
        dataType: "html",
        url: "/booking/schedule",
        data: {
            
            questId: id,
            date: date
        },
        success: function(html) {
            $("#schedule" + id).replaceWith(html);
            //var title = $("#schedule" + id + "> input").val();
            //$("#title" + id).text(title);
        }
    });
}
 