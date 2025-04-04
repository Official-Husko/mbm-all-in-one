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
        private Vector2 _scrollPosition = Vector2.zero; // Persistent scroll position

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
                GameManager.Instance.AddSystemMessage($"Event {selectedEvent} executed");
            }
            else
            {
                Debug.LogError("Invalid event index selected.");
                GameManager.Instance.AddSystemMessage("Invalid event index selected.");
            }
        }

        public void Register(CheatManager cheatManager)
        {
            cheatManager.RegisterCheat(this);
        }

        public void DrawEventSelector()
        {
            GUILayout.BeginVertical();
            
            // Ensure the scroll view has a fixed height to enable scrolling
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(150), GUILayout.ExpandWidth(true));
            
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
            
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
} 