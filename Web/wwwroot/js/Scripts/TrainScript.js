var table = null;

$(document).ready(function () {
    table = $('#train').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/training/LoadData",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            {
                "data": "id",
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + meta.settings._iDisplayStart + 1;
                    //return meta.row + 1;
                }
            },
            { "data": "employee.name" },
            { "data": "title" },
            { "data": "type.name" },
            {
                "data": "schedule",
                'render': function (jsonDate) {
                    var date = new Date(jsonDate);
                    return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH: mm');
                }
            },
            { "data": "location" },
            { "data": "target" },
            {
                "sortable": false,
                "render": function (data, type, row) {
                    //console.log(row);
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<div class="form-button-action">'
                        + '<button class="btn btn-link btn-lg btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + row.id + ')" ><i class="fa fa-lg fa-edit"></i></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-link btn-lg btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + row.id + ')" ><i class="fa fa-lg fa-times"></i></button>'
                        + '</div>'
                }
            }
        ],

    });
});

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#update').hide();
    $('#add').show();
}

function GetById(id) {
    //debugger;
    $.ajax({
        url: "/training/GetById/",
        data: { Id: id }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#Type').val(result.name);
        $('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Save() {
    //debugger;
    var Type = new Object();
    Type.Id = 0;
    Type.Name = $('#Type').val();
    $.ajax({
        type: 'POST',
        url: "/training/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Type
    }).then((result) => {
        //debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data inserted Successfully',
                showConfirmButton: false,
                timer: 1500,
            })
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Update() {
    //debugger;
    var Type = new Object();
    Type.Id = $('#Id').val();
    Type.Name = $('#Type').val();
    $.ajax({
        type: 'POST',
        url: "/training/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Type
    }).then((result) => {
        //debugger;
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Data Updated Successfully',
                showConfirmButton: false,
                timer: 1500,
            });
            table.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!',
    }).then((resultSwal) => {
        if (resultSwal.value) {
            //debugger;
            $.ajax({
                url: "/training/Delete/",
                data: { id: id }
            }).then((result) => {
                //debugger;
                if (result.statusCode == 200) {
                    //debugger;
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Delete Successfully',
                        showConfirmButton: false,
                        timer: 1500,
                    });
                    table.ajax.reload(null, false);
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}