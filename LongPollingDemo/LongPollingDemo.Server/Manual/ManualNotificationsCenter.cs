using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using LongPolling;
using LongPolling.Shared;
using LongPolling.Shared.Utilities;

namespace LongPolling.Server.Manual
{
    public class ManualNotificationsCenter : NotificationsCenterBase
    {
        private static Lazy<ManualNotificationsCenter> _instance = new Lazy<ManualNotificationsCenter>(() => new ManualNotificationsCenter(Settings.Default.EventsDueTime, Settings.Default.EventsInterval, Settings.Default.EventsPerInterval), true);
        public static ManualNotificationsCenter Instance { get { return _instance.Value; } }

        
        private ResultContextManager<User, Notification> _contextManager;


        private ManualNotificationsCenter(TimeSpan dueTime, TimeSpan interval, int eventsPerInterval)
            : base(dueTime, interval, eventsPerInterval)
        {
            _contextManager = new ResultContextManager<User, Notification>(new TaskResultContextFactory());
        }

        protected override void DispatchNotifications(List<User> users)
        {
            users.ForEach(u => _contextManager.AddResults(u, new Notification("Beat...")));
        }


        public IResultContext<IEnumerable<Notification>> GetNotifications(User user)
        {
            return _contextManager.GetResultContext(user);
        }

        protected override void RegisterUser(User user)
        {

        }

        protected override void UnregisterUser(User user)
        {
            _contextManager.RemoveResults(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _contextManager)
                {
                    _contextManager.Dispose();
                    _contextManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
