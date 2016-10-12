namespace Hangman.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Hangman.Data.Common.Models;

    public class Word : BaseModel<int>
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Content { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
