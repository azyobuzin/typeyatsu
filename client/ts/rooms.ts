interface Room {
    ownerName: string
    membersCount: number
}

(() => {
    var socket = Application.socket;

    WinJS.UI.Pages.define("rooms.html", {
        ready: function() {
            socket.on("updatedRooms", this._onUpdateRooms);
            socket.emit("enterRoomsPage");

            document.getElementById("rooms-createroom").onclick = () => {
                WinJS.Navigation.navigate("create_room.html");
            };
        },
        unload: function() {
            socket.off("updatedRooms", this._onUpdateRooms);
        },
        _onUpdateRooms: function(rooms: Room[]) {
            document.getElementById("rooms-availablerooms")
                .winControl.data = new WinJS.Binding.List(rooms);
        }
    });
})();
