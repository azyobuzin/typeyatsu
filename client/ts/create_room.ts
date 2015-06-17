(() => {
    var socket = Application.socket;

    WinJS.UI.Pages.define("create_room.html", {
        roomId: "",
        ready: function() {
            var self = this;
            socket.once("createdRoom", id => { self.roomId = id });
            socket.on("updatedRoomMembers", self._onUpdatedRoomMembers);
            socket.emit("createRoom");

            document.getElementById("createroom-back").onclick = () => {
                //TODO: 確認画面
            };
        },
        unload: function() {
            socket.off("updatedRoomMembers", this._onUpdatedRoomMembers);
        },
        _onUpdatedRoomMembers: function(roomId: string, members: string[]) {
            if (roomId === this.roomId) {
                document.getElementById("createroom-members")
                    .winControl.data = new WinJS.Binding.List(members);
            }
        }
    });
})();
