(function(wams) {
    wams.accounting = {
        showTotalDuesGraph: function(year) {
            $.ajax({
                url: wams.config.totalMonthlyDuesUrl + "?year=" + year,
                success: function(data) {
                    console.log(data);
                },
                error: function(d) {
                    
                }
            });
        }
    }
})(wams);

