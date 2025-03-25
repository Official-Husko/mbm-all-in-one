using System.Collections.Generic;
using MBMScripts;
using mbm_all_in_one.src.modules.utils;
using UnityEngine;

namespace mbm_all_in_one.src.modules.cheats
{
    public class ExecuteEventCheat : ICheat, IRegisterableCheat
    {
        public string Name => "Execute Event";
        public CheatType Type => CheatType.ListExecute;
        public Tab DisplayTab => Tab.Events;

        private List<EPlayEventType> _availableEvents;
        private int _selectedEventIndex = 0;

        public ExecuteEventCheat()
        {
            _availableEvents = EventUtils.FetchAvailableEvents();
        }

        public void Execute(int amount)
        {
            if (_selectedEventIndex >= 0 && _selectedEventIndex < _availableEvents.Count)
            {
                EPlayEventType selectedEvent = _availableEvents[_selectedEventIndex];
                // Execute the selected event
                PlayData.Instance.AddPlayEvent(selectedEvent);
                Debug.Log($"Executed event: {selectedEvent}");
            }
            else
            {
                Debug.LogError("Invalid event index selected.");
            }
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }

        public void DrawEventSelector()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Select Event", GUILayout.Width(100));
            int newSelectedIndex = GUILayout.SelectionGrid(
                _selectedEventIndex, 
                _availableEvents.ConvertAll(e => e.ToString()).ToArray(), 
                1, 
                GUILayout.ExpandWidth(true)
            );
            if (newSelectedIndex != _selectedEventIndex)
            {
                _selectedEventIndex = newSelectedIndex;
                Debug.Log($"Selected event index changed to: {_selectedEventIndex}");
            }
            GUILayout.EndVertical();
        }
    }
} 