using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace LongPolling
{
    public static class LongPollingContext<K, V>
    {
        private static Lazy<IResultContextManager<K, V>> _manager = new Lazy<IResultContextManager<K, V>>(() => new ResultContextManager<K, V>(new TaskResultContextFactory()), true);
        public static IResultContextManager<K, V> Manager { get { return _manager.Value; } }


    }
}
