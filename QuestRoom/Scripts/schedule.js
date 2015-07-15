$(document).ready(function() {
    var input = $(".input-group.date");
    
    input.datepicker({
        autoclose: true,
        language: window.lang.split("-")[0],
        startDate: window.startDate,
        endDate: window.endDate
    });

    input.datepicker("setDate", window.selectedDate);
    input.datepicker().on("changeDate", onDateChanged);
});

function onDateChanged(e) {
    var date = e.format(0, "ddmmyy");
    window.location.href = "/" + date;
}
 