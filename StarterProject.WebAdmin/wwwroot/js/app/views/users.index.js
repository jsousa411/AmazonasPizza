views.createNS("views.users.index");

views.users.index = (function () {

    var init = function () {
        $(document).ready(function () {
            views.app.initPage();

            mvcGrid = views.app.initMvcGrid({
                elementId: 'users-grid',
                onLoad
            });

            $('.grid-search').focus();
        });
    };

    var onLoad = function (rows) {
        rows.unbind('click');
        rows.click(function () {
            var row = $(this);
            var rowId = row.find('.collapse').text();
            if (event.target.tagName.toLowerCase() !== 'button' && event.target.tagName.toLowerCase() !== 'a') {
                views.app.navTo('/Users/Edit/' + rowId);
            }
        });
    };

    return {
        init
    };
})();