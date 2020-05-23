using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Models;
using accessible_codenames.Repositories;

namespace accessible_codenames.Services
{
    public class GameService
    {
        private IGameRepository _repository;
        private static int NumberOfCards = 25;
        private static int NumberOfAssassins = 1;
        private static int NumberOfCardsPerTeam = (NumberOfCards - NumberOfAssassins) / 3;

        public GameService(IGameRepository repository)
        {
            _repository = repository;
        }

        public Game CreateGame(string wordList)
        {
            var game = new Game
            {
                Created = DateTime.UtcNow,
                CurrentTurn = Team.Blue,
                Id = Guid.NewGuid()
            };

            game.Words = CreateWordsForGame(wordLis);

            _repository.SaveGame(game);
            
            return game;
        }

        private List<Word> CreateWordsForGame(string wordList)
        {
            string wordListFile = (wordList.ToLower()) switch
            {
                "normal" => "normal1.txt",
                "adult" => "adult1.txt",
                _ => throw new ArgumentException("Unrecognized word list name used"),
            };

            var allWords = GetAllWordsInWordList(wordListFile);

            var selectedWords = new List<Word>();



            return selectedWords;
        }

        private IEnumerable<string> GetAllWordsInWordList(string wordListFile) =>
            File.ReadAllLines($"wordlists/{wordListFile}");
    }
}
