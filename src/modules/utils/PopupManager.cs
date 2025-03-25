using System;
using UnityEngine;
using MBMScripts;

namespace mbm_all_in_one.src.modules.utils
{
    public class PopupManager : MonoBehaviour
    {
        private bool _showPopup = false;
        private Vector2 _scrollPosition = Vector2.zero;
        private int _selectedItemIndex = 0;
        private string _selectedItemAmount = "0";
        private string[] _itemNames;
        private Action<EItemType, int> _onExecute;

        public void Initialize(string[] itemNames, Action<EItemType, int> onExecute)
        {
            _itemNames = itemNames;
            _onExecute = onExecute;
        }

        public void ShowPopup()
        {
            _showPopup = true;
        }

        private void Update()
        {
            if (_showPopup)
            {

                // Close the popup with the Escape key
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _showPopup = false;
                }
            }
        }

        private void OnGUI()
        {
            if (!_showPopup) return;

            // Draw a dark overlay
            GUI.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white; // Reset color

            // Create a centered popup window
            Rect windowRect = new Rect((Screen.width - 300) / 2, (Screen.height - 200) / 2, 300, 200);
            GUI.Window(1, windowRect, PopupWindow, "Select Item");
        }

        private void PopupWindow(int windowID)
        {
            GUILayout.BeginVertical();

            // Scrollable list of items
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(280), GUILayout.Height(100));
            for (int i = 0; i < _itemNames.Length; i++)
            {
                if (GUILayout.Button(_itemNames[i]))
                {
                    _selectedItemIndex = i;
                }
            }
            GUILayout.EndScrollView();

            // Input field for amount
            _selectedItemAmount = GUILayout.TextField(_selectedItemAmount, GUILayout.Width(280));

            // Execute button
            if (GUILayout.Button("Execute") && int.TryParse(_selectedItemAmount, out int amount) && amount > 0)
            {
                var selectedItem = (EItemType)Enum.Parse(typeof(EItemType), _itemNames[_selectedItemIndex]);
                _onExecute?.Invoke(selectedItem, amount);
                _showPopup = false; // Close the popup after execution
            }

            // Close button
            if (GUILayout.Button("Close"))
            {
                _showPopup = false;
            }

            GUILayout.EndVertical();
        }
    }
} 