"use strict";

// hacky hacky hacky, we likey the hacky
// lol j/k, this is Microsoft boilerplate code turned into a game
// do fancy stuff later
// current working theory: the game itself will be an SPA embedded within a regular MVC site
// although: now we have the templating and such, it's not too shabby for framework-less

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/gamehub?gameId=" + getGameIdFromUrl())
    .withAutomaticReconnect()
    .build();

var wordCardTemplate;

_.templateFromUrl("/templates/wordcard.html").then(function (compiledTemplate) {
    // using our underscore mixin here. it returns a function that we can assign and use to do the templating
    wordCardTemplate = compiledTemplate;
});

//Disable buttons until connection is established
for (var button of document.querySelectorAll(".btn")) {
    button.disabled = true;
}

connection.on("GameStateReceived", function (game) {
    console.log("Game state received: ", game);
    document.querySelector("#current-turn-lbl").innerText = game.currentTurn + " team's turn";
    document.querySelector("#game-board-ctr").innerHTML = "";
    var container = document.createElement("div");
    container.classList.add("row");
    for (var word of game.words) {
        var templateElement = document.createElement("template");
        var output = wordCardTemplate(word);
        templateElement.innerHTML = output;
        var card = templateElement.content.firstChild;

        if (word.revealed) {
            var wordState = word.state.toLowerCase();
            var wordCard = card.querySelector(".btn");
            wordCard.classList.add(wordState);
            wordCard.classList.add("revealed");
            wordCard.setAttribute("aria-pressed", "true");
            wordCard.setAttribute("aria-label", word.text + ": " + wordState);
        }

        container.appendChild(card);
    }
    document.querySelector("#game-board-ctr").appendChild(container);

    for (var button of document.querySelectorAll(".btn")) {
        button.disabled = false;
    }

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
    var wordCard = document.querySelector(".word .btn[data-id='" + word.text + "']");
    wordCard.classList.add(wordState);
    wordCard.classList.add("revealed");
    wordCard.setAttribute("aria-pressed", "true");
    wordCard.setAttribute("aria-label", word.text + ": " + wordState);

    if (wordState != "assassin") {
        addToLog(`The word ${word.text} was picked. It was ${word.state}`);
    } else {
        addToLog(`The word ${word.text} was picked. It was the assassin word!`);
    }
});

function onSpymasterButtonClicked() {
    connection.invoke("PromoteToSpymaster");
    document.querySelector("#spymaster-btn").disabled = true;
}

connection.on("PromotedToSpymaster", function (game) {
    addToLog("You have been promoted to a spymaster");
    for (var word of game.words) {
        var wordState = word.state.toLowerCase();
        var wordCard = document.querySelector(".word .btn[data-id='" + word.text + "']");
        wordCard.classList.add(wordState);
        wordCard.setAttribute("aria-label", word.text + ": " + wordState);
    }
    document.querySelector("body").classList.add("spymaster");
});

connection.on("NewSpymasterAdded", function () {
    addToLog("A new spymaster has been appointed");
});

function onFilterClicked(e) {
    var id = e.dataset.id;
    if (id == "all") {
        for (var wordCard of document.querySelectorAll(".card")) {
            wordCard.classList.remove("d-none");
        }
    } else {
        for (var wordCard of document.querySelectorAll(".card")) {
            if (wordCard.querySelector(".btn").classList.contains(id)) {
                wordCard.classList.remove("d-none");
            } else {
                wordCard.classList.add("d-none");
            }
        }
    }
}

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