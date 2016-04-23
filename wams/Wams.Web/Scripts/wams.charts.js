﻿(function (wams) {
    'use strict';

    wams.charts = {
        drawBarChart: function (ele, data, chartLegend, opt) {

            var chartData = {
                labels: [],
                datasets: [
                    {
                        label: opt.label,
                        fillColor: opt.fillColor,
                        strokeColor: opt.strokeColor,
                        highlightFill: opt.highlightFill,
                        highlightStroke: opt.highlightStroke,
                        data: []
                    }
                ]
            };
            var resetCanvas = function () {
                var par = $("#" + ele).parent(".canvas-container");
                $("#" + ele).remove();
                par.prepend("<canvas id='" + ele + "' width='550' height='350'></canvas>");
            };

            for (var i = 0; i < data.length; i++) {
                chartData.labels.push(data[i].Key);
                chartData.datasets[0].data.push(data[i].Value);
            }

            var options = {
                //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                scaleBeginAtZero: true,

                //Boolean - Whether grid lines are shown across the chart
                scaleShowGridLines: true,

                //String - Colour of the grid lines
                scaleGridLineColor: "rgba(0,0,0,.05)",

                //Number - Width of the grid lines
                scaleGridLineWidth: 1,

                //Boolean - Whether to show horizontal lines (except X axis)
                scaleShowHorizontalLines: true,

                //Boolean - Whether to show vertical lines (except Y axis)
                scaleShowVerticalLines: true,

                //Boolean - If there is a stroke on each bar
                barShowStroke: true,

                //Number - Pixel width of the bar stroke
                barStrokeWidth: 2,

                //Number - Spacing between each of the X value sets
                barValueSpacing: 5,

                //Number - Spacing between data sets within X values
                barDatasetSpacing: 1,

                // String - Template string for single tooltips
                tooltipTemplate: opt.tooltipTemplate,

                //String - A legend template
                legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

            }

            resetCanvas();

            // Get context with jQuery - using jQuery's .get() method.
            var ctx = $("#" + ele).get(0).getContext("2d");

            // This will get the first returned node in the jQuery collection.
            var myNewChart = new Chart(ctx);
            var myBarChart = myNewChart.Bar(chartData, options);

            $(chartLegend).html(myBarChart.generateLegend());
        }
    };

})(wams);