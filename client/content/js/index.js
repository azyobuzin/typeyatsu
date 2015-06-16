var remote = require("remote");
(function () {
    WinJS.Namespace.define("Application", { socket: io(remote.getGlobal("remoteServer")) });
    WinJS.Application.onready = function () {
        WinJS.UI.processAll().done(function () {
            WinJS.Navigation.navigate(Application.navigator.home);
        });
    };
    WinJS.Application.start();
})();
