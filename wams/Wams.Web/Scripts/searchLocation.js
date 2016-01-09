var destination = {
    search: function (term, callback) {
        if (term) {
            
            $.ajax({
                url: 'planner/SearchLocation',
                type: 'GET',
                data: { searchTerm: term }
            }).done(function (data) {

                if (callback) {
                    callback(data);
                }
            });

        }
    }
}