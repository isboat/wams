(function(wams) {
    wams.accounting = {
        showTotalDuesGraph: function(year) {
            $.ajax({
                url: wams.config.totalMonthlyDuesUrl + "?year=" + year,
                success: function(data) {
                    console.log(data);
                    $("#totalAmount").html("&#x20b5; " + data.TotalDuesAmount + " ghc");


                    // Get context with jQuery - using jQuery's .get() method.
                    var ctx = $("#duesChart").get(0).getContext("2d");
                    // This will get the first returned node in the jQuery collection.
                    var myNewChart = new Chart(ctx);

                    var data = {
                        labels: ["January", "February", "March", "April", "May", "June", "July"],
                        datasets: [
                            {
                                label: "My Second dataset",
                                fillColor: "rgba(151,187,205,0.5)",
                                strokeColor: "rgba(151,187,205,0.8)",
                                highlightFill: "rgba(151,187,205,0.75)",
                                highlightStroke: "rgba(151,187,205,1)",
                                data: [28, 48, 40, 19, 86, 27, 90]
                            }
                        ]
                    };

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

                        //String - A legend template
                        legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

                    }

                    var myBarChart = myNewChart.Bar(data, options);
                },
                error: function(d) {
                    
                }
            });
        }


    }
})(wams);

