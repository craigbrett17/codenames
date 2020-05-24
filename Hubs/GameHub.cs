namespace accessible_codenames.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using accessible_codenames.Repositories;
    using accessible_codenames.Services;
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

        public async Task ChangeTurn()
        {
            
        }

        public async Task PickWord(string word)
        {

        }

        public async Task SpymasterReveal()
        {

        }
    }
}
