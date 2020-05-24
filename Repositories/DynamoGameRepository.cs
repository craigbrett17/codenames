using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Models;

namespace accessible_codenames.Repositories
{
    public class DynamoGameRepository : IGameRepository
    {
        public Task<Game> GetGameById(string id)
        {
            return null;
        }

        public Task SaveGame(Game game)
        {
            return Task.CompletedTask;
        }
    }
}
