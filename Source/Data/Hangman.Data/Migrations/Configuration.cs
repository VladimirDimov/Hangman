namespace Hangman.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using MvcTemplate.Data.Models;
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            this.SeedCategoriesWithWords(context);
            this.SeedUsers(context);
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            for (int i = 1; i <= 10; i++)
            {
                var email = $"user{i}@site.com";
                var user = new User
                {
                    Email = email,
                    UserName = email,
                    PasswordHash = new PasswordHasher().HashPassword("email"),
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                userManager.Create<User, string>(user);
            }

            context.SaveChanges();
        }

        private void SeedCategoriesWithWords(ApplicationDbContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }

            var animalsCategory = new Category { Name = "Animals" };
            animalsCategory.Words.Add(new Word { Content = "Tiger", Description = "A big cat who lives in Asia." });
            animalsCategory.Words.Add(new Word { Content = "Elephant", Description = "Animal with long trunk." });
            context.Categories.Add(animalsCategory);

            var townsCategory = new Category { Name = "Towns" };
            townsCategory.Words.Add(new Word { Content = "Stara Zagora", Description = "A town in Bulgaria." });
            townsCategory.Words.Add(new Word { Content = "Berlin", Description = "A town in Germany" });
            context.Categories.Add(townsCategory);

            context.SaveChanges();
        }
    }
}
