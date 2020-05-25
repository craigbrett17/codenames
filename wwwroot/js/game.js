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
    // using our underscore mixin here. it returns a function that we can assign and use to do the templating
    wordCardTemplate = compiledTemplate;
});

//Disable change turn button until connection is established
document.querySelector("#change-turn-btn").disabled = true;

connection.on("GameStateReceived", function (game) {
    console.log("Game state received: ", game);
    document.querySelector("#current-turn-lbl").innerText = game.currentTurn + " team's turn";
    document.querySelector("#game-board-ctr").innerHTML = "";
    for (var word of game.words) {
        var elem = document.createElement("div");
        var output = wordCardTemplate({ word: word });
        elem.innerHTML = output;
        document.querySelector("#game-board-ctr").appendChild(elem);
    }
    document.querySelector("#change-turn-btn").disabled = false;
    addToLog("Game loaded");
});

document.querySelector("#change-turn-btn").addEventListener("click", function (e) {
    connection.invoke("ChangeTurn");
    e.preventDefault();
});

connection.on("ChangedTurn", function (team) {
    document.querySelector("#current-turn-lbl").innerText = team + " team's turn";
    addToLog("Turn changed: " + team + " team's turn");
});

function onWordClicked(e) {
    var word = e.dataset.id;
    connection.invoke("PickWord", word);
}

connection.on("WordPicked", function (word) {
    var wordState = word.state.toLowerCase();
    if (wordState != "assassin") {
        addToLog(`The word ${word.text} was picked. It was ${word.state}`);
    } else {
        addToLog(`The word ${word.text} was picked. It was the assassin word!`);
    }
    var wordCard = document.querySelector(".word[data-id='" + word.text + "']");
    wordCard.classList.add(wordState);
    wordCard.classList.add("revealed");
});

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    document.getElementById("game-board-ctr").innerHTML = "<div class=\"alert\">Error connecting to the server. An unknown error occurred. Refreshing may resolve this issue</div>";
    return console.error(err.toString());
});

function getGameIdFromUrl() {
    var segments = location.pathname.split('/');
    return segments[segments.length - 1];
}

function addToLog(message) {
    var div = document.createElement("div");
    div.innerText = message;
    document.querySelector("#text-log").appendChild(div);
}