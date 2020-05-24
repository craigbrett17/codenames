using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using accessible_codenames.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace accessible_codenames.Controllers
{
    public class GameController : Controller
    {
        private IGameService _gameService;
        private ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        public IActionResult Index(string id)
        {
            return View();
        }

        public async Task<IActionResult> Create(string id)
        {
            var game = await _gameService.CreateGame(wordList: id);
            _logger.LogInformation($"Game created. With ID: {game.Id}. Returning redirect");
            return RedirectToAction("Index", "Game", new { id = game.Id.ToString() });
        }
    }
}