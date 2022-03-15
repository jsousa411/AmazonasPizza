var views = views || {};

views.createNS = function (namespace) {
    var nsparts = namespace.split(".");
    var parent = views;

    if (nsparts[0] === "views") {
        nsparts = nsparts.slice(1);
    }

    for (var i = 0; i < nsparts.length; i++) {
        var partname = nsparts[i];

        if (typeof parent[partname] === "undefined") {
            parent[partname] = {};
        }

        parent = parent[partname];
    }

    return parent;
};

views.createNS("views.app");

var ajaxErrorHandled = false;

views.app = (function () {

    var contentShell = $('#main-content');

    var initApp = function () {
        hideEmptyMenu();
        initPage();
    };

    var initPage = function () {
        $(document).ready(function () {
            ajaxLink();
            showTabsWithErrors();
            loadMasks();
            initJQueryValidateUnobstrusive();
            initSelect2();
            toUppercase();
            datePicker();
            iniICheck();
        });
    };

    $(document).ready(function () {
        assineOnLoadingAjaxForm();
    });

    var datePicker = function () {
        $('.datepicker').datepicker({
            format: 'dd/mm/yyyy',
            language: 'en-US',
            autoclose: true,
            todayBtn: true
        });
    }

    var assineOnLoadingAjaxForm = function () {
        var disable = false;
        var $loadingAjaxForm = $('#loading-ajax-form');
        $loadingAjaxForm.hide();
        $loadingAjaxForm.on('show', function () {
            loading(true);
            var $allForm = $('#main-content').find('form');
            disable = true;
            $allForm.submit(function () {
                return !disable;
            });
        });
        $loadingAjaxForm.on('hide', function () {
            loading(false);
            var $allForm = $('#main-content').find('form');
            disable = !disable;
        });
    };

    var initJQueryValidateUnobstrusive = function () {
        $("form").removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("form");
    }

    var iniICheck = function () {
        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green',
        });
    };

    var initSelect2 = function () {
        var select2 = $('.select2');
        select2.css('width', '100%');

        select2.each(function () {
            var placeholder = $(this).attr('placeholder');
            if (placeholder == undefined || placeholder == null) {
                placeholder = 'Select';
            }

            $(this).select2({
                theme: 'bootstrap4',
                language: "en-US",
                placeholder: placeholder,
                allowClear: $(this).find("option[value='']").length > 0
            });
        });

        $(document).on('focus', '.select2-selection.select2-selection--single', function (e) {
            var _select2 = $(this).closest(".select2-container").siblings('select:enabled')
            if (_select2.attr('readonly') !== 'readonly' && _select2.attr('disabled') !== 'disabled') {
                _select2.select2('open');
            }
        });

        $('select.select2').on('select2:closing', function (e) {
            $(e.target).data("select2").$selection.one('focus focusin', function (e) {
                e.stopPropagation();
            });
        }).on('select2:opening', function (e) {
            if ($(this).attr('readonly') == 'readonly' || $(this).attr('disabled') == 'disabled') {
                e.preventDefault();
            }
        });
    };

    var select2AddNewItem = function (select2Selector, onClick) {
        $(select2Selector).on('select2:open', function () {
            var a = $(this).data('select2');
            if (!$('.select2-link').length) {
                a.$results.parents('.select2-results')
                    .append('<div class="select2-link"><a>+ Add</a></div>')
                    .on('click', function (b) {
                        a.trigger('close');
                        if (onClick) {
                            onClick();
                        }
                    });
            }
        }).on('select2:opening', function (e) {
            if ($(this).attr('readonly') == 'readonly' || $(this).attr('disabled') == 'disabled') {
                e.preventDefault();
            }
        });;
    };

    var hideEmptyMenu = function () {
        //$('#js-nav-menu').find('ul:not(:has(li))').parent().remove();
    };

    var toUppercase = function () {
        $('.uppercase').on('input', function () {
            let p = this.selectionStart;
            this.value = this.value.toUpperCase();
            this.setSelectionRange(p, p);
        });
    }

    var navTo = function (url) {
        var newLink = $('<a>', {
            text: 'Loading...',
            href: url,
            class: 'ajax-link'
        }).appendTo('#main-content');
        ajaxLink();
        newLink.click();
        newLink.remove();
    };

    var deleteRecord = function (url, functionSuccess) {
        confirmYesNo("Confirm delete?", function () {
            $.ajax({
                method: "POST",
                url: url
            }).done(function () {
                showToast('Success', 'Record deleted successfully!', 'success');
                if (functionSuccess != undefined) {
                    functionSuccess();
                }
            }).fail(function () {
                confirmError("Error deleting.");
            });
        });
    };

    var fetchSelectAjax = function (select, url, ibge) {
        select.empty();
        select.append('<option value=""></option>');
        $.ajax({
            url: url
        }).done(function (data) {
            for (var i = 0; i < data.length; i++) {
                if (ibge !== undefined && ibge !== null && data[i].ibge === ibge) {
                    select.append('<option value="' + data[i].id + '" selected>' + data[i].text + '</option>');
                } else {
                    select.append('<option value="' + data[i].id + '">' + data[i].text + '</option>');
                }
            }
        });
    };

    var loadMasks = function () {

        //var SPMaskBehavior = function (val) {
        //    return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
        //};

        //var spOptions = {
        //    onKeyPress: function (val, e, field, options) {
        //        field.mask(SPMaskBehavior.apply({}, arguments), options);
        //    },
        //    clearIfNotMatch: true
        //};

        //$('.fixo').mask("(00) 0000-0000", { clearIfNotMatch: true });
        //$('.cel').mask("(00) 00000-0000", { clearIfNotMatch: true });
        //$('.fone').mask(SPMaskBehavior, spOptions);
        //$('.inteiro').mask("#", { reverse: true });
        //$('.decimal').mask("#.##0,00", { reverse: true });
        //$('.decimal1').mask("#.#0,0", { reverse: true });
        //$('.decimal4').mask("#.####0,0000", { reverse: true });
        //$('.date').mask("00/00/0000");
        //$('.diames').mask("00/00");
        //$('.time').mask("00:00");
        //$('.ano').mask("0000", { clearIfNotMatch: true });
    };

    var showTabsWithErrors = function () {

        var firstTabError = null;

        $('.field-validation-error').each(function () {
            var propId = $(this).closest('.tab-pane').prop('id');
            if (propId !== undefined && propId !== null && propId !== '') {
                if (firstTabError === null) {
                    firstTabError = propId;
                }
                $('a[href$="#' + propId + '"]').css('background-color', '#ffeeee').css('color', '#ff0000');
            }
        });

        if (firstTabError !== null) {
            $('.nav-tabs a[href="#' + firstTabError + '"]').tab('show');
        }
    };

    var ajaxLink = function () {
        $(".ajax-link").off('click');
        $(".ajax-link").on('click', function (e) {
            e.preventDefault();
            var url = $(this).attr('href');
            //if (!url.toLowerCase().endsWith('/controller/action')) {
            //    views.controller.action.desconecte();
            //}
            History.pushState(null, 'xFácil', url);
        });
    };

    var lastUrl = null;
    var initAjaxLink = function () {

        var History = window.History, State = History.getState();

        History.Adapter.bind(window, 'statechange', function () {
            State = History.getState();
            if (State.url === '') {
                return;
            }

            if (State.url == lastUrl) {
                return;
            }

            //views.someHub.stop();

            lastUrl = State.url;
            loadContentShell(State.url);
        });
    };

    var loadContentShell = function (url) {
        contentShell.showLoading();
        $.ajax({
            type: "GET",
            url: url,
            dataType: "html",
            success: function (data, status, xhr) {
                contentShell.hideLoading();
                contentShell.hide();
                contentShell.html(data);
                contentShell.fadeIn(350);
                ajaxLink();
            },
            error: function (xhr, status, error) {
                contentShell.hideLoading();
                if (xhr.status == 404) {
                    contentShell.html(xhr.responseText);
                } else if (xhr.status !== 401 && xhr.status !== 403) {
                    views.app.confirmOk("Error loading page.\n" + xhr.responseText);
                }
            }
        });
    };

    var reloadContentShell = function () {
        loadContentShell(window.location.pathname + window.location.search);
        $('.modal-backdrop').remove();
    }

    var loading = function (on) {
        if (on === false) {
            contentShell.hideLoading();
        } else {
            contentShell.showLoading();
        }
    };

    var loadingElement = function (element, on) {
        if (on) {
            $(element).showLoading();
        } else {
            $(element).hideLoading();
        }
    };

    var confirmError = function (html, onClickOk) {
        var div = document.createElement("div");
        div.innerHTML = html;
        swal({
            title: "Oops...",
            content: div,
            icon: 'error'
        }).then((result) => {
            if (result) {
                if (onClickOk) onClickOk();
            }
        });
        initPage();
    };

    var confirmYesNo = function (html, onClickYes, onClickNo) {
        var div = document.createElement("div");
        div.innerHTML = html;
        swal({
            title: "Warning!",
            content: div,
            icon: 'warning',
            buttons: {
                sim: {
                    text: "Yes",
                    value: true,
                    className: 'btn-primary'
                },
                nao: {
                    text: "No",
                    value: false,
                    className: 'btn-danger'
                }
            }
        }).then((result) => {
            if (result) {
                if (onClickYes) onClickYes();
            } else {
                if (onClickNo) onClickNo();
            }
        });
        initPage();
    };

    var confirmOk = function (html, onClickOk, icon) {
        var div = document.createElement("div");
        div.innerHTML = html;
        swal({
            title: "Attention!",
            content: div,
            icon: icon
        }).then((result) => {
            if (result) {
                if (onClickOk) onClickOk();
            }
        });
        initPage();
    };

    var resetFields = function (element) {
        element.find(':input')
            .not(':button, :submit, :reset, :hidden')
            .val('')
            .prop('checked', false)
            .prop('selected', false)
            .trigger('change');
    };

    var disableInputs = function (element) {
        element.find(':input')
            .not(':button, :submit, :reset') //, :hidden
            .attr('disabled', 'disabled');
    };

    var showToast = function (titulo, mensagem, tipo) {
        var msg = mensagem;
        var title = titulo;

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": 300,
            "hideDuration": 100,
            "timeOut": 5000,
            "extendedTimeOut": 1000,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut",
            "onclick": null
        };

        toastr[tipo](msg, title);
    };

    var showToastSuccess = function (msg) {
        showToast('Success', msg, 'success');
    };

    var showToastDanger = function (msg) {
        showToast('Error', msg, 'error');
    };

    var showToastWarning = function (msg) {
        showToast('Warning', msg, 'warning');
    };

    var showToastInfo = function (msg) {
        showToast('Information', msg, 'info');
    };

    var syncPanelTitle = function (panelId, fieldId) {
        $(panelId).find(fieldId).keyup(function () {
            $(panelId).find('.ibox-title > h5').text($(this).val());
        });
    };

    var formatDuration = function (ms) {
        return new Date(ms).toISOString().slice(11, -1);
    };

    var uploadAjaxProgress = function (formData, url, onProgress, onSuccess) {
        $.ajax({
            url: url,
            type: 'POST',
            contentType: false,
            cache: false,
            processData: false,
            data: formData,
            xhr: function () {
                var jqXHR = null;
                if (window.ActiveXObject) {
                    jqXHR = new window.ActiveXObject("Microsoft.XMLHTTP");
                }
                else {
                    jqXHR = new window.XMLHttpRequest();
                }

                //Upload progress
                jqXHR.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        var percentComplete = Math.round((evt.loaded * 100) / evt.total);

                        if (onProgress != undefined) {
                            onProgress(percentComplete);
                        }
                    }
                }, false);

                //Download progress
                jqXHR.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        //var percentComplete = Math.round((evt.loaded * 100) / evt.total);
                        //console.log('Downloaded percent', percentComplete);
                    }
                }, false);

                return jqXHR;
            },
            success: function (data) {
                if (onSuccess != undefined) {
                    onSuccess(data);
                }
            },
            error: function (request, status, error) {
                confirmError(request.responseText);
            }
        });
    };

    var formatDate = function (date) {
        if (date != undefined && date != null && date != '') {
            return moment(date).format('MM/DD/YYYY');
        }
        else {
            return '';
        }
    };

    var formatDateTime = function (date) {
        if (date != undefined && date != null && date != '') {
            return moment(date).format('MM/DD/YYYY HH:MM:SS');
        }
        else {
            return '';
        }
    };

    var formataCurrency = function (valor) {
        if (valor) {
            return '$' + ' ' + parseFloat(valor.toString()).formatMoney(2, ',', '.');
        } else {
            return "";
        }
    };

    var formatDateMonthYear = function (date) {
        if (date != undefined && date != null && date != '') {
            return moment(date).format('MM/YYYY');
        }
        else {
            return '';
        }
    };

    var formatBytes = function (bytes, decimals) {
        if (bytes == 0) return '0 Bytes';
        var k = 1000,
            dm = decimals + 1 || 3,
            sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'],
            i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
    };

    var guid = function () {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            s4() + '-' + s4() + s4() + s4();
    };

    var rowClickFunction;

    var initMvcGrid = function (params) {
        var elementId = params.elementId;
        var onLoad = params.onLoad;
        var getFilters = params.getFilters;

        var divElement = document.getElementById(elementId);
        var element = $('#' + elementId).find('.mvc-grid')[0];
        var grid = new MvcGrid(element);

        var reloadEvent = function (e) {
            var $element = $(e.detail.grid.element);
            var rows = $element.find('tbody').find('tr').not('.mvc-grid-empty-row');

            if (onLoad) {
                onLoad(rows);
            }
        };

        divElement.removeEventListener('reloadend', reloadEvent);
        divElement.addEventListener('reloadend', reloadEvent);

        var typingTimer;
        var doneTypingInterval = 1000;

        $('#' + elementId).on('keyup', '.grid-search', function () {
            clearTimeout(typingTimer);
            typingTimer = setTimeout(doneTyping, doneTypingInterval);
        });

        function doneTyping() {
            search();
        }

        var search = function () {
            var grid = new MvcGrid(element);
            grid.url.searchParams = [];
            grid.url.searchParams.set("search", $('#' + elementId).find('.grid-search').val());

            if (getFilters != undefined && getFilters != null) {

                var filters = getFilters();
                if (filters != undefined && filters != null) {
                    $.each(filters, function (index, filter) {
                        grid.url.searchParams.set(filter.field, filter.value);
                    });
                }
            };

            grid.reload();
        };

        $('.content-refresh').on('click', function () {
            var grid = new MvcGrid(element);
            grid.reload();
        });

        var reload = function () {
            var grid = new MvcGrid(element);
            grid.reload();
            grid = new MvcGrid(element);
            grid.reload();
        };

        return {
            search,
            reload
        };
    };

    initAjaxLink();

    return {
        initApp,
        initPage,
        loadMasks,
        ajaxLink,
        navTo,
        select2AddNewItem,
        deleteRecord,
        fetchSelectAjax,
        loading,
        loadingElement,
        reloadContentShell,
        confirmYesNo,
        confirmOk,
        confirmError,
        resetFields,
        disableInputs,
        showToast,
        showToastSuccess,
        showToastDanger,
        showToastWarning,
        showToastInfo,
        syncPanelTitle,
        formatDuration,
        uploadAjaxProgress,
        formatDate,
        formatDateTime,
        formataCurrency,
        formatDateMonthYear,
        formatBytes,
        guid,
        initMvcGrid
    };
})();

moment.locale('en-US');

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
views.createNS("views.home.index");

views.home.index = (function () {

    var init = function () {
        $(document).ready(function () {
            views.app.initPage();
            initChart();
        });
    };

    var initChart = function () {
            
        var data2 = [
            [gd(2012, 1, 1), 7], [gd(2012, 1, 2), 6], [gd(2012, 1, 3), 4], [gd(2012, 1, 4), 8],
            [gd(2012, 1, 5), 9], [gd(2012, 1, 6), 7], [gd(2012, 1, 7), 5], [gd(2012, 1, 8), 4],
            [gd(2012, 1, 9), 7], [gd(2012, 1, 10), 8], [gd(2012, 1, 11), 9], [gd(2012, 1, 12), 6],
            [gd(2012, 1, 13), 4], [gd(2012, 1, 14), 5], [gd(2012, 1, 15), 11], [gd(2012, 1, 16), 8],
            [gd(2012, 1, 17), 8], [gd(2012, 1, 18), 11], [gd(2012, 1, 19), 11], [gd(2012, 1, 20), 6],
            [gd(2012, 1, 21), 6], [gd(2012, 1, 22), 8], [gd(2012, 1, 23), 11], [gd(2012, 1, 24), 13],
            [gd(2012, 1, 25), 7], [gd(2012, 1, 26), 9], [gd(2012, 1, 27), 9], [gd(2012, 1, 28), 8],
            [gd(2012, 1, 29), 5], [gd(2012, 1, 30), 8], [gd(2012, 1, 31), 25]
        ];

        var data3 = [
            [gd(2012, 1, 1), 800], [gd(2012, 1, 2), 500], [gd(2012, 1, 3), 600], [gd(2012, 1, 4), 700],
            [gd(2012, 1, 5), 500], [gd(2012, 1, 6), 456], [gd(2012, 1, 7), 800], [gd(2012, 1, 8), 589],
            [gd(2012, 1, 9), 467], [gd(2012, 1, 10), 876], [gd(2012, 1, 11), 689], [gd(2012, 1, 12), 700],
            [gd(2012, 1, 13), 500], [gd(2012, 1, 14), 600], [gd(2012, 1, 15), 700], [gd(2012, 1, 16), 786],
            [gd(2012, 1, 17), 345], [gd(2012, 1, 18), 888], [gd(2012, 1, 19), 888], [gd(2012, 1, 20), 888],
            [gd(2012, 1, 21), 987], [gd(2012, 1, 22), 444], [gd(2012, 1, 23), 999], [gd(2012, 1, 24), 567],
            [gd(2012, 1, 25), 786], [gd(2012, 1, 26), 666], [gd(2012, 1, 27), 888], [gd(2012, 1, 28), 900],
            [gd(2012, 1, 29), 178], [gd(2012, 1, 30), 555], [gd(2012, 1, 31), 993]
        ];


        var dataset = [
            {
                label: "Number of orders",
                data: data3,
                color: "#1ab394",
                bars: {
                    show: true,
                    align: "center",
                    barWidth: 24 * 60 * 60 * 600,
                    lineWidth: 0
                }

            }, {
                label: "Payments",
                data: data2,
                yaxis: 2,
                color: "#1C84C6",
                lines: {
                    lineWidth: 1,
                    show: true,
                    fill: true,
                    fillColor: {
                        colors: [{
                            opacity: 0.2
                        }, {
                            opacity: 0.4
                        }]
                    }
                },
                splines: {
                    show: false,
                    tension: 0.6,
                    lineWidth: 1,
                    fill: 0.1
                },
            }
        ];


        var options = {
            xaxis: {
                mode: "time",
                tickSize: [3, "day"],
                tickLength: 0,
                axisLabel: "Date",
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Arial',
                axisLabelPadding: 10,
                color: "#d5d5d5"
            },
            yaxes: [{
                position: "left",
                max: 1070,
                color: "#d5d5d5",
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: 'Arial',
                axisLabelPadding: 3
            }, {
                position: "right",
                clolor: "#d5d5d5",
                axisLabelUseCanvas: true,
                axisLabelFontSizePixels: 12,
                axisLabelFontFamily: ' Arial',
                axisLabelPadding: 67
            }
            ],
            legend: {
                noColumns: 1,
                labelBoxBorderColor: "#000000",
                position: "nw"
            },
            grid: {
                hoverable: false,
                borderWidth: 0
            }
        };

        function gd(year, month, day) {
            return new Date(year, month - 1, day).getTime();
        }

        var previousPoint = null, previousLabel = null;

        $.plot($("#flot-dashboard-chart"), dataset, options);

    };

    return {
        init
    };
})();
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