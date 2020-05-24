using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace accessible_codenames.Models
{
    public enum Team
    {
        Red,
        Blue
    }
    
    [DynamoDBTable("Codenames")]
    public class Game
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }
        public Team CurrentTurn { get; set; }
        public List<Word> Words { get; set; }
        public DateTime Created { get; set; }
        public long Expiration { get; set; }
    }
}
