using System;
using System.Collections.Generic;
using System.Linq;

namespace mbm_all_in_one.src.modules.utils
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Where(e => !e.Equals(default(T)));
        }
    }
} 