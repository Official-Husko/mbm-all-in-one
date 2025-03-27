using System;
using System.Collections.Generic;
using MBMScripts;

namespace mbm_all_in_one.src.modules.utils
{
    public static class LikeabilitiesUtils
    {
        public static List<ELikeability> FetchAvailableLikeabilities()
        {
            List<ELikeability> availableLikeabilities = new List<ELikeability>();

            // Get all enum values from EPlayEventType
            Type enumType = typeof(ELikeability);
            if (enumType.IsEnum)
            {
                Array enumValues = Enum.GetValues(enumType);
                foreach (var value in enumValues)
                {
                    if ((ELikeability)value != ELikeability.None)
                    {
                        availableLikeabilities.Add((ELikeability)value);
                    }
                }
            }

            return availableLikeabilities;
        }
    }
} 