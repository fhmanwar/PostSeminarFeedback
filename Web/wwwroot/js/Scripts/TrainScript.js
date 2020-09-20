var table = null;
var arrData = [];

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
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: '<i class="fas fa-file-pdf" data-placement="Bottom" data-toggle="tooltip" data-animation="false" title="PDF" ></i>',
                className: 'btn btn-danger btn-border',
                title: 'Training Schedule',
                filename: 'Training Schedule ' + moment(),
                exportOptions: {
                    //format: {
                    //    body: function (data, row, column, node) {
                    //        // Strip $ from salary column to make it numeric
                    //        //return column === 5 ? data.replace(/[$,]/g, '') : data;
                    //        return column === 2 ? data.replace(/[$,]/g, '') : data;
                    //    }
                    //},
                    columns: [0, 1, 2, 3, 4, 5, 6],
                    search: 'applied',
                    order: 'applied',
                    modifier: {
                        page: 'current',
                    },
                },
                customize: function (doc) {
                    //doc.content.splice(1, 0, {
                    //    margin: [0, 0, 0, 12],
                    //});
                    //debugger;
                    //doc.content[1].margin = [150, 0, 130, 0]  //left, top, right, bottom
                    var rowCount = doc.content[1].table.body.length;
                    for (i = 1; i < rowCount; i++) {
                        doc.content[1].table.body[i][2].alignment = 'center';
                    };
                    doc.content[1].table.body[0][0].text = 'No.';
                    //doc.content[1].table.body[0][2].text = 'Department';
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                'This is your left footer column',
                                {
                                    // This is the right column
                                    alignment: 'right',
                                    text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                }
                            ],
                            margin: [10, 0]
                        }
                    });
                }
            },
            {
                extend: 'excelHtml5',
                text: '<i class="fas fa-file-excel" data-placement="Bottom" data-toggle="tooltip" data-animation="false" title="Excel" ></i>',
                className: 'btn btn-success btn-border',
                title: 'Training Schedule',
                filename: 'Training Schedule ' + moment(),
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                    search: 'applied',
                    order: 'applied',
                    modifier: {
                        page: 'current',
                    },
                },
                customize: function (excel) {
                    debugger;
                    var sheet = excel.xl.worksheets['sheet1.xml'];
                    // jQuery selector to add a border
                    //$('col c[r*="10"]', sheet).attr('s', '25');
                    $('c[r=A2] t', sheet).text('No.');
                    //$('c[r=C2] t', sheet).text('Department');
                }
            },
            {
                extend: 'csvHtml5',
                text: '<i class="far fa-file-alt" data-placement="Bottom" data-toggle="tooltip" data-animation="false" title="CSV"></i>',
                className: 'btn btn-info btn-border',
                title: 'Training Schedule',
                filename: 'Training Schedule ' + moment(),
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                    search: 'applied',
                    order: 'applied',
                    modifier: {
                        page: 'current',
                    },
                },
                //customize: function (csv) {
                //    //debugger;
                //    var sheet = excel.xl.worksheets['sheet1.xml'];
                //    // jQuery selector to add a border
                //    //$('col c[r*="10"]', sheet).attr('s', '25');
                //    $('c[r=A2] t', sheet).text('No.');
                //    //$('c[r=C2] t', sheet).text('Department');
                //}
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print" data-placement="Bottom" data-toggle="tooltip" data-animation="false" title="Print"></i>',
                className: 'btn btn-secondary btn-border',
                title: 'Training Schedule',
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

function LoadData(element) {
    //debugger;
    if (arrData.length === 0) {
        if (element[0].name == 'TypeOption') {
            $.ajax({
                type: "Get",
                url: "/typetrain/LoadType",
                success: function (data) {
                    arrData = data;
                    renderData(element);
                }
            });
        } else if (element[0].name == 'UserOption') {
            $.ajax({
                type: "Get",
                url: "/trainer/GetAllTrainer",
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
    if (element[0].name == 'TypeOption') {
        $.each(arrData, function (i, val) {
            $option.append($('<option/>').val(val.id).text(val.name))
        });
    } else if (element[0].name == 'UserOption') {
        $.each(arrData, function (i, val) {
            $option.append($('<option/>').val(val.empId).text(val.employeeName))
        });
    }
    
}

LoadData($('#TypeOption'))
LoadData($('#UserOption'))

function GetById(id) {
    //debugger;
    $.ajax({
        url: "/training/GetById/",
        data: { Id: id }
    }).then((result) => {
        //debugger;
        $('#Id').val(result.id);
        $('#Title').val(result.title);

        var date = new Date(result.schedule);
        //var dateStart = date.getFullYear() + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + ("0" + date.getDate()).slice(-2);
        //console.log(dateStart);
        //console.log(moment(date).format('YYYY-MM-DD'));
        //console.log(moment(date).format('HH:mm'));
        $('#ScheduleDate').val(moment(date).format('YYYY-MM-DD'));
        $('#ScheduleTime').val(moment(date).format('HH:mm'));
        $('#Location').val(result.location);
        $('#Target').val(result.target);
        $('#UserOption').val(result.employee.empId);
        $('#TypeOption').val(result.type.id);
        $('#add').hide();
        $('#update').show();
        $('#myModal').modal('show');
    })
}

function Save() {
    //debugger;
    var Data = new Object();
    Data.Id = 0;
    Data.title = $('#Title').val();
    Data.schedule = $('#ScheduleDate').val() + 'T' + $('#ScheduleTime').val();
    Data.location = $('#Location').val();
    Data.target = $('#Target').val();
    Data.userId = $('#UserOption').val();
    Data.typeId = $('#TypeOption').val();
    $.ajax({
        type: 'POST',
        url: "/training/InsertOrUpdate/",
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
    Data.title = $('#Title').val();
    Data.schedule = $('#ScheduleDate').val() + 'T' + $('#ScheduleTime').val();
    Data.location = $('#Location').val();
    Data.target = $('#Target').val();
    Data.userId = $('#UserOption').val();
    Data.typeId = $('#TypeOption').val();
    $.ajax({
        type: 'POST',
        url: "/training/InsertOrUpdate/",
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