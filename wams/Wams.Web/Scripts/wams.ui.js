(function(wams) {
    'use strict';
    
    wams.ui = {
        showBusy: function(ele) {
            var $busyEle = $(ele ? ele : 'body');
            $busyEle.prepend("<div class='loading' />");
        },

        hideBusy: function() {
            $('.loading').remove();
        }
    }

})(wams)