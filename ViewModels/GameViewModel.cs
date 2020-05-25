using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Models;

namespace accessible_codenames.ViewModels
{
    public class GameViewModel
    {
        public Team CurrentTurn { get; private set; }
        public IEnumerable<Word> Words { get; private set; }

        public static GameViewModel FromGameModel(Game game) => new GameViewModel
        {
            CurrentTurn = game.CurrentTurn,
            Words = game.Words.Select(word => new Word
            {
                Revealed = word.Revealed,
                Text = word.Text,
                State = (word.Revealed) ? word.State : State.Unknown
            })
        };
    }
}
