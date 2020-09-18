var table = null;
var arrData = [];

$(document).ready(function () {
    //table = $('#TopTraining').DataTable({
    //    "processing": true,
    //    "responsive": true,
    //    "pagination": true,
    //    "stateSave": true,
    //    "ajax": {
    //        url: "/Dashboard/LoadTop",
    //        type: "GET",
    //        dataType: "json",
    //        dataSrc: "",
    //    },
    //    "columns": [
    //        {
    //            render: function (data, type, row, meta) {
    //                //console.log(row);
    //                return meta.row + meta.settings._iDisplayStart + 1;
    //                //return meta.row + 1;
    //            }
    //        },
    //        { "data": "trainer" },
    //        { "data": "title" },
    //        { "data": "typeTraining" },
    //        {
    //            "data": "rate",
    //            'render': function (data, type, row) {
    //                return row.rate + ' Stars'
    //            }
    //        },
    //    ],

    //});

    $.ajax({
        url: "/Dashboard/LoadTop",
        type: "GET",
        dataType: "json",
        dataSrc: "",
        success: function (dataQuest) {
            //debugger;
            arrQuest = dataQuest;
            $.each(dataQuest, function (i, val) {
                //debugger;
                $('#TopTraining').append('<tr><td>' + val.trainer + '</td> <td>' + val.title + '</td> <td>' + val.typeTraining + '</td> <td>' + val.rate +' Stars</td> </tr>');
            });
        }
    });
});