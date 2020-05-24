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
            var context = new DynamoDBContext(_dynamo);
            var game = await context.LoadAsync<Game>(id);
            return game;
        }

        public async Task SaveGame(Game game)
        {
            var context = new DynamoDBContext(_dynamo);
            await context.SaveAsync(game);
        }
    }
}
