$(function () {
    FillTable();
    $("#add").on('click', function () {
        $(".modal-title").text("Add Category");
        $("#add-button").show();
        $("#edit-button").hide();
        $("#name").text("");
        $(".modal").modal();
    });
    $(".table").on("click", ".edit", function () {
        $(".modal-title").text("Edit Category");
        $("#add-button").hide();
        $("#edit-button").show();
        var name = $(this).data("category-name");
        $("#id").val($(this).data("category-id"));
        $("#name").val(name);
        $("#edit-button").data("category-id", $(this).data('category-id'));
        $("#edit-button").data("category-name", $(this).data('category-name'));
        $(".modal").modal();
    });
    $(".table").on("click", ".delete", function () {
        var id = $(this).data("category-id");
        $.post("/admin/deletecategory", { id: id }, function () {
            FillTable();
        });
    });
    $("#add-button").on("click", function () {
        var params = {
           name: $("#name").val()
        };
        $.post("/admin/addcategory", params, function () {
            FillTable();
            $(".modal").modal('hide');
        });
    });
    $("#edit-button").on("click", function () {
        var button = $(this);
        var params = {
            name: $("#name").val(),
            id: button.data('category-id')
        };
        console.log(params);
        $.post("/admin/editcategory", params, function () {
            FillTable();
            $(".modal").modal('hide');
        });
    });
    function FillTable() {
        $(".table tr:gt(0)").remove();
        $.get("/admin/getcategories", function (categories) {
            $.each(categories, function (index, c) {
                var text = `<tr><td>` + c.name + `</td><td> <button class="btn btn-success edit" data-category-name="` + c.name +
                    `" data-category-id="` + c.id + `">Rename</button>`;
                if (!c.hasApplications) {
                    text += `<button class="btn btn-danger delete" data-category-id="` + c.id + `">Delete</button>`;
                }
                text += `</td></tr>`;
                $(".table").append(text);
            });
        });
       
    }
});