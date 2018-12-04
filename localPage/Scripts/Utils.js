// JavaScript source code
; !function (root, factory) {
    root.Utils = factory(root);
}(this, function (window) {
    return {
        // 日期，在原有日期基础上，增加days天数，默认增加1天
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
        //// 日期月份/天的显示，如果是1位数，则在前面加上'0'
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
    };
});
