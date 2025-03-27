using System;
using System.Collections.Generic;
using System.Linq;
using MBMScripts;

namespace mbm_all_in_one.src.modules.utils
{
    public static class EventUtils
    {
        public static List<EPlayEventType> FetchAvailableEvents()
        {
            return EnumUtils.GetEnumValues<EPlayEventType>().ToList();
        }
    }
} 