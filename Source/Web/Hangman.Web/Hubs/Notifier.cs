namespace Hangman.Web.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    public class Notifier : Hub
    {
        private static object usersLocker = new object();
        private static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();

        public void UpdateGame(IList<string> userIds)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<Notifier>();
            var connectionIds = this.GetConnectionIds(userIds);
            context.Clients.Clients(connectionIds).updateGame();
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

        public override Task OnConnected()
        {
            var userId = this.Context.User.Identity.GetUserId();
            var connectionId = this.Context.ConnectionId;

            this.CacheUser(userId, connectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string x = null;
            var counter = GlobalConstants.ThreadSafeNumberOfTryouts;
            while (!users.TryRemove(this.Context.User.Identity.GetUserId(), out x) || counter > 0)
            {
                counter--;
            }

            return base.OnDisconnected(stopCalled);
        }

        private void CacheUser(string userId, string connectionId)
        {
            if (!users.ContainsKey(userId))
            {
                var counter = GlobalConstants.ThreadSafeNumberOfTryouts;
                while (!users.TryAdd(userId, connectionId) || counter > 0)
                {
                    counter--;
                }
            }
        }
    }
}
