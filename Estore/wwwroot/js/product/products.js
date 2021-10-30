var dataTable;

$(document).ready(function () {
    loadTable();
});
const delay = ms => new Promise(res => setTimeout(res, ms));
function loadTable() {
    let data = {};
    if (filter != null) {
        data = filter;
    }

    console.log(JSON.stringify(data))
    $.ajax({
        type: "POST",
        url: "/Admin/Products/GetAll",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                dataTable = $("#tblData").DataTable({
                    "bDestroy": true,
                    "searching": false,
                    'processing': true,
                    data: data.data,
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
                        },
                        {
                            "data": "id", "render": function (data) {
                                return `
                                    <div class="form-check">
                                      <input class="checkbox-product form-check-input" type="checkbox" value="${data}">
                                    </div>
                                `
                            }
                        }
                    ],
                });
                dataTable.searching = true;
                console.log(JSON.stringify(data.data))
            }
            else {
                console.log(JSON.stringify(data.data))
            }
        }
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
                    $("#tblData").dataTable().fnDestroy();
                    loadTable();
                }
                else {
                    toastr.error(data.message);
                }
            }
        })
    });
}





