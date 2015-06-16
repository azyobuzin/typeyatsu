/// <reference path="typings.d.ts" />

import socketIO = require("socket.io");
import common = require("./common");
import rooms = require("./rooms");

var io = socketIO();
var app = new common.Application(io);

io.on("connection", socket => {
    var user = app.createUser(socket);

    socket.on("updateName", name => { user.name = name });

    rooms.initConnection(app, user);
});

var port = process.env["PORT"];
port = port ? Number(port) : 5000;
console.log(`Listening on *:${port}`);
io.listen(port);
