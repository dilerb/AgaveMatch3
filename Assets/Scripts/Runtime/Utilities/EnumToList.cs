using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Utilities
{
    public static class EnumToList
    {
        public static List<T> Convert<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}