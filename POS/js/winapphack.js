function WinAppHack() {

    if (MSApp && MSApp.execUnsafeLocalFunction) {

        // override jquery html method
        var old_html = $.fn.html;
        $.fn.html = function () {
            var args = arguments;
            var This = this;
            var result = null;
            //$(this).css('font-size', 24)
            MSApp.execUnsafeLocalFunction(function () {
                result = old_html.apply(This, args);
            });
            return result;
        };

        // override alert method
        alert = function (msg, Closed) {
            if (!Closed)
                Closed = function () { };
            navigator.notification.alert(
            msg,
            Closed,
            'Message',
            'OK'
            );
        };
    }
}