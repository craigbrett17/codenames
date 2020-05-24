"use strict";

// hacky hacky hacky, we likey the hacky
// lol j/k, this is Microsoft boilerplate code turned into a game
// do fancy stuff later
// current working theory: the game itself will be an SPA embedded within a regular MVC site

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub?gameId=" + getGameIdFromUrl())
    .withAutomaticReconnect()
    .build();

//Disable change turn button until connection is established
document.getElementById("change-turn-btn").disabled = true;

connection.start().then(function () {
    connection.invoke("GetGameState", getGameIdFromUrl()).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("GameStateReceived", function (game) {
    document.getElementById("current-turn-lbl").innerText = game.currentTurn + " team's turn";
    for (var word in game.words) {
        var elem = document.createElement("div");
        elem.classList.add("word");
        elem.classList.add("col-md-3");
        elem.classList.add("col-sm-4");
        elem.innerText = word;
        document.getElementById("game-board-ctr").appendChild(elem);
    }
    addToLog("Game loaded");
});

document.getElementById("change-turn-btn").addEventListener("click", function (e) {
    connection.invoke("ChangeTurn");
    e.preventDefault();
});

connection.on("ChangedTurn", function (team) {
    document.getElementById("current-turn-lbl").innerText = team + " team's turn";
    addToLog("Turn changed: " + team + " team's turn");
});

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function getGameIdFromUrl() {
    var segments = location.pathname.split('/');
    return segments[segments.length - 1];
}

function addToLog(message) {
    var div = document.createElement("div");
    div.innerText = message;
    document.getElementById("text-log").appendChild(div);
}