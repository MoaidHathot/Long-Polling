using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace LongPolling
{
    public interface IResultContext<T> : IDisposable
    {
        void OnCompleted(Action continuation);
        bool IsCompleted { get; }
        bool IsCancelled { get; }
        T GetResult();
        T Result { get; }
        bool SetResult(T result);
        bool SetCancelled();
        void Wait();
        bool Wait(TimeSpan timeout, bool cancelIfTimeout);
    }
}
