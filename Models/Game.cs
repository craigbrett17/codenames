using System;
using System.Collections.Generic;

namespace accessible_codenames.Models
{
    public enum Team
    {
        Red,
        Blue
    }
    
    public class Game
    {
        public Guid Id { get; set; }
        public Team CurrentTurn { get; set; }
        public List<Word> Words { get; set; }
        public DateTime Created { get; set; }
    }
}
