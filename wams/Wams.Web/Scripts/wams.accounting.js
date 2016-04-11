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
                },
                error: function(d) {
                    
                }
            });
        }


    }
})(wams);

