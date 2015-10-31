using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling
{
    public class TaskResultContextFactory : IResultContextFactory
    {
        public IResultContext<T> CreateContext<T>()
        {
            return new TaskResultContext<T>();
        }

        public void Dispose()
        {
            
        }
    }
}
