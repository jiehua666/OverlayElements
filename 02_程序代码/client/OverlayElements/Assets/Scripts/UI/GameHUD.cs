// Game HUD - In-game user interface
// Version: 0.1.0
// Created: 2026-03-20

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OverlayElements.Card;
using OverlayElements.Game;

namespace OverlayElements.UI
{
    /// <summary>
    /// Game HUD - manages all in-game UI elements
    /// </summary>
    public class GameHUD : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField] private TextMeshProUGUI playerHealthText;
        [SerializeField] private TextMeshProUGUI playerEnergyText;
        [SerializeField] private TextMeshProUGUI opponentHealthText;
        [SerializeField] private TextMeshProUGUI opponentEnergyText;
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private TextMeshProUGUI deckCountText;

        [Header("Card Areas")]
        [SerializeField] private Transform handArea;
        [SerializeField] private Transform playerFieldArea;
        [SerializeField] private Transform opponentFieldArea;
        [SerializeField] private GameObject cardPrefab;

        [Header("Buttons")]
        [SerializeField] private Button endTurnButton;
        [SerializeField] private Button menuButton;

        [Header("Info Panels")]
        [SerializeField] private GameObject turnIndicator;
        [SerializeField] private TextMeshProUGUI phaseText;

        // References
        private GameManager gameManager;
        private List<CardUI> handCards = new List<CardUI>();
        private List<CardUI> playerFieldCards = new List<CardUI>();
        private List<CardUI> opponentFieldCards = new List<CardUI>();

        private void Start()
        {
            gameManager = GameManager.Instance;
            
            if (gameManager != null)
            {
                // Subscribe to events
                gameManager.OnGameStateChanged += OnGameStateChanged;
                gameManager.OnTurnStarted += OnTurnStarted;
                gameManager.OnPlayerWon += OnPlayerWon;
            }

            // Button events
            endTurnButton?.onClick.AddListener(OnEndTurnClicked);
            menuButton?.onClick.AddListener(OnMenuClicked);

            // Initial state
            UpdateUI();
        }

        private void OnDestroy()
        {
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= OnGameStateChanged;
                gameManager.OnTurnStarted -= OnTurnStarted;
                gameManager.OnPlayerWon -= OnPlayerWon;
            }
        }

        /// <summary>
        /// Update all UI elements
        /// </summary>
        public void UpdateUI()
        {
            if (gameManager == null) return;

            var player = gameManager.CurrentPlayer;
            var opponent = gameManager.OpponentPlayer;

            // Player info
            playerHealthText.text = $"{player.Health}/{player.MaxHealth}";
            playerEnergyText.text = $"{player.Energy}/{player.MaxEnergy}";

            // Opponent info
            opponentHealthText.text = $"{opponent.Health}/{opponent.MaxHealth}";
            opponentEnergyText.text = $"{opponent.Energy}/{opponent.MaxEnergy}";

            // Turn info
            turnText.text = $"Turn {gameManager.TurnNumber}";

            // Deck count
            deckCountText.text = $"Deck: {player.Deck.DrawPileCount}";

            // Phase
            phaseText.text = gameManager.CurrentState.ToString();

            // Update card displays
            UpdateHandCards();
            UpdateFieldCards();
        }

        /// <summary>
        /// Update hand cards display
        /// </summary>
        private void UpdateHandCards()
        {
            if (gameManager == null) return;

            var player = gameManager.CurrentPlayer;

            // Remove excess cards
            while (handCards.Count > player.Deck.HandCount)
            {
                var card = handCards[handCards.Count - 1];
                handCards.RemoveAt(handCards.Count - 1);
                Destroy(card.gameObject);
            }

            // Add new cards
            while (handCards.Count < player.Deck.HandCount)
            {
                var cardUI = CreateCardUI(handArea);
                handCards.Add(cardUI);
            }

            // Update card data
            for (int i = 0; i < handCards.Count; i++)
            {
                var cardInstance = player.Deck.Hand[i];
                handCards[i].SetCard(cardInstance);
                handCards[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Update field cards display
        /// </summary>
        private void UpdateFieldCards()
        {
            if (gameManager == null) return;

            var player = gameManager.CurrentPlayer;
            var opponent = gameManager.OpponentPlayer;

            // Player field
            UpdateFieldArea(playerFieldCards, player.Deck.Field, playerFieldArea);

            // Opponent field (hide card details)
            UpdateFieldArea(opponentFieldCards, opponent.Deck.Field, opponentFieldArea);
        }

        private void UpdateFieldArea(List<CardUI> cardList, IReadOnlyList<CardInstance> instances, Transform area)
        {
            // Remove excess
            while (cardList.Count > instances.Count)
            {
                var card = cardList[cardList.Count - 1];
                cardList.RemoveAt(cardList.Count - 1);
                Destroy(card.gameObject);
            }

            // Add new
            while (cardList.Count < instances.Count)
            {
                var cardUI = CreateCardUI(area);
                cardList.Add(cardUI);
            }

            // Update
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i].SetCard(instances[i]);
                cardList[i].UpdateDisplay();
            }
        }

        /// <summary>
        /// Create a card UI element
        /// </summary>
        private CardUI CreateCardUI(Transform parent)
        {
            var go = Instantiate(cardPrefab, parent);
            go.transform.localScale = Vector3.one;
            return go.GetComponent<CardUI>();
        }

        // === Event Handlers ===

        private void OnGameStateChanged(GameState state)
        {
            UpdateUI();
            
            if (state == GameState.GameOver)
            {
                ShowGameOver();
            }
        }

        private void OnTurnStarted(int playerIndex)
        {
            UpdateUI();
            ShowTurnIndicator(playerIndex == 0 ? "Your Turn" : "Enemy Turn");
        }

        private void OnPlayerWon(Player winner)
        {
            Debug.Log($"Player won: {winner.Name}");
        }

        private void OnEndTurnClicked()
        {
            gameManager?.EndTurn();
        }

        private void OnMenuClicked()
        {
            // Show pause menu
            Time.timeScale = 0;
        }

        /// <summary>
        /// Show turn indicator
        /// </summary>
        private void ShowTurnIndicator(string message)
        {
            if (turnIndicator != null)
            {
                turnIndicator.SetActive(true);
                var text = turnIndicator.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    text.text = message;
                }
                // Auto hide after 2 seconds
                Invoke(nameof(HideTurnIndicator), 2f);
            }
        }

        private void HideTurnIndicator()
        {
            if (turnIndicator != null)
            {
                turnIndicator.SetActive(false);
            }
        }

        /// <summary>
        /// Show game over screen
        /// </summary>
        private void ShowGameOver()
        {
            // TODO: Show game over panel
            Debug.Log("Game Over!");
        }
    }
}
