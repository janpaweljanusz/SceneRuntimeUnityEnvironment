const engine = require("~engine")

// Our simple serialization/deserialization functions for messages:
const encode = (obj) => JSON.stringify(obj)
const decode = (obj) => JSON.parse(obj)

// I had to change it because it is changed later
// Queues for incoming and outgoing messages:
let incoming = []
let outgoing = []

async function sendReceive() {
    // Exchange serialized messages:
    const res = await engine.sendMessage(outgoing.map(encode)).ToPromise();
    //newIncoming.forEach(elem => console.log(elem));
    const newIncoming = Array.from(res).map(decode)

    incoming = incoming.concat(newIncoming)
    outgoing = []
}

// The known ID of the only Entity in this example:
const cubeId = 1
let rotationX = 0
let scaleY = 0
let isSpaceBarPressed = 0

module.exports.onStart = async function () {
    outgoing.push({
        method: "entity_add",
        data: { id: cubeId }
    })

    outgoing.push({
        method: "entity_transform_update",
        data: {
            entityId: cubeId,
            transform: {
                position: [0, 0, 0],
                rotation: [0, 0, 0, 0],
                scale: [1, 1, 1]
            }
        }
    })

    await sendReceive()
}

module.exports.onUpdate = async function (dt) {
    // Process incoming messages:
    for (msg of incoming) {
        if (msg.method === "key_down" && msg.data.key === "space") {
            isSpaceBarPressed = true
        }
        if (msg.method === "key_up" && msg.data.key === "space") {
            isSpaceBarPressed = false
        }
    }
    // Clear queue
    incoming = [];

    /**
     * Pressing the space bar makes the cube grow bigger.
     * If it's released, it shrinks back to its original size.
     */
    if (isSpaceBarPressed) {
        scaleY += dt
    } else {
        scaleY = Math.max(1.0, scaleY - dt)
    }

    /**
     * The cube rotates on the X axis with time
     */
    rotationX += dt

    const currentRot = fromEuler(0, rotationX, 0);
    // Queue outgoing messages:
    outgoing.push({
        method: "entity_transform_update",
        data: {
            entityId: cubeId,
            transform: {
                position: [0, 0, 0],
                //rotation: [rotationX, 0, 0, 0], // it would mess up a project
                rotation: currentRot,
                scale: [1, scaleY, 1]
            }
        }
    })

    // Make the exchange:
    await sendReceive()
}


function fromEuler (X, Y, Z) {

    var _x = X * 0.5;
    var _y = Y * 0.5;
    var _z = Z * 0.5;

    var cX = Math.cos(_x);
    var cY = Math.cos(_y);
    var cZ = Math.cos(_z);

    var sX = Math.sin(_x);
    var sY = Math.sin(_y);
    var sZ = Math.sin(_z);

    return [
        cX * cY * cZ + sX * sY * sZ,
        cY * cZ * sX - cX * sY * sZ,
        cX * cY * sZ - cZ * sX * sY,
        cX * cZ * sY + cY * sX * sZ];
};