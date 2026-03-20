// Game Manager - Main game state controller
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;
using OverlayElements.Card;

namespace OverlayElements.Game
{
    /// <summary>
    /// Main game manager singleton
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game Settings")]
        [SerializeField] private int initialHandSize = 5;
        [SerializeField] private int maxFieldCards = 5;
        [SerializeField] private int maxDeckSize = 40;
        [SerializeField] private int startingHealth = 30;
        [SerializeField] private int startingEnergy = 3;

        [Header("References")]
        [SerializeField] private CardDatabase cardDatabase;
        
        // Game state
        private GameState gameState;
        private TurnManager turnManager;
        private List<Player> players = new List<Player>();
        private int currentPlayerIndex;
        private int turnNumber;

        // Events
        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnTurnStarted;
        public event Action<Player> OnPlayerWon;
        public event Action OnGameOver;

        public GameState CurrentState => gameState;
        public TurnManager TurnManager => turnManager;
        public Player CurrentPlayer => players[currentPlayerIndex];
        public Player OpponentPlayer => players[1 - currentPlayerIndex];
        public int TurnNumber => turnNumber;
        public CardDatabase CardDatabase => cardDatabase;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            turnManager = new TurnManager();
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartGame()
        {
            Debug.Log("[GameManager] Starting new game...");
            
            // Initialize players
            players.Clear();
            players.Add(new Player("Player 1", startingHealth, startingEnergy));
            players.Add(new Player("Player 2 (AI)", startingHealth, startingEnergy));
            
            // Initialize decks
            foreach (var player in players)
            {
                var deck = new CardDeck(player.Name);
                if (cardDatabase != null)
                {
                    deck.SetMainDeck(cardDatabase.CreateRandomDeck(maxDeckSize));
                }
                deck.Initialize();
                player.SetDeck(deck);
            }
            
            currentPlayerIndex = 0;
            turnNumber = 0;
            
            SetGameState(GameState.Starting);
            SetGameState(GameState.PlayerTurn);
            
            Debug.Log("[GameManager] Game started!");
        }

        /// <summary>
        /// End current player's turn
        /// </summary>
        public void EndTurn()
        {
            if (gameState != GameState.PlayerTurn) return;
            
            Debug.Log($"[GameManager] Player {CurrentPlayer.Name} ends turn");
            
            // End turn effects
            CurrentPlayer.OnTurnEnd();
            
            // Switch player
            currentPlayerIndex = 1 - currentPlayerIndex;
            turnNumber++;
            
            // Start new turn
            SetGameState(GameState.EndingTurn);
            SetGameState(GameState.PlayerTurn);
            
            OpponentPlayer.OnTurnStart();
            OnTurnStarted?.Invoke(currentPlayerIndex);
        }

        /// <summary>
        /// Check win/lose conditions
        /// </summary>
        public void CheckGameOver()
        {
            foreach (var player in players)
            {
                if (player.Health <= 0)
                {
                    var winner = players.Find(p => p != player);
                    OnGameOver?.Invoke();
                    OnPlayerWon?.Invoke(winner);
                    SetGameState(GameState.GameOver);
                    Debug.Log($"[GameManager] Game Over! Winner: {winner.Name}");
                    return;
                }
            }
        }

        /// <summary>
        /// Set game state
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (gameState == newState) return;
            
            Debug.Log($"[GameManager] State: {gameState} → {newState}");
            gameState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// Get player by index
        /// </summary>
        public Player GetPlayer(int index)
        {
            return index >= 0 && index < players.Count ? players[index] : null;
        }

        /// <summary>
        /// Quit game
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("[GameManager] Quitting game...");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    /// <summary>
    /// Game state machine
    /// </summary>
    public enum GameState
    {
        Idle,           // Initial state
        Starting,        // Game is starting
        PlayerTurn,      // Active player's turn
        EnemyTurn,       // AI turn
        EndingTurn,      // Turn ending animation
        Paused,         // Game paused
        GameOver        // Game ended
    }
}
