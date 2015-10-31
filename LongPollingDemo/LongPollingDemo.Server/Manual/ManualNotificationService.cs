using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using LongPolling;
using LongPolling.Shared;

namespace LongPolling.Server.Manual
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [WorkerThreadPoolBehavior]
    public class ManualNotificationService : INotificationService
    {
        public void Register(User user)
        {
            ManualNotificationsCenter.Instance.Register(user);
        }

        public IEnumerable<Notification> GetNotifications(User user)
        {
            return GetNotifications(user, TimeSpan.FromMinutes(2));
        }

        public IEnumerable<Notification> GetNotifications(User user, TimeSpan timeout)
        {
            var context = ManualNotificationsCenter.Instance.GetNotifications(user);

            if (!context.Wait(timeout, true))
            {
                return new Notification[] { };
            }

            return context.Result;
        }

        public void Unregister(User user)
        {
            ManualNotificationsCenter.Instance.Unregister(user);
        }
    }
}
