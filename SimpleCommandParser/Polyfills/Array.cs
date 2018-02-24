using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NET45

namespace System
{
    internal static class ArrayHelper<T>
    {
        public static readonly T[] Empty = new T[0];
    }

    internal class Array
    {
        public static T[] Empty<T>()
        {
            return ArrayHelper<T>.Empty;
        }
    }
}

#endif