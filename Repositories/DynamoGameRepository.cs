using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace accessible_codenames.Repositories
{
    public class DynamoGameRepository : IGameRepository
    {
        private readonly IAmazonDynamoDB _dynamo;

        public DynamoGameRepository(IAmazonDynamoDB dynamo)
        {
            _dynamo = dynamo;
        }

        public async Task<Game> GetGameById(string id)
        {
            using (var context = new DynamoDBContext(_dynamo))
            {
                var game = await context.LoadAsync<Game>(id);
                return game;
            }
        }

        public async Task SaveGame(Game game)
        {
            game.Expiration = GetExpiration();
            
            using (var context = new DynamoDBContext(_dynamo))
            {
                await context.SaveAsync(game);
            }
        }

        private static long GetExpiration()
        {
            return Convert.ToInt64(
                DateTime.UtcNow.AddDays(7).Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    ).TotalMilliseconds);
        }
    }
}
