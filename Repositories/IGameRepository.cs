using System.Threading.Tasks;
using accessible_codenames.Models;

namespace accessible_codenames.Repositories
{
    public interface IGameRepository
    {
        Task<Game> GetGameById(string id);
        Task SaveGame(Game game);
    }
}