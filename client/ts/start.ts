import ipc = require("ipc");
(() => {
    var socket = Application.socket;

    WinJS.UI.Pages.define("start.html", {
        ready: () => {
            var inputName = <HTMLInputElement>document.getElementById("start-name");
            inputName.value = Application.userName; //TODO: 初回ランダム生成
            document.getElementById("start-name-container").onsubmit = () => {
                var name = inputName.value || "名無しさん";
                Application.userName = name;
                socket.emit("updateName", name);
                WinJS.Navigation.navigate("rooms.html");
                return false;
            };
            document.getElementById("start-leaflet").onclick = () => { ipc.send("openLeaflet") };

            if (!socket.connected) {
                var btnStart = <HTMLButtonElement>document.getElementById("start-start");
                btnStart.disabled = true;
                btnStart.innerHTML = "接続中...";
                socket.on("connect", () => {
                    btnStart.innerHTML = "スタート";
                    btnStart.disabled = false;
                });
            }
        }
    });
})();
