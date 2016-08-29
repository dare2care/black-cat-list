$(document).ready(function () {

    var $tables = $("table");

    $tables.DataTable({
        order: [],
        autoWidth: false,
        columnDefs: [
            { orderable: false, targets: 0 },
            { orderable: false, targets: 1 }
        ],
        fixedHeader: {
            headerOffset: 50
        },
        colReorder: true
    });

    $tables.removeClass("initializing");
}(jQuery));
