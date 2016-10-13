namespace Hangman.Web.ViewModels.Games.Index
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Display(Name = "Game Type")]
        public GameType GameType { get; set; }

        public string GameName { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}
