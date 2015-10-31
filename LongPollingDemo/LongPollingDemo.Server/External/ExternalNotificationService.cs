using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LongPolling;
using LongPolling.Shared;
using LongPolling.Shared.Utilities;
using System.ServiceModel;

namespace LongPolling.Server.External
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [WorkerThreadPoolBehavior]
    public class ExternalNotificationService : INotificationService
    {
        public void Register(User user)
        {
            ServiceWrapper.Wrap(() =>
            {
                ExternalNotificationsCenter.Instance.Register(user);

            }, string.Format("User: '{0}'", user));
        }

        public IEnumerable<Notification> GetNotifications(User user)
        {
            return ServiceWrapper.Wrap(() =>
            {
                return GetNotifications(user, TimeSpan.FromMinutes(2));

            }, string.Format("User: '{0}'", user));
        }

        public IEnumerable<Notification> GetNotifications(User user, TimeSpan timeout)
        {
            return ServiceWrapper.Wrap(() =>
            {
                var context = LongPollingContext<User, Notification>.Manager.GetResultContext(user);

                if (!context.Wait(timeout, true))
                {
                    return new Notification[] { };
                }

                return context.Result;

            }, string.Format("User: '{0}'", user));
        }

        public void Unregister(User user)
        {
            ServiceWrapper.Wrap(() =>
            {
                ExternalNotificationsCenter.Instance.Unregister(user);

            }, string.Format("User: '{0}'", user));
        }
    }
}
