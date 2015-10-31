using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling
{
    public interface IResultContextManager<K, V> : IDisposable
    {
        void AddResults(K key, params V[] values);
        IResultContext<IEnumerable<V>> GetResultContext(K key);
        IEnumerable<V> RemoveResults(K key);
    }
}
