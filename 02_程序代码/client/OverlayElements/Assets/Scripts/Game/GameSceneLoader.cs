// Game Scene Loader - Loads game scene from main menu
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;
using UnityEngine.SceneManagement;

namespace OverlayElements.Core
{
    /// <summary>
    /// Handles scene transitions
    /// </summary>
    public class GameSceneLoader : MonoBehaviour
    {
        public static GameSceneLoader Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Load game scene
        /// </summary>
        public void LoadGameScene()
        {
            Debug.Log("[GameSceneLoader] Loading Game Scene...");
            SceneManager.LoadScene("Game");
        }

        /// <summary>
        /// Load main menu scene
        /// </summary>
        public void LoadMainMenu()
        {
            Debug.Log("[GameSceneLoader] Loading Main Menu...");
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Restart current game
        /// </summary>
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Quit application
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameSceneLoader] Quitting game...");
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
