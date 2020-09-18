var table = null;
var arrData = [];
var arrQuest = [];
var arrCreate = [];

$(document).ready(function () {
    //debugger;

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

                    $('#QuestionP').show();
                    $('.myRate').show();
                    $.ajax({
                        url: "/Employee/LoadtrainerQuest/",
                        type: "Get",
                        data: { id: getid },
                        success: function (dataQuest) {
                            //debugger;
                            arrQuest = dataQuest;
                            $.each(dataQuest, function (i, val) {
                                //debugger;
                                console.log(i + 1);
                                console.log(val.questionDesc);
                                $('#dataQuest').append('<div class="form-group"><p class="form-control-static">' + parseInt(i + 1) + '. ' + val.questionDesc + '</p></div><div class= "row flex-row justify-content-center" ><div class="myRate"></div></div>');
                                $(".myRate").starRating({
                                    totalStars: 5,
                                    starShape: 'rounded',
                                    starSize: 40,
                                    emptyColor: 'lightgray',
                                    hoverColor: 'salmon',
                                    activeColor: 'crimson',
                                    useGradient: false,
                                    callback: function (currentRating, $el) {
                                        // make a server call here
                                        //console.log(currentRating);
                                        $('#dataQuest').append('<input type="text" id="getRate-' + parseInt(i + 1) +'" value="'+currentRating+'" class="form-control" hidden>');
                                    }
                                });
                            });
                        }
                    });
                }
            });

        });

    });
    //Stars();
});

function ClearScreen() {
    $('#Id').val('');
    $('#TrainerOption').val('0');
    $('#cekTitle').hide();
    $('#cekschedule').hide();
    $('.myRate').hide();

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

function Save() {
    //debugger;
    var count = arrQuest.length;
    console.log(count);
    $.each(arrQuest, function (i, data) {
        //debugger;
        //console.log(data);
        var Data = new Object();
        //Data.review = $('#Review').val();
        Data.rate = $('#getRate-' + parseInt(i + 1)).val();
        Data.questionId = data.questionId;
        //console.log(Data);
        //arrCreate.push(Data);
        //console.log(arrCreate);
        $.ajax({
            type: 'POST',
            url: "/employee/Insert/",
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
                window.setTimeout(function () {
                    location.reload();
                }, 1500);
            } else {
                Swal.fire('Error', 'Failed to Input', 'error');
                ClearScreen();
            }
        });
    })
}

function Stars() {
    /* 1. Visualizing things on Hover - See next part for action on click */
    $('#stars li').on('mouseover', function () {
        var onStar = parseInt($(this).data('value'), 10); // The star currently mouse on

        // Now highlight all the stars that's not after the current hovered star
        $(this).parent().children('li.star').each(function (e) {
            if (e < onStar) {
                $(this).addClass('hover');
            }
            else {
                $(this).removeClass('hover');
            }
        });

    }).on('mouseout', function () {
        $(this).parent().children('li.star').each(function (e) {
            $(this).removeClass('hover');
        });
    });


    /* 2. Action to perform on click */
    $('#stars li').on('click', function () {
        var onStar = parseInt($(this).data('value'), 10); // The star currently selected
        var stars = $(this).parent().children('li.star');

        for (i = 0; i < stars.length; i++) {
            $(stars[i]).removeClass('selected');
        }

        for (i = 0; i < onStar; i++) {
            $(stars[i]).addClass('selected');
        }

        // JUST RESPONSE (Not needed)
        var ratingValue = parseInt($('#stars li.selected').last().data('value'), 10);
    });
}

function rate() {
    //debugger;
    //var rate = $(".getRate").attr('name');
    var getRate = $("input[name=rating]:checked").val();
    console.log(getRate);
    //location.reload();
    var getRate = $("input[name=rating]:checked").prop('checked', false);
}