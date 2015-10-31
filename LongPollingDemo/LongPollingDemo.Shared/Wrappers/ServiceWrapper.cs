using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LongPolling.Shared;

namespace LongPolling.Shared
{
    public static class ServiceWrapper
    {
        public static void Wrap(Action action, string info = "", [CallerMemberName] string name = "")
        {
            var timeWatch = new Stopwatch();

            try
            {
                timeWatch.Start();

                Log.Instance.Info(">> [{0}](Info: '{1}')", name, info);

                action();

                timeWatch.Stop();
            }
            catch (Exception ex)
            {
                Log.Instance.Warning("Exception in [{0}](Info: '{1}'). Exception: '{2}'", name, info, ex);
            }
            finally
            {
                timeWatch.Stop();
            }

            Log.Instance.Info("<< [{0}](Info: '{1}'), Elapsed: '{2}'", name, info, timeWatch.Elapsed);
        }

        public static T Wrap<T>(Func<T> func, string info = "", [CallerMemberName] string name = "")
        {
            var timeWatch = new Stopwatch();
            T result = default(T);

            try
            {
                timeWatch.Start();

                Log.Instance.Info(">> [{0}](Info: '{1}')", name, info);

                result = func();

                timeWatch.Stop();
            }
            catch (Exception ex)
            {
                Log.Instance.Warning("Exception in [{0}](Info: '{1}'). Exception: '{2}'", name, info, ex);
            }
            finally
            {
                timeWatch.Stop();
            }

            Log.Instance.Info("<< [{0}](info: '{1}'), Elapsed: '{2}', Result: '{3}'", name, info, timeWatch.Elapsed, null == result ? "null" :  result is IEnumerable ? ((IEnumerable)result).ToMultiLineString(false) : result.ToString());

            return result;
        }
    }
}
