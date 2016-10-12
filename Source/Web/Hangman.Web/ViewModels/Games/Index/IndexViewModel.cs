namespace Hangman.Web.ViewModels.Games.Index
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> Categories { get; set; }

        public GameType GameType { get; set; }

        public string GameName { get; set; }

        public int CategoryId { get; set; }
    }
}
