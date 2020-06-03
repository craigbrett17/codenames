using System.Threading.Tasks;
using accessible_codenames.Models;
using accessible_codenames.ViewModels;

namespace accessible_codenames.Hubs
{
    public interface IGameHubClient
    {
        Task ChangedTurn(Team team);
        Task WordPicked(Word word);
        Task GameStateReceived(GameViewModel viewModel);
        Task PromotedToSpymaster(GameSpymasterViewModel viewModel);
        Task NewSpyMasterAdded();
    }
}
