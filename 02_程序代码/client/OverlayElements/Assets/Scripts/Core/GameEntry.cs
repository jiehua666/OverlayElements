// Overlay Elements - Game Entry Point
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;

namespace OverlayElements.Core
{
    /// <summary>
    /// Game entry point and basic initialization
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        [Header("Game Configuration")]
        [SerializeField] private string gameVersion = "0.1.0";
        [SerializeField] private bool enableDebugMode = true;

        private void Awake()
        {
            InitializeGame();
        }

        private void Start()
        {
            if (enableDebugMode)
            {
                Debug.Log($"[Overlay Elements] Game Started - Version {gameVersion}");
            }
        }

        private void InitializeGame()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;

            if (enableDebugMode)
            {
                Debug.Log("[Overlay Elements] Game systems initialized");
            }
        }
    }
}
