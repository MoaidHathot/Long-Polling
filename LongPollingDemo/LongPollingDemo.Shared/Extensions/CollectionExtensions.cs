using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongPolling.Shared
{
    public static class CollectionExtensions
    {
        //public static string ToSingleLineString<T>(this IEnumerable<T> items)
        //{
        //    var builder = new StringBuilder();

        //    if (null != items)
        //    {
        //        foreach (var item in items)
        //        {
        //            builder.AppendFormat("{0}{1}", 0 < builder.Length ? ", " : "", item);
        //        }
        //    }

        //    return builder.ToString();
        //}

        public static string ToSingleLineStrin(this IEnumerable items)
        {
            var builder = new StringBuilder();

            if (null != items)
            {
                foreach (var item in items)
                {
                    builder.AppendFormat("{0}{1}", 0 < builder.Length ? ", " : "", item);
                }
            }

            return builder.ToString();
        }

        //public static string ToMultiLineString<T>(this IEnumerable<T> items, bool startFromNextLine = true)
        //{
        //    var builder = new StringBuilder();

        //    if (startFromNextLine)
        //    {
        //        builder.AppendLine();
        //    }

        //    if (null != items)
        //    {
        //        foreach (var item in items)
        //        {
        //            builder.AppendLine(item.ToString());
        //        }
        //    }

        //    return builder.ToString();
        //}

        public static string ToMultiLineString(this IEnumerable items, bool startFromNextLine = true)
        {
            var builder = new StringBuilder();

            if (startFromNextLine)
            {
                builder.AppendLine();
            }

            if (null != items)
            {
                foreach (var item in items)
                {
                    builder.AppendLine(item.ToString());
                }
            }

            return builder.ToString();
        }
    }
}
