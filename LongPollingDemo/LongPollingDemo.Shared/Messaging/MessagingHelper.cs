using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LongPolling;

namespace LongPolling.Shared.Messaging
{
    public static class MessagingHelper
    {
        public static string GenerateQueue(User user)
        {
            return string.Format("{0}_{1}", user.Hostname, user.SetIndex);
        }

        public static string GenerateCorrelation(User user)
        {
            return user.ID;
        }
    }
}
