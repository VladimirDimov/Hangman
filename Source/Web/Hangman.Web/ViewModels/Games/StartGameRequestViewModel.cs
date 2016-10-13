namespace Hangman.Web.ViewModels.Games
{
    using System.ComponentModel.DataAnnotations;

    public class StartGameRequestViewModel
    {
        [Display(Name = "Game Type", Description = "Singleplayer or Multiplayer")]
        public GameType GameType { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Game Name")]
        [MinLength(1)]
        [MaxLength(50)]
        public string GameName { get; set; }
    }
}
