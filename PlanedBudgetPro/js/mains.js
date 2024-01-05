$(document).ready(function () {
     var calendar = $.calendars.instance('ethiopian', 'am');
    $('#popupDatepicker').calendarsPicker({
        dateFormat: 'mm-dd-yyyy',
        calendar: calendar
    });

   
    });