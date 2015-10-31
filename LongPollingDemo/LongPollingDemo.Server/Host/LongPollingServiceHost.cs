using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace LongPolling.Host
{
    public class LongPollingServiceHost : ServiceHost
    {
        public LongPollingServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var contractDescription in this.ImplementedContracts.Values)
            {
                contractDescription.Behaviors.Add(new LongPollingInstanceProvider());
            }
        }
    }
}
