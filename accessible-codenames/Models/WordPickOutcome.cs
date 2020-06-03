namespace accessible_codenames.Models
{
    public class WordPickOutcome
    {
        public bool ShouldChangeTurn { get; set; }
        public Word Word { get; internal set; }
    }
}
