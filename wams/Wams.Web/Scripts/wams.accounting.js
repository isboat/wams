(function (wams) {
    'use strict';

    function drawBarChart(ele, data, chartLegend, opt) {

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
        var resetCanvas = function() {
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
        var ctx = $("#"+ele).get(0).getContext("2d");

        // This will get the first returned node in the jQuery collection.
        var myNewChart = new Chart(ctx);
        var myBarChart = myNewChart.Bar(chartData, options);

        $(chartLegend).html(myBarChart.generateLegend());
    }

    wams.accounting = {
        showTotalDuesGraph: function (year) {

            wams.ui.showBusy();

            $.ajax({
                url: wams.config.totalMonthlyDuesUrl + "?year=" + year,
                success: function(data) {
                    console.log(data);
                    $("#totalAmount").html("&#x20b5; " + data.TotalDuesAmount + " ghc");
                    $("#usersWithFullDues").html(data.UsersWithFullDues);
                    $("#usersWithNoDues").html(data.UsersWithNoDues);

                    drawBarChart("duesChart", data.AnnualDues, "#duesChartLegend", {
                        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= value %> ghc",
                        label: "Monthly dues total",
                        fillColor: "rgba(151,187,205,0.5)",
                        strokeColor: "rgba(151,187,205,0.8)",
                        highlightFill: "rgba(151,187,205,0.75)",
                        highlightStroke: "rgba(151,187,205,1)"
                    });
                    drawBarChart("monthlyDuePaidChart", data.AnnualMonthlyPaidUser, "#monthlyDuePaidLegend", {
                        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= value %> members",
                        label: "Number of member who paid each month",
                        fillColor: "rgba(200,200,200,0.5)",
                        strokeColor: "rgba(200,200,200,0.8)",
                        highlightFill: "rgba(200,200,200,0.75)",
                        highlightStroke: "rgba(151,187,205,1)"
                    });
                    wams.ui.hideBusy();
                },
                error: function(d) {
                    
                }
            });
        },

        showInvestmentData: function (){}
    }
})(wams);

