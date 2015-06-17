import socketIO = require("socket.io");
import common = require("./common");
import rooms = require("./rooms");

var io = socketIO();
var app = new common.Application(io);

io.on("connection", socket => {
    console.log("Connection: " + socket.id);
    socket.on("disconnect", () => {
        console.log("Disconnect: " + socket.id)
        //TODO: ユーザーが参加中のルームの処理
    });

    var user = app.createUser(socket);
    socket.on("updateName", name => { user.name = name });
    rooms.initConnection(app, user);
});

var port = process.env["PORT"];
port = port ? Number(port) : 5000;
console.log(`Listening on *:${port}`);
io.listen(port);
