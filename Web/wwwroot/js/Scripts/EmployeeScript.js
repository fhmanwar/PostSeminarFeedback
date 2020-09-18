var table = null;
var arrData = [];

$(document).ready(function () {
    ClearScreen();
    $('#TrainerOption').on('change', function () {
        //debugger;
        $('#cekTitle').show();
        var getid = $('#TrainerOption').val();
        $.ajax({
            url: "/employee/Loadtitle",
            type: "Get",
            data: { id: getid },
            success: function (data) {
                //debugger;
                //console.log(data);
                $('#TrainOption').empty();
                $('#TrainOption').append($('<option/>').val('0').text('-- Select --'));
                $.each(data, function (i, val) {
                    $('#TrainOption').append($('<option/>').val(val.trainingId).text(val.trainingTitle))
                });
            }
        });
        $('#TrainOption').on('change', function () {
            $('#cekschedule').show();
            var getidtraining = $('#TrainOption').val();
            $.ajax({
                url: "/training/GetById",
                type: "Get",
                data: { id: getidtraining },
                success: function (dataTrain) {
                    //debugger;
                    console.log(dataTrain.schedule);
                    var date = new Date(dataTrain.schedule);
                    console.log(moment(date).format('YYYY-MM-DD'));
                    $('#ScheduleDate').val(moment(date).format('YYYY-MM-DD'));
                }
            });
        });
        
    });
    
});

function ClearScreen() {
    $('#Id').val('');
    $('#Question').val('');
    $('#TrainerOption').val('0');
    $('#cekTitle').hide();
    $('#cekschedule').hide();
    $('#update').hide();
    $('#add').show();
}

function LoadData(element) {
    //debugger;
    if (arrData.length === 0) {
        if (element[0].id == 'TrainerOption') {
            $.ajax({
                type: "Get",
                url: "/employee/Loadtrainer",
                success: function (data) {
                    //debugger;
                    //console.log(data);
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
    $option.append($('<option/>').val('0').text('-- Select --'));
    if (element[0].id == 'TrainerOption') {
        $.each(arrData, function (i, val) {
            $option.append($('<option/>').val(val.id).text(val.name))
        });
    }
}

LoadData($('#TrainerOption'))

function rate() {
    var rating = document.querySelector('input[name="rating"]:checked').value;
    var star05 = document.querySelector('label[id="starhalf"]').title;
    var star1 = document.querySelector('label[id="star1"]').title;
    var star15 = document.querySelector('label[id="star1half"]').title;
    var star2 = document.querySelector('label[id="star2"]').title;
    var star25 = document.querySelector('label[id="star2half"]').title;
    var star3 = document.querySelector('label[id="star3"]').title;
    var star35 = document.querySelector('label[id="star3half"]').title;
    var star4 = document.querySelector('label[id="star4"]').title;
    var star45 = document.querySelector('label[id="star4half"]').title;
    var star5 = document.querySelector('label[id="star5"]').title;

    if (rating == 0.5) {
        console.log(star05);
        console.log(rating);
        alert("Success Your Rate = " + star05);
    } else if (rating == 1) {
        console.log(star1);
        console.log(rating);
        alert("Success Your Rate = " + star1);
    } else if (rating == 1.5) {
        console.log(star15);
        console.log(rating);
        alert("Success Your Rate = " + star15);
    } else if (rating == 2) {
        console.log(star2);
        console.log(rating);
        alert("Success Your Rate = " + star2);
    } else if (rating == 2.5) {
        console.log(star25);
        console.log(rating);
        alert("Success Your Rate = " + star25);
    } else if (rating == 3) {
        console.log(star3);
        console.log(rating);
        alert("Success Your Rate = " + star3);
    } else if (rating == 3.5) {
        console.log(star35);
        console.log(rating);
        alert("Success Your Rate = " + star35);
    } else if (rating == 4) {
        console.log(star4);
        console.log(rating);
        alert("Success Your Rate = " + star4);
    } else if (rating == 4.5) {
        console.log(star45);
        console.log(rating);
        alert("Success Your Rate = " + star45);
    } else if (rating == 5) {
        console.log(star5);
        console.log(rating);
        alert("Success Your Rate = " + star5);
    }


}