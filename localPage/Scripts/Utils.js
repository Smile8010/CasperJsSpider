// JavaScript source code
; !function (root, factory) {
    root.Utils = factory(root);
}(this, function (window) {
    return {
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
    };
});
