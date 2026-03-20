// Turn Manager - Turn phase and timing controller
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OverlayElements.Game
{
    /// <summary>
    /// Turn manager - controls turn phases and timing
    /// </summary>
    public class TurnManager
    {
        private TurnPhase currentPhase;
        private float phaseTimer;
        private bool isProcessing;

        public TurnPhase CurrentPhase => currentPhase;
        public bool IsProcessing => isProcessing;

        // Events
        public event Action<TurnPhase> OnPhaseChanged;
        public event Action OnTurnStart;
        public event Action OnTurnEnd;
        public event Action OnPhaseStart;
        public event Action OnPhaseEnd;

        public TurnManager()
        {
            currentPhase = TurnPhase.Idle;
        }

        /// <summary>
        /// Start a new turn
        /// </summary>
        public void StartTurn()
        {
            if (isProcessing) return;
            
            Debug.Log("[TurnManager] Starting new turn");
            isProcessing = true;
            
            // Execute turn start sequence
            SetPhase(TurnPhase.Start);
        }

        /// <summary>
        /// Process phases in order
        /// </summary>
        public void ProcessPhase(float deltaTime = 0)
        {
            if (!isProcessing) return;

            switch (currentPhase)
            {
                case TurnPhase.Start:
                    OnPhaseStart?.Invoke();
                    SetPhase(TurnPhase.Draw);
                    break;

                case TurnPhase.Draw:
                    // Draw card handled by Player
                    SetPhase(TurnPhase.Main);
                    break;

                case TurnPhase.Main:
                    // Main phase - player actions
                    OnPhaseStart?.Invoke();
                    // Phase stays until player ends turn
                    break;

                case TurnPhase.End:
                    OnPhaseEnd?.Invoke();
                    OnTurnEnd?.Invoke();
                    isProcessing = false;
                    SetPhase(TurnPhase.Idle);
                    break;
            }
        }

        /// <summary>
        /// End current turn
        /// </summary>
        public void EndTurn()
        {
            if (currentPhase == TurnPhase.End || currentPhase == TurnPhase.Idle)
                return;

            Debug.Log("[TurnManager] Ending turn");
            SetPhase(TurnPhase.End);
            ProcessPhase();
        }

        /// <summary>
        /// Set current phase
        /// </summary>
        public void SetPhase(TurnPhase newPhase)
        {
            if (currentPhase == newPhase) return;

            Debug.Log($"[TurnManager] Phase: {currentPhase} → {newPhase}");
            currentPhase = newPhase;
            OnPhaseChanged?.Invoke(newPhase);
        }

        /// <summary>
        /// Get phase duration (for timed phases)
        /// </summary>
        public float GetPhaseDuration(TurnPhase phase)
        {
            return phase switch
            {
                TurnPhase.Start => 0.5f,
                TurnPhase.Draw => 0.3f,
                TurnPhase.End => 1.0f,
                _ => 0f
            };
        }
    }

    /// <summary>
    /// Turn phases
    /// </summary>
    public enum TurnPhase
    {
        Idle,   // No active turn
        Start,  // Turn start effects
        Draw,   // Draw phase
        Main,   // Main phase (player actions)
        End     // Turn end effects
    }
}
