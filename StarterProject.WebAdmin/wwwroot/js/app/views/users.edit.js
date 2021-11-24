views.createNS("views.users.edit");

views.users.edit = (function () {

    var init = function () {
        $(document).ready(function () {
            views.app.initPage();
            views.app.syncPanelTitle('#panel-form', '#Name');
            subscribeBtnDeleteClick();
        });
    };

    var subscribeBtnDeleteClick = function () {
        $('.btn-delete').click(function () {
            var id = $('#Id').val().getNumbers();
            views.app.deleteRecord('/Users/Delete/' + id, function () {
                views.app.navTo('/Users/Index');
            });
        });
    };

    return {
        init
    };
})();