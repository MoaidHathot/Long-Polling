using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace LongPolling
{
    public class ResultContextManager<K, V> : IResultContextManager<K, V>
    {
        private IResultContextFactory _factory;
        private ConcurrentDictionary<K, ResultContextItem<V>> _contexes = new ConcurrentDictionary<K, ResultContextItem<V>>();

        public ResultContextManager(IResultContextFactory factory)
        {
            _factory = factory;
        }

        public void AddResults(K key, params V[] values)
        {
            _contexes.AddOrUpdate(key, new ResultContextItem<V>(values), (k, v) => { v.AddItems(values); return v; });
        }

        public IResultContext<IEnumerable<V>> GetResultContext(K key)
        {
            var item = _contexes.AddOrUpdate(key, (k) => new ResultContextItem<V>(_factory.CreateContext<IEnumerable<V>>()), (k, v) => { v.UpdateContext(_factory.CreateContext<IEnumerable<V>>()); return v; });

            return item.Context;
        }

        public IEnumerable<V> RemoveResults(K key)
        {
            ResultContextItem<V> item;

            if (_contexes.TryRemove(key, out item) && null != item && null != item.Context && item.Context.IsCompleted)
            {
                return item.Context.Result;
            }

            return new V[] { };
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _factory)
                {
                    _factory.Dispose();
                    _factory = null;
                }

                if (null != _contexes)
                {
                    _contexes.Clear();
                    _contexes = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
