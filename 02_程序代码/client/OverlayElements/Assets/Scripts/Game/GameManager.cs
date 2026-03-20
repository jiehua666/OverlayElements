// Game Manager - Main game state and flow controller
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;
using OverlayElements.Card;
using OverlayElements.UI;

namespace OverlayElements.Game
{
    /// <summary>
    /// Game state enumeration
    /// </summary>
    public enum GameState
    {
        NotStarted,
        PlayerTurn,
        OpponentTurn,
        Animating,
        GameOver
    }

    /// <summary>
    /// Main game manager
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private int initialHandSize = 5;
        [SerializeField] private int maxFieldCards = 5;
        [SerializeField] private int playerStartingHealth = 30;
        [SerializeField] private int opponentStartingHealth = 30;

        [Header("Game References")]
        [SerializeField] private CardDatabase cardDatabase;
        [SerializeField] private GameHUD gameHUD;

        [Header("Test Mode")]
        [SerializeField] private bool useTestDeck = true;

        // Game state
        private GameState currentState = GameState.NotStarted;
        private Player player;
        private Player opponent;
        private GameBoard board;
        private TurnManager turnManager;
        private CombatManager combatManager;
        private AIController aiController;

        // References
        private CardAnimationController animationController;

        // Events
        public event Action<GameState> OnGameStateChanged;
        public event Action<Player> OnGameOver;

        // Properties
        public GameState CurrentState => currentState;
        public Player Player => player;
        public Player Opponent => opponent;
        public GameBoard Board => board;
        public TurnManager TurnManager => turnManager;
        public CombatManager Combat => combatManager;

        private void Awake()
        {
            animationController = GetComponent<CardAnimationController>();
            if (animationController == null)
            {
                animationController = gameObject.AddComponent<CardAnimationController>();
            }
        }

        private void Start()
        {
            // Initialize game systems
            InitializeGameSystems();

            // Initialize players
            InitializePlayers();

            // Subscribe to events
            SubscribeToEvents();

            // Start the game
            StartGame();
        }

        /// <summary>
        /// Initialize game systems
        /// </summary>
        private void InitializeGameSystems()
        {
            board = new GameBoard();
            turnManager = new TurnManager();
            combatManager = new CombatManager(board, turnManager);
            aiController = new AIController(combatManager, board);
        }

        /// <summary>
        /// Initialize players
        /// </summary>
        private void InitializePlayers()
        {
            player = new Player("Player", playerStartingHealth, 3);
            opponent = new Player("Opponent", opponentStartingHealth, 3);

            // Create decks
            if (useTestDeck && cardDatabase != null)
            {
                CreateTestDecks();
            }
            else
            {
                CreateDefaultDeck(player);
                CreateDefaultDeck(opponent);
            }

            // Initialize decks
            player.Deck.Initialize();
            opponent.Deck.Initialize();

            Debug.Log($"[GameManager] Players initialized. Player Health: {player.Health}, Opponent Health: {opponent.Health}");
        }

        /// <summary>
        /// Create test decks with available cards
        /// </summary>
        private void CreateTestDecks()
        {
            var allCards = cardDatabase.GetAllCards();
            var playerCards = new List<CardData>();
            var opponentCards = new List<CardData>();

            foreach (var card in allCards)
            {
                // Each player gets 2 copies of each card
                playerCards.Add(card);
                playerCards.Add(card);
                opponentCards.Add(card);
                opponentCards.Add(card);
            }

            var playerDeck = new CardDeck("Player Deck");
            playerDeck.SetMainDeck(playerCards);
            player.SetDeck(playerDeck);

            var opponentDeck = new CardDeck("Opponent Deck");
            opponentDeck.SetMainDeck(opponentCards);
            opponent.SetDeck(opponentDeck);
        }

        /// <summary>
        /// Create default deck
        /// </summary>
        private void CreateDefaultDeck(Player p)
        {
            var deck = new CardDeck(p.Name + " Deck");
            var cards = new List<CardData>();

            // Add default cards
            for (int i = 0; i < 4; i++)
            {
                cards.Add(cardDatabase.GetCardById("FIRE_001"));
                cards.Add(cardDatabase.GetCardById("WATER_001"));
                cards.Add(cardDatabase.GetCardById("WIND_001"));
                cards.Add(cardDatabase.GetCardById("WOOD_001"));
            }

            deck.SetMainDeck(cards);
            p.SetDeck(deck);
        }

        /// <summary>
        /// Subscribe to game events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (turnManager != null)
            {
                turnManager.OnTurnStart += HandleTurnStart;
                turnManager.OnTurnEnd += HandleTurnEnd;
            }

            if (combatManager != null)
            {
                combatManager.OnCardDestroyed += HandleCardDestroyed;
            }
        }

        /// <summary>
        /// Start the game
        /// </summary>
        public void StartGame()
        {
            Debug.Log("[GameManager] Game Starting...");

            // Initial draw
            player.DrawCards(initialHandSize);
            opponent.DrawCards(initialHandSize);

            // Start first turn
            turnManager.StartGame();
            SetGameState(GameState.PlayerTurn);

            Debug.Log("[GameManager] Game Started!");
        }

        /// <summary>
        /// Set game state
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (currentState == newState) return;

            Debug.Log($"[GameManager] State: {currentState} -> {newState}");
            currentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// Handle turn start
        /// </summary>
        private void HandleTurnStart(bool isPlayerTurn)
        {
            if (isPlayerTurn)
            {
                SetGameState(GameState.PlayerTurn);
                player.OnTurnStart();
            }
            else
            {
                SetGameState(GameState.OpponentTurn);
                opponent.OnTurnStart();

                // Trigger AI
                StartCoroutine(AITurnCoroutine());
            }
        }

        /// <summary>
        /// AI turn coroutine
        /// </summary>
        private System.Collections.IEnumerator AITurnCoroutine()
        {
            yield return new WaitForSeconds(1f);

            while (currentState == GameState.OpponentTurn && !aiController.IsTurnComplete())
            {
                aiController.TakeTurn(opponent, player);
                yield return new WaitForSeconds(0.5f);
            }

            // End AI turn
            EndTurn();
        }

        /// <summary>
        /// Handle turn end
        /// </summary>
        private void HandleTurnEnd(bool isPlayerTurn)
        {
            if (isPlayerTurn)
            {
                player.OnTurnEnd();
            }
            else
            {
                opponent.OnTurnEnd();
            }
        }

        /// <summary>
        /// End current turn
        /// </summary>
        public void EndTurn()
        {
            if (currentState != GameState.PlayerTurn) return;

            Debug.Log("[GameManager] Player ending turn...");
            SetGameState(GameState.Animating);
            turnManager.EndTurn();
        }

        /// <summary>
        /// Handle card destroyed
        /// </summary>
        private void HandleCardDestroyed(CardInstance card, int position)
        {
            Debug.Log($"[GameManager] Card destroyed: {card.CardName}");

            // Check win/lose conditions
            CheckGameOver();
        }

        /// <summary>
        /// Check game over conditions
        /// </summary>
        public void CheckGameOver()
        {
            if (!player.IsAlive)
            {
                SetGameState(GameState.GameOver);
                OnGameOver?.Invoke(opponent);
                Debug.Log("[GameManager] GAME OVER - Opponent Wins!");
            }
            else if (!opponent.IsAlive)
            {
                SetGameState(GameState.GameOver);
                OnGameOver?.Invoke(player);
                Debug.Log("[GameManager] GAME OVER - Player Wins!");
            }
        }

        /// <summary>
        /// Get current game info for UI
        /// </summary>
        public string GetGameInfo()
        {
            return $"State: {currentState}\n" +
                   $"Player HP: {player.Health}/{player.MaxHealth} | Energy: {player.Energy}/{player.MaxEnergy}\n" +
                   $"Opponent HP: {opponent.Health}/{opponent.MaxHealth}\n" +
                   $"Player Hand: {player.Deck.HandCount} | Field: {player.Deck.FieldCount}\n" +
                   $"Opponent Hand: {opponent.Deck.HandCount} | Field: {opponent.Deck.FieldCount}";
        }

        private void OnDestroy()
        {
            if (turnManager != null)
            {
                turnManager.OnTurnStart -= HandleTurnStart;
                turnManager.OnTurnEnd -= HandleTurnEnd;
            }
        }
    }
}
