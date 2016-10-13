namespace Hangman.Web.Hubs
{
    using System.Collections.Generic;
    using Microsoft.AspNet.SignalR;

    public class Notifier : Hub
    {
        public void UpdateGame(IList<string> userIds)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<Notifier>();
            context.Clients.All.updateGame();
            //context.Clients.Users(userIds).updateGame();
        }
    }
}
