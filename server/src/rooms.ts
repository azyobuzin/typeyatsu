import common = require("./common");

const roomsPageRoom = "roomsPage";
const updatedRoomsEvent = "updatedRooms";
const destroyedRoomEvent = "destroyedRoom";

export function initConnection(app: common.Application, user: common.User) {
    var socket = user.socket;

    function formatAvailableRooms() {
        return app.getAvailableRooms().map(v => {
            return {
                ownerName: v.owner.name, membersCount: v.members.length
            }
        });
    }

    function notifyUpdatedRooms() {
        app.server.to(roomsPageRoom).emit(updatedRoomsEvent, formatAvailableRooms());
    }

    function notifyUpdatedRoomMembers(room: common.Room) {
        app.server.to(room.id)
            .emit("updatedRoomMembers", room.id, room.members.map(v => v.name));
        notifyUpdatedRooms();
    }

    socket.on("enterRoomsPage", () =>
        socket.join(roomsPageRoom).emit(updatedRoomsEvent, formatAvailableRooms()));

    socket.on("exitRoomsPage", () => socket.leave(roomsPageRoom));

    socket.on("createRoom", () => {
        var room = app.createRoom(user);
        socket.join(room.id).emit("createdRoom", room.id);
        notifyUpdatedRoomMembers(room);
    });

    socket.on("destroyRoom", (id: string) => {
        var room = app.getRoom(id);
        if (!room || room.owner !== user) return;
        app.server.to(id).emit(destroyedRoomEvent, id);
        app.destroyRoom(id);
        notifyUpdatedRooms();
    });

    socket.on("enterRoom", (id: string) => {
        var room = app.getRoom(id);
        if (!room) {
            socket.emit(destroyedRoomEvent, id);
            return;
        }
        socket.join(id);
        room.enter(user);
        notifyUpdatedRoomMembers(room);
    });

    socket.on("exitRoom", (id: string) => {
        var room = app.getRoom(id);
        if (!room) return;
        socket.leave(id);
        room.exit(user);
        notifyUpdatedRoomMembers(room);
    });
}
