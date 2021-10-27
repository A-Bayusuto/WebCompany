var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("GetOrderList?status=inprocess");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("GetOrderList?status=pending");
        }
        else {
            if (url.includes("complete")) {
                loadDataTable("GetOrderList?status=completed");
            }
            else {
                if (url.includes("rejected")) {
                    loadDataTable("GetOrderList?status=rejected");
                }
                else {
                    loadDataTable("GetOrderList?status=all");
                }
            }
        }
    }
});

function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Customer/MyOrder/" + url
        },
        "columns": [
            { "data": "applicationId", "width": "10%" },
            { "data": "orderDate", "width": "10%" },
            { "data": "orderTotal", "width": "10%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "paymentStatus", "width": "10%" },
            { "data": "name", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },


        ]
    });

}



function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
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