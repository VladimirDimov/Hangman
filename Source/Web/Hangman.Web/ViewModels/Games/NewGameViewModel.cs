namespace Hangman.Web.ViewModels.Games
{
    using System.Collections.Generic;

    public class NewGameViewModel
    {
        public List<char> OpenedPositions { get; set; }

        public int NumberOfErrors { get; set; }

        public bool IsMultiplayer { get; set; }

        public string GameId { get; set; }

        public string Description { get; set; }
    }
}
