using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LongPolling;
using LongPolling.Shared;
using LongPolling.Shared.Utilities;

namespace LongPolling.Server.External
{
    public class ExternalNotificationsCenter : NotificationsCenterBase
    {
        private static Lazy<ExternalNotificationsCenter> _instance = new Lazy<ExternalNotificationsCenter>(() => new ExternalNotificationsCenter(Settings.Default.EventsDueTime, Settings.Default.EventsInterval, Settings.Default.EventsPerInterval), true);
        public static ExternalNotificationsCenter Instance { get { return _instance.Value; } }

        public ExternalNotificationsCenter(TimeSpan dueTime, TimeSpan interval, int eventsPerInterval)
            : base(dueTime, interval, eventsPerInterval)
        {

        }

        protected override void DispatchNotifications(List<User> users)
        {
            users.ForEach((user) => LongPollingContext<User, Notification>.Manager.AddResults(user, new Notification("Beat..")));
        }

        protected override void RegisterUser(User user)
        {
 	        
        }

        protected override void UnregisterUser(User user)
        {
            LongPollingContext<User, Notification>.Manager.RemoveResults(user);
        }
    }
}
