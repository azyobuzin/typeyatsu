import base64id = require("base64id");

export class User {
    name: string;
    constructor(public socket: SocketIO.Socket) { }
}

export class Room {
    constructor(public id: string, public owner: User) {
        this._members = [owner];
    }

    private _members: User[];
    get members(): User[] { return this._members; }

    enter(user: User) {
        this._members.push(user);
    }

    exit(user: User) {
        var i = this._members.indexOf(user);
        if (i != -1) this._members.splice(i, 1);
    }
}

export var generateId = base64id.generateId;

export class Application {
    private users: { [socketId: string]: User } = {};
    private availableRooms: { [id: string]: Room } = {};
    constructor(public server: SocketIO.Server) { }

    createUser(socket: SocketIO.Socket): User {
        var user = new User(socket);
        this.users[socket.id] = user;
        return user;
    }

    getUser(socketId: string): User {
        return this.users[socketId];
    }

    getAvailableRooms(): Room[] {
        var result: Room[] = [];
        for (var id in this.availableRooms) {
            result.push(this.availableRooms[id]);
        }
        return result;
    }

    getRoom(id: string): Room {
        return this.availableRooms[id];
    }

    createRoom(owner: User): Room {
        var id = generateId();
        var room = new Room(id, owner);
        this.availableRooms[id] = room;
        return room;
    }

    destroyRoom(id: string) {
        delete this.availableRooms[id];
    }
}
