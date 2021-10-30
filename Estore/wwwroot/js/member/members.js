﻿var dataTable;

$(document).ready(function () {
    loadTable();
});

function loadTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "type": "GET",
            "url": "/Admin/Members/GetAll"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "email", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "companyName", "width": "15%" },
            { "data": "country", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    console.log("Delete user with id: " + data)
                    return `
                        <div class="text-center">
                            <a href="/Admin/Members/Update/${data}" class="btn btn-success" style="cursor:pointer;">
                                Update
                            </a>

                            <a onclick=Delete("/Admin/Members/Delete/${data}") class="btn btn-danger" style="cursor:pointer;">
                                Delete
                            </a>
                        </div>
                    `;
                },
                "width": "25%"
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





