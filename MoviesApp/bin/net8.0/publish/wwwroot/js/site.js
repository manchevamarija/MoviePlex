// site.js - MoviesApp

// Document ready
$(document).ready(function () {

    // Initialize all DataTables on the page
    $('table').each(function () {
        if (!$.fn.dataTable.isDataTable(this)) {
            $(this).DataTable({
                // Optional: customize DataTable options
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

    // Example: Auto-hide Toast/Alert after 3 seconds
    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 3000);

    // Optional: add custom JS here for MoviesApp interactivity
    console.log("MoviesApp site.js loaded");
});
