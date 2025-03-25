using System;
using System.Collections.Generic;
using MBMScripts;

namespace mbm_all_in_one.src.modules.utils
{
    public static class EventUtils
    {
        public static List<EPlayEventType> FetchAvailableEvents()
        {
            List<EPlayEventType> availableEvents = new List<EPlayEventType>();

            // Get all enum values from EPlayEventType
            Type enumType = typeof(EPlayEventType);
            if (enumType.IsEnum)
            {
                Array enumValues = Enum.GetValues(enumType);
                foreach (var value in enumValues)
                {
                    if ((EPlayEventType)value != EPlayEventType.None)
                    {
                        availableEvents.Add((EPlayEventType)value);
                    }
                }
            }

            return availableEvents;
        }
    }
} 