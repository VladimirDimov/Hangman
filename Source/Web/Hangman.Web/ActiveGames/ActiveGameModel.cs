namespace Hangman.Web.ActiveGames
{
    using System.Collections.Generic;

    public class ActiveGameModel
    {
        public string GameName { get; set; }

        public string Word { get; set; }

        public string Description { get; set; }

        public bool IsMultiplayer { get; set; }

        public IList<ActiveGamePlayerModel> Players { get; set; }

        public ActiveGamePlayerModel Owner { get; set; }

        public string WinnerId { get; set; }

        public GameStatus GameStatus { get; set; }

        public string GameId { get; set; }
    }
}
