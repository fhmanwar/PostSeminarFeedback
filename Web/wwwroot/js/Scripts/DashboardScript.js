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
        url: "/trainer/Loadtrainer",
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


    $.ajax({
        url: "/Dashboard/LoadTop",
        type: "GET",
        dataType: "json",
        dataSrc: "",
        success: function (dataQuest) {
            //debugger;
            $.each(dataQuest, function (i, val) {
                //debugger;
                $('#TopTraining').append('<tr><td>' + val.trainer + '</td> <td>' + val.title + '</td> <td>' + val.typeTraining + '</td> <td>' + val.rate +' Stars</td> </tr>');
            });
        }
    });
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
            "category": "departmentName",
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

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("barChart", am4charts.XYChart);
    chart.scrollbarX = new am4core.Scrollbar();

    // Add data
    chart.dataSource.url = "/dashboard/LoadBar";

    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "departmentName";
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
    series.dataFields.valueY = "total";
    series.dataFields.categoryX = "departmentName";
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