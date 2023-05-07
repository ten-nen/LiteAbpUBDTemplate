
(function () {

    /* bs5 *********************************************/

    abp.bs5 = abp.bs5 || {};

    abp.bs5.draggable = function (selector) {
        $(selector).on("mousedown", function (mousedownEvt) {
            var $draggable = $(this);
            var x = mousedownEvt.pageX - $draggable.offset().left,
                y = mousedownEvt.pageY - $draggable.offset().top;
            $("body").on("mousemove.draggable", function (mousemoveEvt) {
                $draggable.closest(".modal-dialog").offset({
                    "left": mousemoveEvt.pageX - x,
                    "top": mousemoveEvt.pageY - y
                });
            });
            $("body").one("mouseup", function () {
                $("body").off("mousemove.draggable");
            });
            $draggable.closest(".modal").one("bs.modal.hide", function () {
                $("body").off("mousemove.draggable");
            });
        });
    }

    var messageHtml = '<div id="site-message" class="toast-container position-fixed top-0 end-0 p-3" ><div class="toast" role="alert" aria-live="assertive" aria-atomic="true"><div class="toast-header"><span class="rounded me-2"></span><strong class="me-auto">提示信息</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button></div><div class="toast-body"></div></div></div></div>';
    var confirmHtml = '<div class="modal modal-blur" id="site-confirm" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true"><div class="modal-dialog modal-sm" role="document"><div class="modal-content"><button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button><div class="modal-body text-center py-4"><h3 class="site-confirm-title"></h3><div class="text-muted site-confirm-content"></div></div><div class="modal-footer"><div class="w-100"><div class="row"><div class="col"><a href="#" class="btn w-100 site-confirm-cancle"></a></div><div class="col"><a href="#" class="btn btn-danger w-100 site-confirm-ok"></a></div></div></div></div></div></div></div>';
    var loadingHtml = '<div class="modal modal-blur" id="site-loading" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true"><div class="modal-dialog modal-dialog-centered modal-sm" role="document"><div class="spinner-border spinner-border text-muted" role="status" style="--tblr-spinner-width: 2rem;--tblr-spinner-height: 2rem;margin: 0 auto;"></div></div></div>';
    var dialogHtml = '<div class="row">';
    dialogHtml += messageHtml;
    dialogHtml += confirmHtml;
    dialogHtml += loadingHtml;
    dialogHtml += '</div>';

    $('body').append(dialogHtml);

    var $confrim = $('#site-confirm');
    var $message = $('#site-message');
    var $loading = $('#site-loading');

    /* NOTIFICATION *********************************************/

    var showNotification = function (type, message, title, options) {
        var svg = '<svg fill="#67c23a" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check-circle-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/></svg>';
        if (type == 'info')
            svg = '<svg fill="#909399" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-info-circle-fill" viewBox="0 0 16 16"><path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zm.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/></svg>';
        else if (type == 'warning')
            svg = '<svg fill="#e6a23c" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-exclamation-circle-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4zm.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2z"/></svg>';
        else if (type == 'error')
            svg = '<svg fill="#f56c6c" xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM5.354 4.646a.5.5 0 1 0-.708.708L7.293 8l-2.647 2.646a.5.5 0 0 0 .708.708L8 8.707l2.646 2.647a.5.5 0 0 0 .708-.708L8.707 8l2.647-2.646a.5.5 0 0 0-.708-.708L8 7.293 5.354 4.646z"/></svg>'
        $message.find('.toast-header strong').text(title || '提示信息');
        $message.find('.toast-body').text(message);
        $message.find('.rounded').html(svg);
        new bootstrap.Toast($message.children()[0]).show();
    };

    abp.notify.success = function (message, title, options) {
        showNotification('success', message, title, options);
    };

    abp.notify.info = function (message, title, options) {
        showNotification('info', message, title, options);
    };

    abp.notify.warn = function (message, title, options) {
        showNotification('warning', message, title, options);
    };

    abp.notify.error = function (message, title, options) {
        showNotification('error', message, title, options);
    };


    /* MESSAGE **************************************************/

    var showMessage = function (title, message, btnTx, btnCb) {
        $('#site-confirm .site-confirm-title').text(title || '提示信息');
        $('#site-confirm .site-confirm-content').text(message);

        $('#site-confirm .site-confirm-ok').text(btnTx || '确定').unbind('click').on('click', function () {
            $confrim.modal('hide');
            if ($.isFunction(btnCb))
                btnCb();
        });
        $('#site-confirm .site-confirm-cancle').parent().hide();
        $confrim.modal('show');
        abp.bs5.draggable('.modal-body');
    };

    abp.message.info = function (message, title) {
        return showMessage(message, title);
    };

    abp.message.success = function (message, title) {
        return showMessage(message, title);
    };

    abp.message.warn = function (message, title) {
        return showMessage(message, title);
    };

    abp.message.error = function (message, title) {
        return showMessage(message, title);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {

        var title = '您确定吗？';
        var closeOnEsc = null;
        if ($.isFunction(titleOrCallback)) {
            closeOnEsc = callback;
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            title = titleOrCallback;
        };

        $('#site-confirm .site-confirm-title').text(title);
        $('#site-confirm .site-confirm-content').text(message);

        $('#site-confirm .site-confirm-ok').text('确定').unbind('click').on('click', function () {
            $confrim.modal('hide');
            if ($.isFunction(callback))
                callback();
        });
        $('#site-confirm .site-confirm-cancle').text('取消').unbind('click').on('click', function () {
            $confrim.modal('hide');
            if ($.isFunction(closeOnEsc))
                closeOnEsc();
        });
        $('#site-confirm .site-confirm-cancle').parent().show();
        $confrim.modal('show');
        abp.bs5.draggable('.modal-body');
    };

    abp.bs5.loading = function (isshow) {
        if (!$loading.modal)
            return;
        if (isshow == undefined || isshow == true)
            $loading.modal('show');
        else
            $loading.modal('hide');
    }

    /* utils *********************************************/

    abp.utils = abp.utils || {};

    abp.utils.url = {
        getParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    }

    abp.utils.form = {
        setValues: function (formEl, values) {
            var keys = Object.keys(values);
            var elems = formEl.elements;
            for (var i = 0; i < elems.length; i++) {
                if (elems[i].name) {
                    var key = keys.filter(function (x) { return x.toLocaleLowerCase() == elems[i].name.toLocaleLowerCase() });
                    if (key.length > 0) {
                        switch (elems[i].type) {
                            case 'checkbox':
                            case 'radio':
                                elems[i].checked = Array.isArray(values[key[0]]) ? values[key[0]].indexOf(elems[i].value) != -1 : elems[i].value == values[key[0]] || elems[i].value == String(values[key[0]]);
                                break;
                            default:
                                $(elems[i]).val(values[key[0]]);
                        }
                    }
                }
            }
        },
        reset: function (formEl) {
            formEl.reset();
            $(formEl).removeClass('was-validated')
        }
    };

    abp.utils.inputEnter = function (selector, cb) {
        $(selector).on('keyup', function (e) {
            var event = e || window.event;
            var code = event.keyCode || event.which || event.charCode;
            if (code == 13 && typeof (cb) === 'function') {
                cb();
            }
        });
    }

    abp.utils.requestings = new Array();

    abp.utils.appendCookie = function (cookieName, cookieValue, expires, path) {
        // 获取当前的 cookie 字符串
        var currentCookies = document.cookie;

        // 创建新的 cookie 字符串
        var newCookie = cookieName + "=" + cookieValue;
        if (expires) {
            newCookie += "; expires=" + expires.toUTCString();
        }
        if (path) {
            newCookie += "; path=" + path;
        }

        // 将新 cookie 字符串添加到当前 cookie 字符串中
        var appendedCookies = currentCookies + '; ' + newCookie;

        // 更新当前的 cookie 字符串
        document.cookie = appendedCookies;
    }

    /* moment template*********************************************/
    if (window.moment && window.template) {
        moment.locale('zh-cn');
        template.defaults.imports.dateFormat = function (date, format) {
            return moment(date).format(format);
        };
    }

    /* ajax *********************************************/

    //abp.utils.setCookieValue('Abp.Localization.CultureName', 'zh-CN');

    abp.ajax.defaultOpts = {
        type: 'GET',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8'
    };

    abp.ajax.handleErrorStatusCode = function (status) {
        switch (status) {
            case 401:
                abp.message.confirm('登录状态失效，是否立即跳转登录', function () {
                    location.href = '/home/login?ReturnUrl=' + encodeURIComponent(document.location.pathname);
                });
                break;
            case 403:
                abp.notify.error('您不具有此操作权限..');
                break;
            case 404:
                abp.ajax.showError(abp.ajax.defaultError404);
                break;
            default:
                abp.ajax.showError(abp.ajax.defaultError);
                break;
        }
    };

    abp.ajax.showError = function (error) {
        if (error.details) {
            return abp.notify.error(error.details, error.message);
        } else {
            return abp.notify.error(error.message || abp.ajax.defaultError.message);
        }
    };

    $.ajaxSetup({
        beforeSend: function (xhr) {
            abp.utils.requestings.push(1);
            setTimeout(function () {
                if (abp.utils.requestings.length > 0 && $loading.is(':hidden'))
                    abp.bs5.loading();
            }, 500);
        },
        complete: function (xhr, status) {
            abp.utils.requestings.pop();
            if (abp.utils.requestings.length == 0) {
                abp.bs5.loading(false);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr, status, error)
            abp.utils.requestings.pop();
            abp.bs5.loading(false);
        }
    });
})();
