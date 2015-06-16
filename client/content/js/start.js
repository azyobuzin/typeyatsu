var ipc = require("ipc");
(function () {
    var socket = Application.socket;
    WinJS.UI.Pages.define("start.html", {
        ready: function () {
            document.getElementById("start-name-container").onsubmit = function () {
                WinJS.Navigation.navigate("rooms.html");
                return false;
            };
            document.getElementById("start-leaflet").onclick = function () { ipc.send("openLeaflet"); };
            if (!socket.connected) {
                var btnStart = document.getElementById("start-start");
                btnStart.disabled = "disabled";
                btnStart.innerHTML = "接続中...";
                socket.on("connect", function () {
                    btnStart.innerHTML = "スタート";
                    btnStart.disabled = "";
                });
            }
        }
    });
})();
