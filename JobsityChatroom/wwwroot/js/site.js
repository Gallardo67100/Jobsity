"use strict";
// Open the web socket connection
var connection = new signalR.HubConnectionBuilder().withUrl("/chatroomHub").build();

// Disable send button until connection is established
$("#sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, timestamp) {

    var newMessageClass = window.currentUser === user ? "ml-auto mr-2" : "mr-auto ml-2";
    var newMessageUserName = window.currentUser === user ? "You" : user;

    // Creates a nes message DOM object and append it to the chat
    var newMessageDiv = "<div class='mt-2 border border-primary rounded bg-light pl-4 pr-4 pt-1 pb-1 " + newMessageClass + "'>";
    newMessageDiv += "<small class='d-block text-left w-100 m-0'>" + newMessageUserName + " said: </small>";
    newMessageDiv += "<p class='text-left font-weight-bold'>" + message + "</p>";
    newMessageDiv += "<small class='d-block w-100 text-right m-0'>" + moment(timestamp).format("YYYY-MM-DD HH:mm:ss") + "</small>";
    newMessageDiv += "</div>";

    // If there's already, fifty messages, it deletes the top one every time a new messages arrives.
    if ($("#chatSpace > div").length >= 50) {
        $('#chatSpace').find('div').first().remove();
    }

    $("#chatSpace").html($("#chatSpace").html() + newMessageDiv);
});

connection.start().then(function () {
    $("#sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});