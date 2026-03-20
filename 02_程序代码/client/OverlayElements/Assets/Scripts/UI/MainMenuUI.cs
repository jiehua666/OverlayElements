// Main Menu UI Controller
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OverlayElements.Game;

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

        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameHUDPrefab;

        [Header("Version Info")]
        [SerializeField] private TextMeshProUGUI versionText;

        private void Start()
        {
            // Button listeners
            startGameButton?.onClick.AddListener(OnStartGame);
            deckEditorButton?.onClick.AddListener(OnDeckEditor);
            settingsButton?.onClick.AddListener(OnSettings);
            quitButton?.onClick.AddListener(OnQuit);

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

            // Hide main menu
            if (mainMenuPanel != null)
            {
                mainMenuPanel.SetActive(false);
            }

            // Show game HUD
            if (gameHUDPrefab != null)
            {
                Instantiate(gameHUDPrefab);
            }

            // Start game
            var gameManager = FindObjectOfType<GameManager>();
            gameManager?.StartGame();
        }

        /// <summary>
        /// Open deck editor
        /// </summary>
        private void OnDeckEditor()
        {
            Debug.Log("[MainMenu] Deck Editor - Not implemented yet");
            // TODO: Implement deck editor
        }

        /// <summary>
        /// Open settings
        /// </summary>
        private void OnSettings()
        {
            Debug.Log("[MainMenu] Settings - Not implemented yet");
            // TODO: Implement settings
        }

        /// <summary>
        /// Quit game
        /// </summary>
        private void OnQuit()
        {
            Debug.Log("[MainMenu] Quitting...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
