using System;
using UnityEngine;
using MBMScripts;

namespace mbm_all_in_one.src.modules.utils
{
    public class PopupManager : MonoBehaviour
    {
        private bool _showPopup;
        private Vector2 _scrollPosition = Vector2.zero;
        private int _selectedItemIndex = 0;
        private string _selectedItemAmount = "1";
        private string[] _itemNames;
        private Action<EItemType, int> _onExecute;
        private Rect _popupRect = new Rect((Screen.width - 320) / 2, (Screen.height - 240) / 2, 320, 240);

        public void Initialize(string[] itemNames, Action<EItemType, int> onExecute)
        {
            _itemNames = itemNames;
            _onExecute = onExecute;
        }

        public void ShowPopup()
        {
            _showPopup = true;
            // Stop game input
            GameManager.Instance.StopCamera = true;
            GameManager.Instance.StopWheel = true;
        }

        private void Update()
        {
            if (_showPopup)
            {
                // Close the popup with the Escape key
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ClosePopup();
                }
            }
        }

        private void OnGUI()
        {
            if (!_showPopup) return;

            // Draw a semi-transparent black overlay
            GUI.color = new Color(0, 0, 0, 0.5f); // Semi-transparent black
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white; // Reset color

            // Center the popup on the screen
            GUILayout.BeginArea(_popupRect);

            // Apply the same style as the mod menu
            GUIStyle popupStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.6f)) }, // Semi-transparent dark background
                padding = new RectOffset(10, 10, 10, 10),
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };

            GUILayout.BeginVertical(popupStyle);

            // Scrollable list of items
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(280), GUILayout.Height(100));
            for (int i = 0; i < _itemNames.Length; i++)
            {
                if (_itemNames[i] != "None") // Exclude "None" item
                {
                    if (GUILayout.Button(_itemNames[i]))
                    {
                        _selectedItemIndex = i;
                    }
                }
            }
            GUILayout.EndScrollView();

            GUILayout.Space(10); // Add space between the list and the input

            // Input field for amount with label
            GUILayout.BeginHorizontal();
            GUILayout.Label("Amount:", GUILayout.Width(60));
            _selectedItemAmount = GUILayout.TextField(_selectedItemAmount, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.Space(10); // Add space between the input and the buttons

            // Execute button
            if (GUILayout.Button("Execute") && int.TryParse(_selectedItemAmount, out int amount) && amount > 0)
            {
                var selectedItem = (EItemType)Enum.Parse(typeof(EItemType), _itemNames[_selectedItemIndex]);
                _onExecute?.Invoke(selectedItem, amount);
                ClosePopup();
            }

            // Close button
            if (GUILayout.Button("Close"))
            {
                ClosePopup();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void ClosePopup()
        {
            _showPopup = false;
            // Re-enable game input
            GameManager.Instance.StopCamera = false;
            GameManager.Instance.StopWheel = false;
        }

        // Helper method to create a texture with a specific color
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
} 