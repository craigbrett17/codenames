﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Models;
using accessible_codenames.Repositories;
using Toore.Shuffling;

namespace accessible_codenames.Services
{
    public class GameService : IGameService
    {
        private IGameRepository _repository;
        private IShuffle _cardShuffler;
        private Random _random;

        private static int NumberOfAssassins = 1;

        public GameService(IGameRepository repository, IShuffle shuffle)
        {
            _repository = repository;
            _cardShuffler = shuffle;
            _random = new Random();
        }

        public async Task<Game> CreateGame(string wordList, int numberOfCards)
        {
            var game = new Game
            {
                Created = DateTime.UtcNow,
                CurrentTurn = Team.Blue,
                Id = Guid.NewGuid().ToString()
            };

            game.Words = CreateWordsForGame(wordList, numberOfCards);

            await _repository.SaveGame(game);

            return game;
        }

        public async Task<Team> ChangeTurn(string gameId)
        {
            var game = await _repository.GetGameById(gameId);
            game.CurrentTurn = (game.CurrentTurn == Team.Red) ? Team.Blue : Team.Red;
            await _repository.SaveGame(game);

            return game.CurrentTurn;
        }

        public async Task<WordPickOutcome> RevealWord(string gameId, string word)
        {
            var game = await _repository.GetGameById(gameId);
            var wordCard = game.Words.Single(w => w.Text == word);
            wordCard.Revealed = true;
            await _repository.SaveGame(game);

            return new WordPickOutcome
            {
                Word = wordCard,
                ShouldChangeTurn = wordCard.State != State.Assassin && wordCard.State.ToString() != game.CurrentTurn.ToString()
            };
        }

        private List<Word> CreateWordsForGame(string wordList, int numberOfCards)
        {
            string wordListFile = (wordList.ToLower()) switch
            {
                "normal" => "normal1.txt",
                "adult" => "adult1.txt",
                "adult_uk" => "adult_uk1.txt",
                "adult_us" => "adult_us1.txt",
                _ => throw new ArgumentException("Unrecognized word list name used"),
            };

            var allWords = GetAllWordsInWordList(wordListFile);

            var wordCards = new List<Word>();

            var sides = new List<State> { State.Blank, State.Blue, State.Red };
            var numberOfCardsPerTeam = (numberOfCards - NumberOfAssassins) / sides.Count;

            foreach (var side in sides)
            {
                for (int index = 0; index < numberOfCardsPerTeam; index++)
                {
                    string word = PickNewUniqueWord(allWords, wordCards);
                    wordCards.Add(new Word { Text = word, State = side });
                }
            }

            for (int index = 0; index < NumberOfAssassins; index++)
            {
                string word = PickNewUniqueWord(allWords, wordCards);
                wordCards.Add(new Word { Text = word, State = State.Assassin });
            }

            wordCards = _cardShuffler.Shuffle(wordCards).ToList();

            return wordCards;
        }

        private string PickNewUniqueWord(List<string> allWords, List<Word> selectedWords)
        {
            string word = null;
            do
            {
                word = allWords[_random.Next(allWords.Count)];
            } while (word == null || selectedWords.Any(existing => existing.Text == word));
            return word;
        }

        private List<string> GetAllWordsInWordList(string wordListFile) =>
            File.ReadAllLines($"wordlists/{wordListFile}").ToList();
    }
}
