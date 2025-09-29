// site.js - MoviesApp

$(document).ready(function () {

    $('table').each(function () {
        if (!$.fn.dataTable.isDataTable(this)) {
            $(this).DataTable({
                paging: true,
                searching: true,
                ordering: true,
                info: true,
                language: {
                    search: "Пребарај:",
                    lengthMenu: "Прикажи _MENU_ записи",
                    info: "Прикажани _START_ до _END_ од _TOTAL_ записи",
                    paginate: {
                        first: "Прва",
                        last: "Последна",
                        next: "Следна",
                        previous: "Претходна"
                    }
                }
            });
        }
    });

    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 3000);

    console.log("MoviesApp site.js loaded");
});
