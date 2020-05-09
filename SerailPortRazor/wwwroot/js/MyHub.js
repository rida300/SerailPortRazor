"use strict";

import { signalR } from "../lib/signalr.js/signalr";

var connection = new signalR.HubConnectionBuilder()
    .ConfigureLogging(signalRtimespa.LogLevel.Information)
    .build();

connection.on("ReceiveMessage", function (msg1) {
    document.getElementById("SerialData").value = msg1;
});

connection.start().catch(function err() {
    return console.error(err.ToString());
});