var table = null;
var arrData = [];

$(document).ready(function () {
    $.ajax({
        url: "/account/Loaddata",
        type: "GET",
        dataType: "json",
        dataSrc: "",
        success: function (data) {
            //debugger;
            $('#countEmp').html(data.length);
        }
    });
    $.ajax({
        url: "/trainer/GetAllTrainer",
        type: "GET",
        dataType: "json",
        dataSrc: "",
        success: function (data) {
            //debugger;
            //console.log(data);
            //console.log(data.length);
            $('#countTrainer').html(data.length);
        }
    });
    $.ajax({
        url: "/training/LoadData",
        type: "GET",
        dataType: "json",
        dataSrc: "",
        success: function (data) {
            //debugger;
            $('#countSchedule').html(data.length);
        }
    });
    $.ajax({
        url: "/feedback/LoadData",
        type: "GET",
        dataType: "json",
        dataSrc: "",
        success: function (data) {
            //debugger;
            $('#countFeedb').html(data.length);
        }
    });

    table = $('#TopTraining').DataTable({
        "pageLength": 3,
        "processing": true,
        "responsive": true,
        "stateSave": true,
        "pagination": true,
        "paging": false,
        "searching": false,
        "ajax": {
            url: "/Dashboard/LoadTop",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columns": [
            {
                "data": "trainer",
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + meta.settings._iDisplayStart + 1;
                    //return meta.row + 1;
                }
            },
            {
                "sortable": false,
                "data": "trainer"
            },
            {
                "sortable": false,
                "data": "title"
            },
            {
                "sortable": false,
                "data": "typeTraining"
            },
            { "data": "rate" },
        ],

    });
    //$.ajax({
    //    url: "/Dashboard/LoadTop",
    //    type: "GET",
    //    dataType: "json",
    //    dataSrc: "",
    //    success: function (dataQuest) {
    //        //debugger;
    //        $.each(dataQuest, function (i, val) {
    //            //debugger;
    //            if (parseInt(i+1) > 5) {
    //                return false;
    //            }
    //            $('#TopTraining').append('<tr><td>' + val.trainer + '</td> <td>' + val.title + '</td> <td>' + val.typeTraining + '</td> <td>' + val.rate +' Stars</td> </tr>');
    //        });
    //    }
    //});
});

am4core.useTheme(am4themes_animated);

var Pie = am4core.createFromConfig({
    "innerRadius": "50%",

    "dataSource": {
        "url": "/dashboard/LoadPie",
        "parser": {
            "type": "JSONParser",
        },
        "reloadFrequency": 5000,
    },

    // Create series
    "series": [{
        "type": "PieSeries",
        "dataFields": {
            "value": "total",
            "category": "title",
        },
        "slices": {
            "cornerRadius": 10,
            "innerCornerRadius": 7
        },
        "hiddenState": {
            "properties": {
                // this creates initial animation
                "opacity": 1,
                "endAngle": -90,
                "startAngle": -90
            }
        },
        "children": [{
            "type": "Label",
            "forceCreate": true,
            "text": "Training",
            "horizontalCenter": "middle",
            "verticalCenter": "middle",
            "fontSize": 40
        }]
    }],

    // Add legend
    "legend": {},

}, "pieChart", am4charts.PieChart);

am4core.ready(function () {

    //// Themes begin
    //am4core.useTheme(am4themes_animated);
    //// Themes end

    // Create chart instance
    var chart = am4core.create("barChart", am4charts.XYChart);
    chart.scrollbarX = new am4core.Scrollbar();

    // Add data
    chart.dataSource.url = "/dashboard/LoadTopBar";

    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "title";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;
    categoryAxis.renderer.labels.template.horizontalCenter = "right";
    categoryAxis.renderer.labels.template.verticalCenter = "middle";
    categoryAxis.renderer.labels.template.rotation = 270;
    categoryAxis.tooltip.disabled = true;
    categoryAxis.renderer.minHeight = 110;

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minWidth = 50;

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.sequencedInterpolation = true;
    series.dataFields.valueY = "rate";
    series.dataFields.categoryX = "title";
    series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
    series.columns.template.strokeWidth = 0;

    series.tooltip.pointerOrientation = "vertical";

    series.columns.template.column.cornerRadiusTopLeft = 10;
    series.columns.template.column.cornerRadiusTopRight = 10;
    series.columns.template.column.fillOpacity = 0.8;

    // on hover, make corner radiuses bigger
    var hoverState = series.columns.template.column.states.create("hover");
    hoverState.properties.cornerRadiusTopLeft = 0;
    hoverState.properties.cornerRadiusTopRight = 0;
    hoverState.properties.fillOpacity = 1;

    series.columns.template.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    });

    // Cursor
    chart.cursor = new am4charts.XYCursor();

}); // end am4core.ready()

am4core.ready(function () {

    //// Themes begin
    //am4core.useTheme(am4themes_animated);
    //// Themes end

    var chart = am4core.create('groupbarChart', am4charts.XYChart)
    chart.scrollbarX = new am4core.Scrollbar();
    chart.colors.step = 2;

    // Add data
    chart.dataSource.url = "/dashboard/LoadBar";

    chart.legend = new am4charts.Legend()
    chart.legend.position = 'top'
    chart.legend.paddingBottom = 20
    chart.legend.labels.template.maxWidth = 95

    var xAxis = chart.xAxes.push(new am4charts.CategoryAxis())
    xAxis.dataFields.category = 'title'
    xAxis.renderer.cellStartLocation = 0.1
    xAxis.renderer.cellEndLocation = 0.9
    xAxis.renderer.grid.template.location = 0;

    var yAxis = chart.yAxes.push(new am4charts.ValueAxis());
    yAxis.min = 0;

    function createSeries(value, name) {
        var series = chart.series.push(new am4charts.ColumnSeries())
        series.dataFields.valueY = value
        series.dataFields.categoryX = 'title'
        series.name = name

        series.events.on("hidden", arrangeColumns);
        series.events.on("shown", arrangeColumns);

        var bullet = series.bullets.push(new am4charts.LabelBullet())
        bullet.interactionsEnabled = false
        bullet.dy = 30;
        bullet.label.text = '{valueY}'
        bullet.label.fill = am4core.color('#ffffff')

        return series;
    }

    createSeries('star1', 'Star 1');
    createSeries('star2', 'Star 2');
    createSeries('star3', 'Star 3');
    createSeries('star4', 'Star 4');
    createSeries('star5', 'Star 5');

    function arrangeColumns() {

        var series = chart.series.getIndex(0);

        var w = 1 - xAxis.renderer.cellStartLocation - (1 - xAxis.renderer.cellEndLocation);
        if (series.dataItems.length > 1) {
            var x0 = xAxis.getX(series.dataItems.getIndex(0), "categoryX");
            var x1 = xAxis.getX(series.dataItems.getIndex(1), "categoryX");
            var delta = ((x1 - x0) / chart.series.length) * w;
            if (am4core.isNumber(delta)) {
                var middle = chart.series.length / 2;

                var newIndex = 0;
                chart.series.each(function (series) {
                    if (!series.isHidden && !series.isHiding) {
                        series.dummyData = newIndex;
                        newIndex++;
                    }
                    else {
                        series.dummyData = chart.series.indexOf(series);
                    }
                })
                var visibleCount = newIndex;
                var newMiddle = visibleCount / 2;

                chart.series.each(function (series) {
                    var trueIndex = chart.series.indexOf(series);
                    var newIndex = series.dummyData;

                    var dx = (newIndex - trueIndex + middle - newMiddle) * delta

                    series.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                    series.bulletsContainer.animate({ property: "dx", to: dx }, series.interpolationDuration, series.interpolationEasing);
                })
            }
        }
    }

}); // end am4core.ready()