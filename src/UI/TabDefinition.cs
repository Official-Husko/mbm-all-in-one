using System;
using mbm_all_in_one.src.modules.utils;

namespace mbm_all_in_one.src
{
    public class TabDefinition
    {
        public Tab Tab { get; }
        public string Label { get; }
        public Action DrawContent { get; }

        public TabDefinition(Tab tab, string label, Action drawContent)
        {
            Tab = tab;
            Label = label;
            DrawContent = drawContent;
        }
    }
}
