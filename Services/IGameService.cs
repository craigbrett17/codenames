using System.Threading.Tasks;
using accessible_codenames.Models;

namespace accessible_codenames.Services
{
    public interface IGameService
    {
        Task<Game> CreateGame(string wordList);
        Task ChangeTurn(Game game);
    }
}