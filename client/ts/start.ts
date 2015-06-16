import ipc = require("ipc");
(() => {
    var socket = Application.socket;

    WinJS.UI.Pages.define("start.html", {
        ready: () => {
            document.getElementById("start-name-container").onsubmit = () => {
                WinJS.Navigation.navigate("rooms.html");
                return false;
            };
            document.getElementById("start-leaflet").onclick = () => { ipc.send("openLeaflet") };

            if (!socket.connected) {
                var btnStart: any = document.getElementById("start-start");
                btnStart.disabled = "disabled";
                btnStart.innerHTML = "接続中...";
                socket.on("connect", () => {
                    btnStart.innerHTML = "スタート";
                    btnStart.disabled = "";
                });
            }
        }
    });
})();
