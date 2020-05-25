namespace accessible_codenames.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;
    using accessible_codenames.Repositories;
    using accessible_codenames.Services;
    using accessible_codenames.ViewModels;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    public class GameHub : Hub<IGameHubClient>
    {
        private readonly IGameRepository _repository;
        private readonly IGameService _service;
        private readonly ILogger<GameHub> _logger;

        public GameHub(IGameRepository repository, ILogger<GameHub> logger, IGameService service)
        {
            _repository = repository;
            _logger = logger;
            _service = service;
        }

        public override async Task OnConnectedAsync()
        {
            string gameId = GetGameId();

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            
            var game = await _repository.GetGameById(gameId);
            await Clients.Caller.GameStateReceived(GameViewModel.FromGameModel(game));

            await base.OnConnectedAsync();
        }

        public async Task ChangeTurn()
        {
            var gameId = GetGameId();
            var newTurn = await _service.ChangeTurn(gameId);
            await Clients.Group(gameId).ChangedTurn(newTurn);
        }

        public async Task PickWord(string word)
        {
            var gameId = GetGameId();
            var outcome = await _service.RevealWord(gameId, word);
            await Clients.Group(gameId).WordPicked(outcome.Word);

            if (outcome.ShouldChangeTurn)
            {
                await this.ChangeTurn();
            }
        }

        public async Task PromoteToSpymaster()
        {
            var gameId = GetGameId();
            var game = await _repository.GetGameById(gameId);

            await Clients.Caller.PromotedToSpymaster(GameSpymasterViewModel.FromGameModel(game));
            await Clients.OthersInGroup(gameId).NewSpyMasterAdded();
        }

        private string GetGameId()
        {
            // find the game ID using the HTTP context that they're calling from
            var context = Context.GetHttpContext();
            var gameId = context.Request.Query["gameId"].FirstOrDefault();
            if (gameId == null)
            {
                throw new HubException("Unable to determine game ID from requested URL");
            }

            return gameId;
        }
    }
}
