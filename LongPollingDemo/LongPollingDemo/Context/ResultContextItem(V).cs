using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling
{
    public class ResultContextItem<V>
    {
        private object _syncLock = new object();
        private Queue<V> _queue = new Queue<V>();

        private IResultContext<IEnumerable<V>> _context;

        public IResultContext<IEnumerable<V>> Context { get { return _context; } }

        public ResultContextItem(params V[] items)
            : this(null, items)
        {

        }

        public ResultContextItem(IResultContext<IEnumerable<V>> context)
            : this(context, null)
        {
            
        }

        public ResultContextItem(IResultContext<IEnumerable<V>> context, params V[] items)
        {
            _context = context;

            if (null != items && 0 < items.Length)
            {
                foreach (var item in items)
                {
                    _queue.Enqueue(item);
                }
            }
        }

        public void AddItems(params V[] notifications)
        {
            lock (_syncLock)
            {
                foreach (var notification in notifications)
                {
                    _queue.Enqueue(notification);
                }

                UpdateTask();
            }
        }

        public void UpdateContext(IResultContext<IEnumerable<V>> context)
        {
            lock (_syncLock)
            {
                _context = context;
                UpdateTask();
            }
        }

        // should be called under a lock
        private void UpdateTask()
        {
            if (null != _context && 0 < _queue.Count)
            {
                if (!_context.IsCancelled)
                {
                    _context.SetResult(_queue);

                    _queue = new Queue<V>();
                }

                //_context = null;
            }
        }

        public void Dispose()
        {
            if (null != _context)
            {
                if (!_context.IsCancelled && !_context.IsCompleted && 0 < _queue.Count)
                {
                    _context.SetResult(_queue.ToArray());
                }

                _context.Dispose();
                _context = null;
            }
        }
    }
}
