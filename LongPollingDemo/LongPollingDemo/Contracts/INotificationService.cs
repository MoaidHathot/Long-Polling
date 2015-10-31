using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace LongPolling
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        void Register(User client);

        [OperationContract(Name = "GetNotifications()")]
        IEnumerable<Notification> GetNotifications(User user);

        [OperationContract(Name = "GetNotifications(User, TimeSpan)")]
        IEnumerable<Notification> GetNotifications(User user, TimeSpan timeout);

        [OperationContract]
        void Unregister(User user);
    }
}
