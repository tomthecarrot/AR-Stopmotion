
// Load WebSocket networking library
WebSocket = require('ws');

// Start WebSocket server
wss = new WebSocket.Server({
  port: 8080,
  host : '0.0.0.0'
});

cameraSocket = null;

// Configure WebSocket server
wss.on('connection', function connection(socket) {

  console.log("A client connected!");

  // Handle client data
  socket.on('message', function incoming(message) {
    console.log("Received message: %s", message);
    if (message == "camera") {
      cameraSocket = socket;
    }
    else {
      if (cameraSocket != null) {
        cameraSocket.send("photo\n");
      }
    }
  });

  // Handle client disconnect
  wss.on('close', function close() {
    console.log("A client disconnected");
  });

  // Handle client error
  wss.on('error', function() {
    console.log("A connection error occurred");
  });

});
