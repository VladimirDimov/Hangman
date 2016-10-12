namespace MvcTemplate.Web.App_Start
{
    using System.Data.Entity;
    using Hangman.Data;
    using Hangman.Data.Migrations;

    public static class DatabaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            ApplicationDbContext.Create().Database.Initialize(true);
        }
    }
}
