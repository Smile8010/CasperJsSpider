"use strict";
phantom.outputEncoding = "gbk";
var casper = require('casper').create({
    pageSettings: {
        userAgent: 'Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36',
        loadImages: false,        // The WebPage instance used by Casper will
        loadPlugins: false         // use these settings
    }
}), startLink;



if (!casper.cli.has(0)) {
    casper.exit();
}

casper.on('resource.requested', function (reqData, req) {
    if (/facebook|google|twitter|linkedin/.test(reqData.url)) {
        req.abort();
    }
});

startLink = casper.cli.get(0);
casper.start(startLink, function () {   
    var obj = this.evaluate(function (link) {
        var entity = { Url: '', Name: '', ChildCatalogs: [] }, nodeList, i, len, tempNode;
        entity.Url = link;
        entity.Name = document.querySelector('.category').innerHTML;
        nodeList = document.querySelectorAll('#zg_browseRoot>ul>ul>li>a');
        for (i = 0, len = nodeList.length; i < len; i++) {
            tempNode = nodeList[i];
            entity.ChildCatalogs.push({
                Name: tempNode.innerHTML.trim(),
                Url: tempNode.getAttribute('href')

            });
        }
        return entity;
    }, startLink);

    this.echo('※' + JSON.stringify(obj));
});

casper.run(function () {     
    this.exit();
});

