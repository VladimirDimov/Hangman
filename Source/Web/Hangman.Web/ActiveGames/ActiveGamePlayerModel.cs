using System.Collections.Generic;

namespace Hangman.Web.ActiveGames
{
    public class ActiveGamePlayerModel
    {
        public string Id { get; set; }

        public IEnumerable<char> OpenedPositions { get; set; }

        public int NumberOfErrors { get; set; }
    }
}
