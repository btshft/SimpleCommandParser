using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NET45

namespace System
{
    internal class Array
    {
        public static T[] Empty<T>()
        {
            return new T[0];
        }
    }
}

#endif