namespace Hangman.Web.ViewModels.Games.Index
{
    using Hangman.Data.Models;
    using Hangman.Web.Infrastructure.Mapping;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public int Id { get; set; }
    }
}
