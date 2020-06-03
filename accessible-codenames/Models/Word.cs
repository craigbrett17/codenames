using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace accessible_codenames.Models
{
    public enum State
    {
        Unknown,
        Red,
        Blue,
        Blank,
        Assassin
    }
    
    public class Word
    {
        public string Text { get; set; }
        public State State { get; set; }
        public bool Revealed { get; set; }
    }
}
