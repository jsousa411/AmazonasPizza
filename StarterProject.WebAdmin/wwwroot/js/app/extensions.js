$(document).ajaxError(function myErrorHandler(event, xhr, ajaxOptions, thrownError) {

    if (ajaxErrorHandled) {
        ajaxErrorHandled = false;
    } else {
        if (xhr.status === 401) {
            window.location = ajaxOptions.url;
        } else if (xhr.status === 403) {
            views.app.confirmErro('Forbidden.');
        } else if (xhr.status === 409) {
            views.app.confirmErro(xhr.responseText);
        } else if (xhr.status === 404) {
            views.app.loading(true);
            var url = window.location.pathname;
            $.ajax({
                method: "POST",
                url: '/Home/PageNotFound',
                data: { url }
            }).done(function (html) {
                $('#main-content').html(html);
                views.app.initPage();
            }).always(function () {
                views.app.loading(false);
            });
        } else if (xhr.status === 500) {
            views.app.confirmErro('Internal server error. ' + xhr.responseText);
        } else {
            views.app.confirmErro(xhr.status + ' - ' + xhr.statusText);
        }
    }
});

(function ($) {
    $.each(['show', 'hide'], function (i, ev) {
        var el = $.fn[ev];
        $.fn[ev] = function () {
            this.trigger(ev);
            return el.apply(this, arguments);
        };
    });
})(jQuery);

Array.prototype.removeItem = function (item) {
    const index = this.indexOf(item);
    if (index > -1) {
        this.splice(index, 1);
    }
    return index;
}

String.prototype.getNumbers = function () {
    return this.replace(/\D/g, "");
}

Number.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
}

String.prototype.padLeft = function (n, str) {
    return Array(n - String(this).length + 1).join(str || '0') + this;
}

Number.prototype.formatMoney = function (c, d, t) {
    var n = this,
        c = isNaN(c = Math.abs(c)) ? 2 : c,
        d = d == undefined ? "." : d,
        t = t == undefined ? "," : t,
        s = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};


$.fn.select2.defaults.set('language', 'pt-BR');