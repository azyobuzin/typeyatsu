/// <reference path="../typings/github-electron/github-electron-renderer.d.ts" />
/// <reference path="../typings/typings/winjs/winjs.d.ts" />
/// <reference path="../typings/socket.io-client/socket.io-client.d.ts" />

declare module Application {
    interface PageControlNavigator {
        home: string
    }

    var navigator: PageControlNavigator
    var socket: SocketIOClient.Socket
}
