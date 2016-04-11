(function(wams) {
    wams.accounting = {
        showTotalDuesGraph: function(year) {
            $.ajax({
                url: wams.config.totalMonthlyDuesUrl + "?year=" + year,
                success: function(data) {
                    console.log(data);
                    $("#totalAmount").html("&#x20b5; " + data.TotalDuesAmount + " ghc");
                },
                error: function(d) {
                    
                }
            });
        }
    }
})(wams);

