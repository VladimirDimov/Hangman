namespace Hangman.Web.ActiveGames
{
    using System.Collections.Generic;

    public class ActiveGamePlayerModel
    {
        public string Id { get; set; }

        public IEnumerable<char> OpenedPositions { get; set; }

        public int NumberOfErrors { get; set; }

        public int NumberOfGuesses { get; set; }
    }
}
