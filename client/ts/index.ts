import remote = require("remote");
(() => {
    WinJS.Namespace.define("Application", { socket: io(<string>remote.getGlobal("remoteServer")) });
    WinJS.Application.onready = () => {
        WinJS.UI.processAll().done(() => {
            WinJS.Navigation.navigate(Application.navigator.home);
        });
    };
    WinJS.Application.start();
})();
