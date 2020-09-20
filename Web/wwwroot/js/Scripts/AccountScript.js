var table = null;
var tRole = null;
var arrData = [];

$(document).ready(function () {
    tRole = $('#role').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/role/LoadData",
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
            { "data": "name" },
            {
                "data": "createData",
                'render': function (jsonDate) {
                    var date = new Date(jsonDate);
                    return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH: mm');
                }
            },
            {
                "data": "updateDate",
                'render': function (jsonDate) {
                    var date = new Date(jsonDate);
                    if (date.getFullYear() != 0001) {
                        return moment(date).format('DD MMMM YYYY') + '<br> Time : ' + moment(date).format('HH:mm');
                    }
                    return "Not updated yet";
                }
            },
            {
                "sortable": false,
                "render": function (data, type, row, meta) {
                    //console.log(row);
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<div class="form-button-action">'
                        + '<button class="btn btn-link btn-lg btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetByRole(' + meta.row + ')" ><i class="fa fa-lg fa-edit"></i></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-link btn-lg btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return DelRole(' + meta.row + ')" ><i class="fa fa-lg fa-times"></i></button>'
                        + '</div>'
                }
            }
        ],
    });

    table = $('#account').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/account/LoadData",
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
            { "data": "nik" },
            {
                "render": function (data, type, row) {
                    return row.name + '<br />Email: ' + row.email 
                }
            },
            {
                "sortable": false,
                "data": "site"
            },
            { "data": "phone" },
            { "data": "address" },
            {
                "sortable": false,
                "data": "roleName"
            },
            {
                "sortable": false,
                "render": function (data, type, row, meta) {
                    //console.log(row);
                    $('[data-toggle="tooltip"]').tooltip();
                    return '<div class="form-button-action">'
                        + '<button class="btn btn-link btn-lg btn-warning" data-placement="left" data-toggle="tooltip" data-animation="false" title="Edit" onclick="return GetById(' + meta.row + ')" ><i class="fa fa-lg fa-edit"></i></button>'
                        + '&nbsp;'
                        + '<button class="btn btn-link btn-lg btn-danger" data-placement="right" data-toggle="tooltip" data-animation="false" title="Delete" onclick="return Delete(' + meta.row + ')" ><i class="fa fa-lg fa-times"></i></button>'
                        + '</div>'
                }
            }
        ],
        initComplete: function () {
            this.api().columns(6).every(function () {
                var column = this;
                var select = $('<select class="form-control"><option value="">ALL</option></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
            this.api().columns(3).every(function () {
                var column = this;
                var select = $('<select class="form-control"><option value="">ALL</option></select>')
                    .appendTo($(column.header()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });

    ClearScreen();
});

function ClearScreen() {
    $('#Id').val('');
    $('#NIK').val('');
    $('#Email').val('');
    $('#Name').val('');
    $('#Site').val('');
    $('#Phone').val('');
    $('#Address').val('');
    $('#RoleOption').val('');
    $('#update').hide();
    $('#add').show();

    $('#Role').val('');
    $('#updRole').hide();
    $('#addRole').show();
}

function LoadData(element) {
    //debugger;
    if (arrData.length === 0) {
        if (element[0].name == 'RoleOption') {
            $.ajax({
                type: "Get",
                url: "/role/LoadData",
                success: function (data) {
                    arrData = data;
                    renderData(element);
                }
            });
        } else if (element[0].name == 'UserOption') {
            $.ajax({
                type: "Get",
                url: "/user/LoadData",
                success: function (data) {
                    arrData = data;
                    renderData(element);
                }
            });
        }
    }
    else {
        renderData(element);
    }
}

function renderData(element) {
    //debugger;
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('-- Select --').hide());
    $.each(arrData, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name))
    });
}

LoadData($('#RoleOption'))

//user
function GetById(number) {
    //debugger;
    var getid = table.row(number).data().id;
    $.ajax({
        url: "/account/GetById/",
        data: { Id: getid }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#NIK').val(result.nik);
        $('#Email').val(result.email);
        $('#Name').val(result.name);
        $('#Site').val(result.site);
        $('#Phone').val(result.phone);
        $('#Address').val(result.address);
        $('#RoleOption').val(result.roleID);
        $('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Save() {
    if ($('#confirmPass').val() == $('#Pass').val()) {
        //debugger;
        var Data = new Object();
        //Data.Id = ;
        Data.NIK = $('#NIK').val();
        Data.Email = $('#Email').val();
        Data.Password = $('#Pass').val();
        Data.Name = $('#Name').val();
        Data.Site = $('#Site').val();
        Data.Phone = $('#Phone').val();
        Data.Address = $('#Address').val();
        Data.roleID = $('#RoleOption').val();
        $.ajax({
            type: 'POST',
            url: "/account/InsertOrUpdate/",
            cache: false,
            dataType: "JSON",
            data: Data
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
    } else {
        Swal.fire('Error', 'Password Not Same', 'error');
    }
}

function Update() {
    //debugger;
    var Data = new Object();
    Data.Id = $('#Id').val();
    Data.NIK = $('#NIK').val();
    Data.Email = $('#Email').val();
    if ($('#Pass').val() != null) {
        Data.Password = $('#Pass').val();
    }
    Data.Name = $('#Name').val();
    Data.Site = $('#Site').val();
    Data.Phone = $('#Phone').val();
    Data.Address = $('#Address').val();
    Data.roleID = $('#RoleOption').val();
    $.ajax({
        type: 'POST',
        url: "/account/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Data
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

function Delete(number) {
    var getId = table.row(number).data().id;
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
                url: "/account/Delete/",
                data: { id: getId }
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

//Role
function GetByRole(number) {
    //debugger;
    var getidRole = tRole.row(number).data().id;
    $.ajax({
        url: "/role/GetById/",
        data: { Id: getidRole }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#Role').val(result.name);
        $('#addRole').hide();
        $('#updRole').show();
        $('#roleModal').modal('show');
    })
}

function SaveRole() {
    //debugger;
    var Data = new Object();
    Data.Id = null;
    Data.Name = $('#Role').val();
    $.ajax({
        type: 'POST',
        url: "/role/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Data
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
            tRole.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function UpdRole() {
    //debugger;
    var Data = new Object();
    Data.Id = $('#Id').val();
    Data.Name = $('#Role').val();
    $.ajax({
        type: 'POST',
        url: "/role/InsertOrUpdate/",
        cache: false,
        dataType: "JSON",
        data: Data
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
            tRole.ajax.reload(null, false);
        } else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    })
}

function DelRole(number) {
    //debugger;
    var getRoleid = tRole.row(number).data().id;
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
                url: "/role/Delete/",
                data: { id: getRoleid }
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
                    tRole.ajax.reload(null, false);
                } else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            })
        };
    });
}