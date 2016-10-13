namespace Hangman.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Hangman.Services.Data.Contracts;
    using Hangman.Web.ActiveGames;
    using Hubs;
    using Microsoft.AspNet.Identity;
    using ViewModels.Games;
    using ViewModels.Games.Index;

    [Authorize]
    public class GamesController : Controller
    {
        private ICategoriesService categoriesService;
        private IWordsService wordsService;
        private IStatisticsService statisticsService;

        public GamesController(
            ICategoriesService categoriesService,
            IWordsService wordsService,
            IStatisticsService statisticsService)
        {
            this.categoriesService = categoriesService;
            this.wordsService = wordsService;
            this.statisticsService = statisticsService;
        }

        public ActionResult Index()
        {
            var categories = this.categoriesService.All()
                .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                .ToList();

            var model = new IndexViewModel
            {
                Categories = categories
            };

            return this.View(model);
        }

        public ActionResult StartGame(StartGameRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var word = this.wordsService.GetRandomFromCategory(model.CategoryId);
            var activeGamesManager = new ActiveGamesManager();

            var gameId = activeGamesManager.CreateGame(
                                                        word,
                                                        this.User.Identity.GetUserId(),
                                                        this.User.Identity.Name,
                                                        model.GameType == GameType.MultiPlayer,
                                                        model.GameName ?? $"Unknown {new Random().Next(100, 10000)}");

            var game = activeGamesManager[gameId];

            var responseModel = new NewGameViewModel
            {
                NumberOfErrors = 0,
                OpenedPositions = game.Owner.OpenedPositions.ToList(),
                IsMultiplayer = game.IsMultiplayer,
                GameId = gameId
            };

            this.Response.Cookies.Add(new System.Web.HttpCookie("CurrentGameId", gameId));

            return this.View("Play", responseModel);
        }

        public ActionResult GetCurrentStatus(string gameId)
        {
            var activeGamesManager = new ActiveGamesManager();
            var game = activeGamesManager[gameId];

            return this.Json(game, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AvailableGames()
        {
            var gamesManager = new ActiveGamesManager();
            var activeGames = gamesManager.Games
                .Where(x => x.Value.IsMultiplayer)
                .Select(x => new JoinGameViewModel { Id = x.Key, Name = x.Value.GameName });

            return this.View("Join", activeGames);
        }

        public ActionResult Join(string gameId)
        {
            var gamesManager = new ActiveGamesManager();
            gamesManager.JoinGame(gameId, this.User.Identity.GetUserId(), this.User.Identity.Name);

            var notifier = new Notifier();
            var game = gamesManager[gameId];
            var playerIds = game.Players
                .Select(p => p.Id)
                .Where(p => p != this.User.Identity.GetUserId())
                .ToArray();
            notifier.UpdateGame(playerIds);

            var responseModel = new NewGameViewModel
            {
                NumberOfErrors = 0,
                OpenedPositions = game.Owner.OpenedPositions.ToList(),
                IsMultiplayer = game.IsMultiplayer,
                GameId = gameId
            };

            this.Response.Cookies.Add(new System.Web.HttpCookie("CurrentGameId", gameId));

            return this.View("Play", responseModel);
        }

        [HttpPost]
        public ActionResult GuessAll(string guess)
        {
            var gameId = this.Request.Cookies["CurrentGameId"].Value;
            var gamesManager = new ActiveGamesManager();
            gamesManager.GuessAll(this.User.Identity.GetUserId(), gameId, guess);

            return this.Json(gamesManager[gameId]);
        }

        [HttpPost]
        public ActionResult MakeGuess(char guess)
        {
            var gameId = this.Request.Cookies["CurrentGameId"].Value;
            var playerId = this.User.Identity.GetUserId();
            var activeGamesManager = new ActiveGamesManager();
            var updatedPlayer = activeGamesManager.MakeGuess(gameId, playerId, guess, false);
            var game = activeGamesManager[gameId];

            if (game.GameStatus == GameStatus.HasWinner || game.GameStatus == GameStatus.Closed)
            {
                this.statisticsService.UpdateUserStatistics(playerId, playerId == game.WinnerId, updatedPlayer.NumberOfErrors, updatedPlayer.NumberOfGuesses);
            }
            else if (updatedPlayer.NumberOfErrors == 5)
            {
                this.statisticsService.UpdateUserStatistics(playerId, playerId == game.WinnerId, updatedPlayer.NumberOfErrors, updatedPlayer.NumberOfGuesses);
            }

            var gameStatus = game.GameStatus;
            var gameWinnerId = game.WinnerId;

            var notifier = new Notifier();
            notifier.UpdateGame(
                                game.Players
                                .Select(x => x.Id)
                                .Where(x => x != this.User.Identity.GetUserId())
                                .ToArray());

            return this.Json(game);
        }
    }
}
