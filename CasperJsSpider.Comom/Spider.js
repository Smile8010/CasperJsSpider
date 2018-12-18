"use strict";
phantom.outputEncoding = "gbk";
var casper = require('casper').create({
    pageSettings: {
        userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36',
        loadImages: false,        // The WebPage instance used by Casper will
        loadPlugins: false         // use these settings
    },
    timeout: 30000,
    waitTimeout: 5000,
    onWaitTimeout: function () {
        this.exit();
    }
}), startLink
    , ProductList = []
    , xLoadDiv = { type: 'xpath', path: '//div[@id="zg_nonCritical"]/div' },
    clickSelector = [2, 3, 4, 5];

if (!casper.cli.has(0)) {
    casper.exit();
}

casper.on('resource.requested', function (reqData, req) {
    if (/facebook|google|twitter|linkedin/.test(reqData.url)) {
        req.abort();
    }
});

startLink = casper.cli.get(0);
casper.start(startLink);

function addStep() {
    casper.waitForSelector(xLoadDiv, function () {
        //pushArray(getProducts.call(this));
        //waitForSelector.call(this);
        var array = this.evaluate(function () {
            var domList = document.querySelectorAll('#zg_left_col1 .zg_itemRow');
            var findArray = [], i, len, tempDom, nameDom, priceDom, imgDom, linkDom, rankDom
                , firstDivDom
                , metadata;
            for (i = 0, len = domList.length; i < len; i++) {
                tempDom = domList[i];
                firstDivDom = tempDom.querySelector('.p13n-asin');
                if (!firstDivDom) { continue; }
                metadata = JSON.parse(firstDivDom.getAttribute('data-p13n-asin-metadata'));
                nameDom = tempDom.querySelector('.p13n-sc-truncated');
                priceDom = tempDom.querySelector('.p13n-sc-price');
                //imgDom = tempDom.querySelector('.a-thumbnail-left');
                imgDom = tempDom.querySelector('img');
                linkDom = tempDom.querySelector('.a-link-normal');
                rankDom = tempDom.querySelector('.zg_rankNumber');
                findArray.push({
                    ImgPath: imgDom ? imgDom.getAttribute('src') : '',
                    RankNumber: rankDom ? parseInt(rankDom.innerHTML.replace('.', '')) : 0,
                    Name: nameDom ? (nameDom.getAttribute('title') || nameDom.innerHTML) : '',
                    Url: linkDom ? (location.origin + decodeURIComponent(linkDom.getAttribute('href'))) : '',
                    Price: priceDom ? priceDom.innerHTML : '',
                    Ref: metadata.ref,
                    Asin: metadata.asin
                });
                //tempDom.parentNode.removeChild(tempDom);
            }

            document.querySelector('#zg_nonCritical').innerHTML = '';

            return findArray;
        }) || [];

        for (var i = 0, len = array.length; i < len; i++) {
            ProductList.push(array[i]);
        }

        var currentPage;
        if (clickSelector.length > 0
            && (
                currentPage = clickSelector.shift(),
                this.evaluate(function (cpage) {
                    return document.querySelector('a[page="' + cpage + '"]') != null;
                }, currentPage)
            )
        ) {
            casper.click({ type: 'xpath', path: '//a[@page="' + currentPage + '"]' });
            addStep();
        }
        else {
            casper.echo('※' + JSON.stringify(ProductList));
        }
    }, function timeout() {
        this.exit();
    });
}

addStep();
casper.run(function () {
    this.exit();
});


//"use strict";
//var casper = require('casper').create({
//    pageSettings: {
//        userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36',
//        loadImages: false,        // The WebPage instance used by Casper will
//        loadPlugins: false         // use these settings
//    }
//}), startLink, targetObj = {
//    CatalogName: '',
//    RID: '',
//    Products: [],
//    CatalogLink: ''
//},
//    xLoadDiv = { type: 'xpath', path: '//div[@id="zg_nonCritical"]/div' },
//    clickSelector = [2,3,4,5];//["#zg_page2", "#zg_page3", "#zg_page4", "#zg_page5"];
//phantom.outputEncoding = "gbk";

//if (!casper.cli.has(0)) {
//    casper.exit();
//}
////casper.echo('※' + casper.cli.get(0));


////casper.start("http://www.baidu.com", function () {
////    this.echo('※哈哈哈！');
////});
////casper.run()

//// 避免加载被墙的资源
//casper.on('resource.requested', function (reqData, req) {
//    if (/facebook|google|twitter|linkedin/.test(reqData.url)) {
//        req.abort();
//    }
//})

//startLink = casper.cli.get(0);
//targetObj.CatalogLink = startLink;
//casper.start(startLink, function () {
//    //ue_id
//    var obj = this.evaluate(function () {
//        return {
//            CatalogName: document.querySelector('.category').innerHTML
//        }
//    });

//    targetObj.CatalogName = obj.CatalogName;
//    targetObj.RID = obj.RID;
//});

////casper.waitFor(function check() {
////    return this.evaluate(function () {
////        return document.querySelectorAll('#zg_left_col1 .zg_itemRow').length > 0
////    });
////}, function then() {
////    var targetObj = {
////        CatalogName: '',
////        Products: [],
////        CatalogLink: startLink
////    }, clickSelector = ["#zg_page2", "#zg_page3", "#zg_page4","#zg_page5"];

////    targetObj.CatalogName = this.evaluate(function () {
////        return document.querySelector('.category').innerHTML;
////    });

////    pushArray(targetObj.Products, getProducts.call(this));    

////    for (var i = 0, len = clickSelector.length; i < len; i++) {
////        this.click(clickSelector[i]);
////        this.wait(5000, function () {
////            pushArray(targetObj.Products, getProducts.call(this));  
////        }); 
////    }



////    this.echo('※' + JSON.stringify(targetObj));
////});



////function waitForSelector() {
////    if (clickSelector.length > 0) {
////        //this.then(function () {
////        //    this.click(clickSelector.unshift());
////        //});

////        this.evaluate(function (selector) {
////            document.querySelector(selector).click();
////        }, clickSelector.unshift());

////        waitFor();
////    } else {
////        casper.echo('※' + JSON.stringify(targetObj));
////    }
////}

//function addStep() {
//    casper.waitForSelector(xLoadDiv, function () {
//        //pushArray(getProducts.call(this));
//        //waitForSelector.call(this);
//        var array = this.evaluate(function (CatalogLink) {
//            var domList = document.querySelectorAll('#zg_left_col1 .zg_itemRow');
//            var findArray = [], i, len, tempDom, nameDom, priceDom, imgDom, linkDom, rankDom
//                , firstDivDom
//                , metadata;
//            for (i = 0, len = domList.length; i < len; i++) {
//                tempDom = domList[i];
//                firstDivDom = tempDom.querySelector('.p13n-asin');
//                if (!firstDivDom) { continue; }
//                metadata = JSON.parse(firstDivDom.getAttribute('data-p13n-asin-metadata'));
//                nameDom = tempDom.querySelector('.p13n-sc-truncated');
//                priceDom = tempDom.querySelector('.p13n-sc-price');
//                imgDom = tempDom.querySelector('.a-thumbnail-left');
//                linkDom = tempDom.querySelector('.a-link-normal');
//                rankDom = tempDom.querySelector('.zg_rankNumber');
//                findArray.push({
//                    ImgPath: imgDom ? imgDom.getAttribute('src'):'',
//                    RankNumber: rankDom?parseInt(rankDom.innerHTML.replace('.', '')):0,
//                    Name: nameDom?(nameDom.getAttribute('title') || nameDom.innerHTML):'',
//                    Link: linkDom?( location.origin + decodeURIComponent(linkDom.getAttribute('href'))):'',
//                    Price: priceDom ? priceDom.innerHTML : '',
//                    Ref: metadata.ref,
//                    Asin: metadata.asin,
//                    RefCatalogLink: CatalogLink
//                });
//                //tempDom.parentNode.removeChild(tempDom);
//            }

//            document.querySelector('#zg_nonCritical').innerHTML = '';

//            return findArray;
//        }, targetObj.CatalogLink) || [];

//        for (var i = 0, len = array.length; i < len; i++) {
//            targetObj.Products.push(array[i]);
//        }


//        if (clickSelector.length > 0) {
//            casper.click({ type: 'xpath', path: '//a[@page="' + clickSelector.shift() + '"]' });
//            addStep();
//        }
//        else {
//            casper.echo('※' + JSON.stringify(targetObj));
//        }

//        //casper.echo('※' + JSON.stringify(targetObj));

//        //casper.click({ type: 'xpath', path: '//a[@page="2"]' });
//        //addStep2();
//    });
//    //casper.waitFor(function check() {
//    //    return this.evaluate(function () {
//    //        return document.querySelectorAll('#zg_left_col1 .zg_itemRow').length > 0
//    //    });
//    //}, function then() {
//    //    pushArray(getProducts.call(this));
//    //    waitForSelector.call(this);
//    //});
//}

////function addStep2() {
////    casper.waitForSelector(xLoadDiv, function () {
////        pushArray(getProducts.call(casper));
////        casper.echo('※' + JSON.stringify(targetObj));
////    });
////}

////function getProducts() {
////    return this.evaluate(function () {
////        var domList = document.querySelectorAll('#zg_left_col1 .zg_itemRow');
////        var findArray = [], i, len, tempDom, nameDom;
////        for (i = 0, len = domList.length; i < len; i++) {
////            tempDom = domList[i];
////            nameDom = tempDom.querySelector('.p13n-sc-truncated');
////            findArray.push({
////                imgPath: tempDom.querySelector('.a-thumbnail-left').getAttribute('src'),
////                rankNumber: parseInt(tempDom.querySelector('.zg_rankNumber').innerHTML.replace('.', '')),
////                name: nameDom.getAttribute('title') || nameDom.innerHTML,
////                link: location.origin + decodeURIComponent(tempDom.querySelector('.a-link-normal').getAttribute('href')),
////                price: tempDom.querySelector('.p13n-sc-price').innerHTML
////            });
////            tempDom.parentNode.removeChild(tempDom);
////        }

////        //for (i = 0, len = domList.length; i < len; i++) {
////        //    tempDom = domList[i];
////        //    tempDom.parentNode.removeChild(tempDom);
////        //}

////        return findArray;
////    });
////}

////function pushArray(array) {
////    for (var i = 0, len = array.length; i < len; i++) {
////        targetObj.Products.push(array[i]);
////    }
////}
//addStep();
//casper.run(function () {
//    this.exit();
//});