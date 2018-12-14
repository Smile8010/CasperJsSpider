// JavaScript source code
; !function (root, factory) {
    root.Utils = factory(root);
}(this, function (window) {
    var utils = {
        emptyFn: function () { },
        post: function (url, params, successFn, options) {
            if (!options) options = { type: 'post' };
            else {
                options.type = 'post';
            }
            return this.jQueryAjax(url, params, successFn, options);
        },
        /*����jQuery��Ajax���� ���� �򵥷�װ*/
        get: function (url, params, successFn, options) {
            if (!jQuery) { return; }
            if (!options) options = {};
            var defaults = {
                context: this,
                data: params,
                dataType: 'json',
                error: this.emptyFn,
                success: successFn ? function (data, textStatus, jqXHR) {
                    console.log(this);
                    if (typeof options.dataType != 'undefined' && options.dataType != 'json') {
                        try { data = jQuery.parseJSON(data); } catch (ex) { }
                    }
                    //ִ�гɹ��ص�����
                    if (typeof options.async != 'undefined' && options.async == false) {
                        successFn.call(this, data, textStatus);
                    } else {
                        setTimeout(successFn.bind(this, data, textStatus), 10);
                    }

                } : this.emptyFn,
                type: 'get',
                url: url
            };
            jQuery.extend(defaults, options);
            return jQuery.ajax(defaults);
        }
    };

    return jQuery.extend(utils, {
        // ���ڣ���ԭ�����ڻ����ϣ�����days������Ĭ������1��
        //addDate: function (date, days) {
        //    if (days == undefined || days == '') {
        //        days = 1;
        //    }
        //    if (!(date instanceof Date)) {
        //        if (typeof date == 'string') {
        //            date = new Date(date);
        //        } else {
        //            date = new Date();
        //        }
        //    }

        //    date.setDate(date.getDate() + days);
        //    var month = date.getMonth() + 1;
        //    var day = date.getDate();
        //    return new Date(date.getFullYear(), month, day);
        //},
        //// �����·�/�����ʾ�������1λ��������ǰ�����'0'
        //getFormatDate: function (arg) {
        //    if (arg == undefined || arg == '') {
        //        return '';
        //    }

        //    var re = arg + '';
        //    if (re.length < 2) {
        //        re = '0' + re;
        //    }

        //    return re;
        //}
    });
});
