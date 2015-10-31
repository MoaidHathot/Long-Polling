using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LongPolling
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Hostname { get; set; }

        [DataMember]
        public int SetIndex { get; set; }

        [DataMember]
        public int Index { get; set; }

        public User(string hostname, int setIndex, int index)
        {
            this.Hostname = hostname;
            this.SetIndex = setIndex;
            this.Index = index;

            this.ID = string.Format("({0}_{1})#{2}", Hostname, SetIndex, index);
        }


        public override bool Equals(object obj)
        {
            var casted = obj as User;

            if (null == casted)
            {
                return false;
            }
            return this.ID == casted.ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            //return string.Format("ID: '{0}'", ID);
            return ID;
        }
    }
}
