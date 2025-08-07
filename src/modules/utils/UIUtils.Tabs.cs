using System;
using System.Collections.Generic;
using UnityEngine;
using mbm_all_in_one.src;

namespace mbm_all_in_one.src.modules.utils
{
    public static class UITabUtils
    {
        public static void DrawTabButtons(List<TabDefinition> tabs, Tab currentTab, Action<Tab> onTabSelected)
        {
            foreach (var tabDef in tabs)
            {
                GUI.backgroundColor = currentTab == tabDef.Tab ? Color.white : Color.grey;
                if (GUILayout.Button(tabDef.Label))
                    onTabSelected(tabDef.Tab);
            }
        }
    }
}
