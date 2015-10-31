using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LongPolling
{
    [DataContract]
    [Serializable]
    public class Notification
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }

        public Notification(string message, DateTime timestmap)
        {
            Message = message;
            TimeStamp = timestmap;
        }

        public Notification(string message)
            : this(message, DateTime.UtcNow)
        {

        }

        public override string ToString()
        {
            return string.Format("Message: '{0}', TimeStamp: '{1}'", Message, TimeStamp);
        }
    }
}
