/// <reference path="../typings/bundle.d.ts" />

declare module "base64id" {
    /**
     * Get random bytes
     *
     * Uses a buffer if available, falls back to crypto.randomBytes
     */
    function getRandomBytes(bytes: Buffer): Buffer;
    /**
     * Generates a base64 id
     *
     * (Original version from socket.io <http://socket.io>)
     */
    function generateId(): string;
}

declare module SocketIO {
    interface Namespace {
        to(room: string): Namespace;
    }

    interface Server {
        to(room: string): Namespace;
        in(room: string): Namespace;
    }
}
