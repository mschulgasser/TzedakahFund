$(() => {
    FillTable();
    $(".table").on("click", ".accept", function () {
        var id = $(this).data('application-id');
        console.log(id);
        $.post("/admin/acceptapplication", { id: id }, function () {
            FillTable();
        });
    });
    $(".table").on("click", ".reject", function () {
        var id = $(this).data('application-id');
        console.log(id);
        $.post("/admin/rejectapplication", { id: id }, function () {
            FillTable();
        });
    });
    $('#category-select').on('change', function () {
        var filterBy = $(this).val();
        if (filterBy === 'all') {
            $(".table tr").show();
        }
        else {
            $('.table').find('tr').each(function (index, row) {
                var category = $(row).find('td:eq(2)').text();
                if (!category) return;
                if (category !== filterBy) {
                    $(row).hide();
                }
                else {
                    $(row).show();
                }
            });
        }
    });
});
function FillTable() {
    $(".table tr:gt(0)").remove();
    $.get("/admin/getpending", function (pending) {
        $.each(pending, function (index, p) {
            var text = `<tr><td>` + p.firstName + " " + p.lastName + `</td><td><a href="/Admin/ViewHistory?email=` +
                p.email + `">` + p.email + `</a></td><td>` + p.categoryName +  `</td><td>` + p.amount + `</td><td>` +
                `<button class="btn btn-success accept" data-application-id="` + p.id + `">Accept</button>` +
                `<button class="btn btn-danger reject" data-application-id="` + p.id + `">Reject</button></td></tr>`;
            $(".table").append(text);
        });
    });
}

