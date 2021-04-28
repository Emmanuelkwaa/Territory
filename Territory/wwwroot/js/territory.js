var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Territory/GetAll"
        },
        "columns": [
            { "data": "title", "width": "5%" },
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "15%" },
            { "data": "street", "width": "20%" },
            { "data": "apartment", "width": "5%" },
            { "data": "city", "width": "5%" },
            { "data": "state", "width": "5%" },
            { "data": "zipcode", "width": "10%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Territory/Upsert/${data}" class="item-edit" style="cursor:pointer">
                                    <span class="material-icons"> create </span>
                                </a>
                            </div>
                           `;
                }, "width": "5%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a onclick=Delete("/Admin/Territory/Delete/${data}") class="item-delete" style="cursor:pointer">
                                    <span class="material-icons"> clear </span>
                                </a>
                            </div>
                           `;
                }, "width": "5%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}