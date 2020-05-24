using System.Threading.Tasks;
using accessible_codenames.Models;

namespace accessible_codenames.Services
{
    public interface IGameService
    {
        Task<Game> CreateGame(string wordList);
        Task<Team> ChangeTurn(string gameId);
        Task<WordPickOutcome> RevealWord(string gameId, string word);
    }
}