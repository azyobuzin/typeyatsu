import remote = require("remote");
(() => {
    var socket = io(<string>remote.getGlobal("remoteServer"));
    socket.on("connect", () => {
        var id: string = (<any>socket).io.engine.id;
        Application.socketId = id;
        console.log("Socket ID: " + id);
    });

    WinJS.Namespace.define("Application", { socket: socket, socketId: null, userName: "" });
    WinJS.Application.onready = () => {
        WinJS.UI.processAll().done(() => {
            WinJS.Navigation.navigate(Application.navigator.home);
        });
    };
    WinJS.Application.start();
})();
