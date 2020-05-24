_.mixin({
    templateFromUrl: function (url, data, settings) {
        return new Promise((resolve, reject) => {
            var templateHtml = "";
            this.cache = this.cache || {};

            if (this.cache[url]) {
                templateHtml = this.cache[url];
                resolve(_.template(templateHtml, data, settings));
            } else {
                fetch(url).then((responseData) => {
                    console.log(responseData);
                    responseData.text().then((text) => {
                        templateHtml = text;
                        this.cache[url] = templateHtml;

                        resolve(_.template(templateHtml, data, settings));
                    });
                });
            }
        });
    }
});