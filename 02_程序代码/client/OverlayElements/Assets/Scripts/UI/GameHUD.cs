// Game HUD - Displays game state information
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OverlayElements.Game;

namespace OverlayElements.UI
{
    /// <summary>
    /// Game HUD controller
    /// </summary>
    public class GameHUD : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [SerializeField] private TextMeshProUGUI playerEnergyText;
        [SerializeField] private TextMeshProUGUI playerHandCountText;

        [Header("Opponent Info")]
        [SerializeField] private TextMeshProUGUI opponentHealthText;
        [SerializeField] private TextMeshProUGUI opponentHandCountText;

        [Header("Game Info")]
        [SerializeField] private TextMeshProUGUI turnIndicatorText;
        [SerializeField] private TextMeshProUGUI gameInfoText;

        [Header("Controls")]
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button returnToMenuButton;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = true;

        private GameManager gameManager;

        private void Start()
        {
            // Find game manager
            gameManager = FindObjectOfType<GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("[GameHUD] GameManager not found!");
                return;
            }

            // Subscribe to events
            gameManager.OnGameStateChanged += OnGameStateChanged;
            gameManager.OnGameOver += OnGameOver;
            
            if (gameManager.Player != null)
            {
                gameManager.Player.OnHealthChanged += OnPlayerHealthChanged;
                gameManager.Player.OnEnergyChanged += OnPlayerEnergyChanged;
            }

            // Button listeners
            if (endTurnButton != null)
                endTurnButton.onClick.AddListener(OnEndTurn);
            
            if (returnToMenuButton != null)
                returnToMenuButton.onClick.AddListener(OnReturnToMenu);

            // Initial update
            UpdateUI();
        }

        private void OnDestroy()
        {
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
                gameManager.OnGameOver -= OnGameOver;

                if (gameManager.Player != null)
                {
                    gameManager.Player.OnHealthChanged -= OnPlayerHealthChanged;
                    gameManager.Player.OnEnergyChanged -= OnPlayerEnergyChanged;
                }
            }
        }

        private void Update()
        {
            if (showDebugInfo && gameManager != null)
            {
                UpdateDebugInfo();
            }
        }

        /// <summary>
        /// Update all UI elements
        /// </summary>
        public void UpdateUI()
        {
            if (gameManager == null) return;

            var player = gameManager.Player;
            var opponent = gameManager.Opponent;

            if (player != null)
            {
                if (playerHealthText != null)
                    playerHealthText.text = $"HP: {player.Health}/{player.MaxHealth}";
                
                if (playerEnergyText != null)
                    playerEnergyText.text = $"Energy: {player.Energy}/{player.MaxEnergy}";
                
                if (playerHandCountText != null)
                    playerHandCountText.text = $"Hand: {player.Deck.HandCount}";
            }

            if (opponent != null)
            {
                if (opponentHealthText != null)
                    opponentHealthText.text = $"HP: {opponent.Health}/{opponent.MaxHealth}";
                
                if (opponentHandCountText != null)
                    opponentHandCountText.text = $"Hand: {opponent.Deck.HandCount}";
            }

            // Turn indicator
            if (turnIndicatorText != null)
            {
                turnIndicatorText.text = gameManager.CurrentState switch
                {
                    GameState.PlayerTurn => "YOUR TURN",
                    GameState.OpponentTurn => "OPPONENT'S TURN",
                    GameState.Animating => "...",
                    GameState.GameOver => "GAME OVER",
                    _ => "WAITING"
                };
            }

            // End turn button state
            if (endTurnButton != null)
            {
                endTurnButton.interactable = gameManager.CurrentState == GameState.PlayerTurn;
            }
        }

        /// <summary>
        /// Update debug info
        /// </summary>
        private void UpdateDebugInfo()
        {
            if (gameInfoText != null && gameManager != null)
            {
                gameInfoText.text = gameManager.GetGameInfo();
            }
        }

        /// <summary>
        /// On game state changed
        /// </summary>
        private void OnGameStateChanged(GameState newState)
        {
            UpdateUI();
        }

        /// <summary>
        /// On player health changed
        /// </summary>
        private void OnPlayerHealthChanged(int newHealth)
        {
            UpdateUI();
        }

        /// <summary>
        /// On player energy changed
        /// </summary>
        private void OnPlayerEnergyChanged(int newEnergy)
        {
            UpdateUI();
        }

        /// <summary>
        /// On game over
        /// </summary>
        private void OnGameOver(Player winner)
        {
            UpdateUI();
            
            if (turnIndicatorText != null)
            {
                string winnerName = winner == gameManager.Player ? "YOU WIN!" : "YOU LOSE!";
                turnIndicatorText.text = winnerName;
            }

            Debug.Log($"[GameHUD] {winnerName}");
        }

        /// <summary>
        /// End turn button clicked
        /// </summary>
        private void OnEndTurn()
        {
            if (gameManager != null)
            {
                gameManager.EndTurn();
            }
        }

        /// <summary>
        /// Return to menu
        /// </summary>
        private void OnReturnToMenu()
        {
            var loader = Core.GameSceneLoader.Instance;
            if (loader != null)
            {
                loader.LoadMainMenu();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
