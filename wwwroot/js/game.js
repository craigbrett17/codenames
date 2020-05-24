"use strict";

// hacky hacky hacky, we likey the hacky
// lol j/k, this is Microsoft boilerplate code turned into a game
// do fancy stuff later
// current working theory: the game itself will be an SPA embedded within a regular MVC site

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub?gameId=" + getGameIdFromUrl())
    .withAutomaticReconnect()
    .build();

var wordCardTemplate;

_.templateFromUrl("/templates/wordcard.html").then(function (compiledTemplate) {
    wordCardTemplate = compiledTemplate;
});

//Disable change turn button until connection is established
document.getElementById("change-turn-btn").disabled = true;

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    document.getElementById("game-board-ctr").innerHTML = "<div class=\"alert\">Error connecting to the server. An unknown error occurred. Refreshing may resolve this issue</div>";
    return console.error(err.toString());
});

connection.on("GameStateReceived", function (game) {
    console.log("Game state received: ", game);
    document.getElementById("current-turn-lbl").innerText = game.currentTurn + " team's turn";
    document.getElementById("game-board-ctr").innerHTML = "";
    for (var word of game.words) {
        var elem = document.createElement("div");
        var output = wordCardTemplate({ word: word });
        elem.innerHTML = output;
        document.getElementById("game-board-ctr").appendChild(elem);
    }
    document.getElementById("change-turn-btn").disabled = false;
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

function getGameIdFromUrl() {
    var segments = location.pathname.split('/');
    return segments[segments.length - 1];
}

function addToLog(message) {
    var div = document.createElement("div");
    div.innerText = message;
    document.getElementById("text-log").appendChild(div);
}