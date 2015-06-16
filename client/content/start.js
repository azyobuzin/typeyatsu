/// <reference path="../typings/bundle.d.ts" />

WinJS.UI.Pages.define("start.html", {
	ready: function () {
		document.getElementById("start-start").onclick = function () {
			WinJS.Navigation.navigate("rooms.html");
		};

		var ipc = require("ipc");
		document.getElementById("start-leaflet").onclick = function () {
			ipc.send("openLeaflet");
		};
	}
});