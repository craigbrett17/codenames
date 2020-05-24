using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace accessible_codenames.ViewModels
{
    public class GameSpymasterViewModel
    {
        public Team CurrentTurn { get; private set; }
        public List<Word> Words { get; private set; }

        public static GameSpymasterViewModel FromGameModel(Game game) => new GameSpymasterViewModel
        {
            CurrentTurn = game.CurrentTurn,
            Words = game.Words
        };
    }
}
