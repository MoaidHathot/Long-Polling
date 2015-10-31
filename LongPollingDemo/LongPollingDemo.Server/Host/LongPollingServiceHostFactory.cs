using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace LongPolling.Host
{
    public class LongPollingServiceHostFactory<T> : ServiceHostFactory
    {
        public LongPollingServiceHostFactory()
        {

        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new LongPollingServiceHost(serviceType, baseAddresses);
        }
    }
}
