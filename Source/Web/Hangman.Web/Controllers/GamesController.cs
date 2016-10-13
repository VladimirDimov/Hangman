namespace Hangman.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Hangman.Services.Data.Contracts;
    using Hangman.Web.ActiveGames;
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

            if (model.GameType == GameType.SinglePlayer)
            {
                return this.RedirectToAction(nameof(this.SinglePlayer), new { categoryId = model.CategoryId });
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult SinglePlayer(int categoryId)
        {
            var word = this.wordsService.GetRandomFromCategory(categoryId);
            var activeGamesManager = new ActiveGamesManager();

            var gameId = activeGamesManager.CreateGame(
                                                            word,
                                                            this.User.Identity.GetUserId(),
                                                            this.User.Identity.Name,
                                                            false);

            var game = activeGamesManager[gameId];

            var model = new NewGameViewModel
            {
                NumberOfErrors = 0,
                OpenedPositions = game.Owner.OpenedPositions.ToList()
            };

            this.Response.Cookies.Add(new System.Web.HttpCookie("CurrentGameId", gameId));

            return this.View("Play", model);
        }

        [HttpPost]
        public ActionResult MakeGuess(IEnumerable<MakeGuessRequestViewModel> guesses)
        {
            var gameId = this.Request.Cookies["CurrentGameId"].Value;
            var playerId = this.User.Identity.GetUserId();
            var activeGamesManager = new ActiveGamesManager();
            var updatedPlayer = activeGamesManager.MakeGuess(gameId, playerId, guesses, false);
            var game = activeGamesManager[gameId];
            if (game.GameStatus == GameStatus.HasWinner || game.GameStatus == GameStatus.Closed)
            {
                this.statisticsService.UpdateUserStatistics(playerId, playerId == game.WinnerId, updatedPlayer.NumberOfErrors, updatedPlayer.NumberOfGuesses);
            }

            return this.Json(new
            {
                updatedPlayer,
                game.GameStatus,
                game.WinnerId
            });
        }
    }
}
