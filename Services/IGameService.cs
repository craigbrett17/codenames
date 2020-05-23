using accessible_codenames.Models;

namespace accessible_codenames.Services
{
    public interface IGameService
    {
        Game CreateGame(string wordList);
    }
}