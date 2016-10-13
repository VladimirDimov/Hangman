namespace Hangman.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Hangman.Data.Common.Models;

    public class Category : BaseModel<int>
    {
        public Category()
        {
            this.Words = new HashSet<Word>();
        }

        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Word> Words { get; private set; }
    }
}
