using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling
{
    public interface IResultContextFactory : IDisposable
    {
        IResultContext<T> CreateContext<T>();
    }
}
