/// <reference path="typings/bundle.d.ts" />

var app = require("app");
var BrowserWindow = require("browser-window");

var typeRemote = process.env["TYPE_REMOTE"];
if (!typeRemote) typeRemote = "typesuruyatsu.azyobuzi.net";
global.remoteServer = typeRemote;

var mainWindow = null;

app.on("ready", function () {
    mainWindow = new BrowserWindow({ width: 800, height: 600 });
    mainWindow.loadUrl("file://" + __dirname + "/content/index.html");
    //mainWindow.openDevTools();
    mainWindow.on("closed", function () { app.quit(); });
});

var ipc = require("ipc");
var leafletWindow = null;

ipc.on("openLeaflet", function () {
    if (leafletWindow) {
        leafletWindow.focus();
    } else {
        leafletWindow = new BrowserWindow({ width: 400, height: 300 });
        leafletWindow.loadUrl("file://" + __dirname + "/content/leaflet.html");
        leafletWindow.on("closed", function () { leafletWindow = null; })
    }
});
