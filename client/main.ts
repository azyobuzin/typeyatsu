/// <reference path="typings/github-electron/github-electron-main.d.ts" />

import app = require("app");
import BrowserWindow = require("browser-window");
import ipc = require("ipc");

var typeRemote: string = process.env["TYPE_REMOTE"];
if (!typeRemote) typeRemote = "http://typesuruyatsu.azyobuzi.net/";
global.remoteServer = typeRemote;

var mainWindow: GitHubElectron.BrowserWindow = null;

app.on("ready", () => {
    mainWindow = new BrowserWindow({ width: 800, height: 600 });
    mainWindow.loadUrl(`file://${__dirname}/content/index.html`);
    mainWindow.on("closed", () => { app.quit(); });
});

var leafletWindow: GitHubElectron.BrowserWindow = null;

ipc.on("openLeaflet", () => {
    if (leafletWindow) {
        leafletWindow.focus();
    } else {
        leafletWindow = new BrowserWindow({ width: 400, height: 300 });
        leafletWindow.loadUrl("file://" + __dirname + "/content/leaflet.html");
        leafletWindow.on("closed", () => { leafletWindow = null; })
    }
});
