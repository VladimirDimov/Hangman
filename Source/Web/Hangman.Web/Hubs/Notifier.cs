namespace Hangman.Web.Hubs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    public class Notifier : Hub
    {
        private static object usersLocker = new object();
        private static Dictionary<string, string> users = new Dictionary<string, string>();

        public void UpdateGame(IList<string> userIds)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<Notifier>();
            var connectionIds = this.GetConnectionIds(userIds);
            context.Clients.Clients(connectionIds).updateGame();
        }

        public override Task OnConnected()
        {
            var userId = this.Context.User.Identity.GetUserId();
            var connectionId = this.Context.ConnectionId;

            this.CacheUser(userId, connectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            lock (usersLocker)
            {
                users.Remove(this.Context.User.Identity.GetUserId());
            }

            return base.OnDisconnected(stopCalled);
        }

        private void CacheUser(string userId, string connectionId)
        {
            if (!users.ContainsKey(userId))
            {
                lock (usersLocker)
                {
                    users.Add(userId, connectionId);
                }
            }
        }

        private IList<string> GetConnectionIds(IList<string> userIds)
        {
            var connectionIds = new List<string>();
            foreach (var userId in userIds)
            {
                if (users.ContainsKey(userId))
                {
                    connectionIds.Add(users[userId]);
                }
            }

            return connectionIds;
        }
    }
}
