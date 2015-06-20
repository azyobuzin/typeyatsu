import remote = require("remote");
(() => {
    var socket = io(<string>remote.getGlobal("remoteServer"), { reconnection: true });
    socket.on("connect", () => {
        var id: string = (<any>socket).io.engine.id;
        Application.socketId = id;
        console.log("Socket ID: " + id);
    });
    socket.on("error", err => console.log("Error", err));
    socket.on("reconnecting", n => { console.log("Reconnecting", n) });
    socket.on("reconnect_error", err => { console.log("Reconnect error:", err) });

    WinJS.Namespace.define("Application", { socket: socket, socketId: null, userName: "" });
    WinJS.Application.onready = () => {
        WinJS.UI.processAll().done(() => {
            WinJS.Navigation.navigate(Application.navigator.home);
        });
    };
    WinJS.Application.start();
})();
