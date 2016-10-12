using System.Collections.Generic;

namespace Hangman.Web.ActiveGames
{
    public class ActiveGameModel
    {
        public string Word { get; set; }

        public bool IsMultiplayer { get; set; }

        public IEnumerable<ActiveGamePlayerModel> Players { get; set; }

        public ActiveGamePlayerModel Owner { get; set; }

        //public ActiveGamePlayerModel FirstPlayer { get; set; }

        //public ActiveGamePlayerModel SecondPlayer { get; set; }
    }
}
