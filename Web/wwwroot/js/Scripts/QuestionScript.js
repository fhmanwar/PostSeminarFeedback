var table = null;
var arrData = [];

$(document).ready(function () {
    table = $('#quest').DataTable({
        "processing": true,
        "responsive": true,
        "pagination": true,
        "stateSave": true,
        "ajax": {
            url: "/Question/LoadData",
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
            {
                'render': function (data, type, row) {
                    return row.training.employee.name
                }
            },
            {
                'render': function (data, type, row) {
                    return row.training.title
                }
            },
            { "data": "questionDesc" },
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
    $('#Name').val('');
    $('#update').hide();
    $('#add').show();
}

function LoadData(element) {
    //debugger;
    if (arrData.length === 0) {
        if (element[0].name == 'TrainOption') {
            $.ajax({
                type: "Get",
                url: "/training/loaddata",
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
        $option.append($('<option/>').val(val.id).text(val.employee.name+' - '+val.title))
    });
}

LoadData($('#TrainOption'))

function GetById(id) {
    //debugger;
    $.ajax({
        url: "/Question/GetById/",
        data: { Id: id }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#Question').val(result.questionDesc);
        $('#TrainOption').val(result.trainingId);
        $('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Save() {
    //debugger;
    var Data = new Object();
    Data.Id = 0;
    Data.questionDesc = $('#Question').val();
    Data.trainingId = $('#TrainOption').val();
    $.ajax({
        type: 'POST',
        url: "/Question/InsertOrUpdate/",
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
}

function Update() {
    //debugger;
    var Data = new Object();
    Data.Id = $('#Id').val();
    Data.questionDesc = $('#Question').val();
    Data.trainingId = $('#TrainOption').val();
    $.ajax({
        type: 'POST',
        url: "/Question/InsertOrUpdate/",
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
                url: "/Question/Delete/",
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