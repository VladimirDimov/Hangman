namespace Hangman.Web.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class ConsoleLogger : Hub
    {
        public void Log(string message, string group)
        {
            this.Clients.OthersInGroup(group).Log(message);
        }
    }
}
