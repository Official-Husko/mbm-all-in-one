using System;
using System.Collections.Generic;
using MBMScripts;
using UnityEngine;

namespace mbm_all_in_one.src.modules.utils
{
    public static class ItemUtils
    {
        public static IEnumerable<EItemType> GetAllItemTypes()
        {
            Debug.Log("Found " + Enum.GetValues(typeof(EItemType)).Length + " items");
            return EnumUtils.GetEnumValues<EItemType>();
        }
    }
} 