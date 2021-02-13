"use strict";
// Open the web socket connection
var connection = new signalR.HubConnectionBuilder().withUrl("/chatroomHub").build();

// Disable send button until connection is established
$("#sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, timestamp) {
    var newMessageClass = "";
    if (window.currentUser === user) {
        newMessageClass = "mt-2 ml-auto mr-2 border border-primary rounded bg-light pl-4 pr-4 pt-1 pb-1";
    } else {
        newMessageClass = "mt-2 mr-auto mr-2 border border-primary rounded bg-light pl-4 pr-4 pt-1 pb-1";
    }

    // Creates a nes message DOM object and append it to the chat
    var newMessageDiv = "<div class='" + newMessageClass + "'>";
    newMessageDiv += "<small class='d-block text-left w-100 text-right m-0'>" + user + "</small>";
    newMessageDiv += "<p class='text-left font-weight-bold'>" + message + "</p>";
    newMessageDiv += "<small class='d-block w-100 text-right m-0'>" + moment(timestamp).format("YYYY-MM-DD HH:mm:ss") + "</small>";
    newMessageDiv += "</div>";

    $("#chatSpace").html($("#chatSpace").html() + newMessageDiv);
});

connection.start().then(function () {
    $("#sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});