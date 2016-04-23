(function (wams) {
    'use strict';

    function displayTopTen(data) {
        var table = "<table class='table table-hover table-responsive'>";
        for (var i = 0; i < data.length; i++) {
            var mem = data[i];
            table += "<tr><td>" + mem.Name + "</td><td>&#x20b5;" + mem.Amount + " ghc</td></tr>";
        }

        table += "</table>";

        return table;
    }

    wams.accounting = {
        showTotalDuesGraph: function (year) {

            wams.ui.showBusy();

            $.ajax({
                url: wams.config.totalMonthlyDuesUrl + "?year=" + year,
                success: function(data) {
                    
                    $("#totalAmount").html("&#x20b5; " + data.TotalAmount + " ghc");
                    $("#usersWithFullDues").html(data.TotalUsersWith);
                    $("#usersWithNoDues").html(data.TotalUsersWithout);

                    wams.charts.drawBarChart("duesChart", data.AnnualChartData, "#duesChartLegend", {
                        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= value %> ghc",
                        label: "Monthly dues total",
                        fillColor: "rgba(151,187,205,0.5)",
                        strokeColor: "rgba(151,187,205,0.8)",
                        highlightFill: "rgba(151,187,205,0.75)",
                        highlightStroke: "rgba(151,187,205,1)"
                    });
                    wams.charts.drawBarChart("monthlyDuePaidChart", data.AnnualMonthlyPaidUser, "#monthlyDuePaidLegend", {
                        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= value %> members",
                        label: "Number of member who paid each month",
                        fillColor: "rgba(200,200,200,0.5)",
                        strokeColor: "rgba(200,200,200,0.8)",
                        highlightFill: "rgba(200,200,200,0.75)",
                        highlightStroke: "rgba(151,187,205,1)"
                    });
                    wams.ui.hideBusy();
                },
                error: function (d) {
                    alert("Error occured getting total dues data")
                    wams.ui.hideBusy();
                }
            });
        },

        showInvestmentData: function (year) {
            wams.ui.showBusy();

            $.ajax({
                url: wams.config.investmentDataUrl + "?year=" + year,
                success: function (data) {
                    
                    $("#totalAmount").html("&#x20b5; " + data.TotalAmount + " ghc");
                    $("#usersWith").html(data.TotalUsersWith);
                    $("#usersWithout").html(data.TotalUsersWithout);
                    $("#topTen").html(displayTopTen(data.TopTenHighestMembers));

                    wams.ui.hideBusy();
                },
                error: function (d) {
                    alert("Error occured getting total investment data")
                    wams.ui.hideBusy();
                }
            });

        }
    }
})(wams);

