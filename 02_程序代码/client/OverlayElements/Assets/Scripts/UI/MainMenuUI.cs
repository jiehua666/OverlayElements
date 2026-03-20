// Main Menu UI Controller
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OverlayElements.Core;

namespace OverlayElements.UI
{
    /// <summary>
    /// Main menu UI controller
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("Menu Buttons")]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button deckEditorButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [Header("Version Info")]
        [SerializeField] private TextMeshProUGUI versionText;

        [Header("Settings")]
        [SerializeField] private bool useSceneLoading = true;

        private void Start()
        {
            // Button listeners
            if (startGameButton != null)
                startGameButton.onClick.AddListener(OnStartGame);
            
            if (deckEditorButton != null)
                deckEditorButton.onClick.AddListener(OnDeckEditor);
            
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettings);
            
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuit);

            // Version
            if (versionText != null)
            {
                versionText.text = $"v0.1.0";
            }
        }

        /// <summary>
        /// Start new game
        /// </summary>
        private void OnStartGame()
        {
            Debug.Log("[MainMenu] Starting game...");

            if (useSceneLoading)
            {
                // Load Game scene
                var loader = GameSceneLoader.Instance;
                if (loader != null)
                {
                    loader.LoadGameScene();
                }
                else
                {
                    Debug.LogError("[MainMenu] GameSceneLoader not found!");
                }
            }
            else
            {
                // Legacy mode: Just show a debug message
                Debug.Log("[MainMenu] Game would start here. Use scene loading for full experience.");
            }
        }

        /// <summary>
        /// Open deck editor
        /// </summary>
        private void OnDeckEditor()
        {
            Debug.Log("[MainMenu] Deck Editor - Coming soon!");
            // TODO: Implement deck editor
        }

        /// <summary>
        /// Open settings
        /// </summary>
        private void OnSettings()
        {
            Debug.Log("[MainMenu] Settings - Coming soon!");
            // TODO: Implement settings
        }

        /// <summary>
        /// Quit game
        /// </summary>
        private void OnQuit()
        {
            var loader = GameSceneLoader.Instance;
            if (loader != null)
            {
                loader.QuitGame();
            }
            else
            {
                Debug.Log("[MainMenu] Quitting...");
                Application.Quit();

                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
