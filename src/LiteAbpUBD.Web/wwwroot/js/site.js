
var site = (function () {
    var that = this;

    that.draggable = function (selector) {
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

    var messageHtml = '<div id="site-message" class="alert col-lg-3" style="z-index: 99999;position: fixed;top: 10%;left: 50%;word-break: break-all;transform: translate(-50%,-50%);text-overflow: ellipsis;white-space: nowrap;overflow: hidden;display:none;" role="alert"><span></span><button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" style="top: 0;position: fixed;right: 0;margin: 3px;"></button></div>';
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

    that.confirm = function (title, content, okTx, cancleTx, okCb, cancleCb) {
        $('#site-confirm .site-confirm-title').text(title || '提示信息');
        $('#site-confirm .site-confirm-content').text(content);
        $('#site-confirm .site-confirm-ok').text(okTx || '确定').unbind('click').on('click', function () {
            $confrim.modal('hide');
            if (typeof (okCb) === 'function')
                okCb();
        });
        $('#site-confirm .site-confirm-cancle').text(cancleTx || '取消').unbind('click').on('click', function () {
            $confrim.modal('hide');
            if (typeof (cancleCb) === 'function')
                cancleCb();
        });
        $confrim.modal('show');
        site.draggable('.modal-body');
    };
    that.message = function (msg, type, cb) {
        $message.removeClass(function (index, className) {
            return (className.match(/(^|\s)alert-\S+/g) || []).join(' ');
        });
        $message.addClass('alert-' + type);
        $message.find('span').text(msg);
        $message.fadeIn().fadeOut(2000, cb);
    };
    that.success = function (msg, cb) {
        this.message(msg, 'success', cb);
    }
    that.error = function (msg, cb) {
        this.message(msg, 'danger', cb);
    }
    that.loading = function (isshow) {
        if (isshow == undefined || isshow == true)
            $loading.modal('show');
        else
            $loading.modal('hide');
    }

    that.url = {
        getParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    }


    that.form = {
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

    that.partialViewToJson = function (html) {
        var r = { html: html, total: 0 };
        var matchs = r.html.match(/{{\d+}}/g);
        if (matchs && matchs.length > 0) {
            r.html = r.html.replace(matchs[0], "");
            try {
                r.total = parseInt(matchs[0].replace('{{', '').replace('}}', ''));
            } catch (e) { }
        }
        return r;
    };

    that.inputEnter = function (selector, cb) {
        $(selector).on('keyup', function (e) {
            var event = e || window.event;
            var code = event.keyCode || event.which || event.charCode;
            if (code == 13 && typeof (cb) === 'function') {
                cb();
            }
        });
    }

    that.requestings = new Array();


    $.ajaxSetup({
        beforeSend: function (xhr) {
            that.requestings.push(1);
            setTimeout(function () {
                if (that.requestings.length > 0 && $loading.is(':hidden'))
                    that.loading();
            }, 500);
        },
        complete: function (xhr, status) {
            that.requestings.pop();
            if (that.requestings.length == 0) {
                that.loading(false);
            }
        },
        error: function (xhr, status, error) {
            console.log(xhr, status, error)
            that.requestings.pop();
            that.loading(false);
            if (xhr.status == 401) {
                that.confirm('提示信息', '登录状态失效，是否立即跳转登录？', null, null, function () {
                    location.href = '/home/login?ReturnUrl=' + encodeURIComponent(document.location.pathname);
                });
                return;
            }
            if (xhr.status == 403) {
                that.error('您不具有此操作权限..');
                return;
            }
            if (xhr.responseJSON && xhr.responseJSON.error) {
                that.error(xhr.responseJSON.error.message);
                return;
            }
            that.error('服务器内部错误，请重新尝试..');
        }
    });

    return that;
})();
