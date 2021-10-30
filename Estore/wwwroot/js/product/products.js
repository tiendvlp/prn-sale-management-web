var dataTable;

$(document).ready(function () {
    loadTable();
});

function loadTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "type": "GET",
            "url": "/Admin/Products/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "20%" },
            { "data": "categoryName", "width": "10%" },
            { "data": "price", "width": "11%" },
            { "data": "quantity", "width": "10%" },
            { "data": "weight", "width": "10%" },
            { "data": "unit", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    console.log("Delete product with id: " + data)
                    return `
                        <div class="text-center">
                            <a href="/Admin/Products/Update/${data}" class="btn btn-success" style="cursor:pointer;">
                                Update
                            </a>

                            <a onclick=Delete("/Admin/Products/Delete/${data}") class="btn btn-danger" style="cursor:pointer;">
                                Delete
                            </a>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ],
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        $.ajax({
            type: "DELETE",
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    $('#tblData').dataTable().api().ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        })
    });
}





