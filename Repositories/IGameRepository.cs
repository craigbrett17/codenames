using accessible_codenames.Models;

namespace accessible_codenames.Repositories
{
    public interface IGameRepository
    {
        Game GetGameById(string id);
        void SaveGame(Game game);
    }
}