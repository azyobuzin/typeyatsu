(() => {
    var socket = Application.socket;

    function onUpdatedRoomMembers(roomId: string, members: string[]) {
        if (roomId === this.roomId) {
            document.getElementById("createroom-members")
                .winControl.data = new WinJS.Binding.List(members);
        }
    }

    WinJS.UI.Pages.define("create_room.html", {
        roomId: "",
        ready: function() {
            socket.once("createdRoom", id => {
                console.log("Created room:", id);
                this.roomId = id;
            });
            this._onUpdatedRoomMembers = onUpdatedRoomMembers.bind(this);
            socket.on("updatedRoomMembers", this._onUpdatedRoomMembers);
            socket.emit("createRoom");

            document.getElementById("createroom-back").onclick = () => {
                //TODO: 確認画面
            };
        },
        unload: function() {
            socket.off("updatedRoomMembers", this._onUpdatedRoomMembers);
        },
        _onUpdatedRoomMembers: null
    });
})();
