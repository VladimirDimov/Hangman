using System.ComponentModel.DataAnnotations;

namespace Hangman.Web.ViewModels.Games
{
    public class StartGameRequestViewModel
    {
        [Display(Name = "Game Type", Description = "Singleplayer or Multiplayer")]
        public GameType GameType { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
