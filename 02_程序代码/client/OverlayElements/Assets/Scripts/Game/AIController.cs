// AI Controller - Simple AI for single player mode
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OverlayElements.Card;

namespace OverlayElements.Game
{
    /// <summary>
    /// AI Controller - handles opponent AI logic
    /// </summary>
    public class AIController
    {
        private Player aiPlayer;
        private Player opponent;
        private CardDeck deck;
        private CombatManager combatManager;
        private GameBoard board;
        private float thinkDelay = 1.5f;
        private bool turnComplete = false;
        private bool isRunning = false;

        public AIController(CombatManager combatManager, GameBoard board)
        {
            this.combatManager = combatManager;
            this.board = board;
        }

        /// <summary>
        /// Initialize with players
        /// </summary>
        public void Initialize(Player aiPlayer, Player opponent)
        {
            this.aiPlayer = aiPlayer;
            this.opponent = opponent;
            this.deck = aiPlayer.Deck;
        }

        /// <summary>
        /// Is AI turn complete?
        /// </summary>
        public bool IsTurnComplete()
        {
            return turnComplete;
        }

        /// <summary>
        /// Execute AI turn (called each frame)
        /// Returns true if still processing
        /// </summary>
        public bool TakeTurn(Player ai, Player opponent)
        {
            if (isRunning) return true;
            
            // This method is called each frame to check if we should continue
            // The actual logic runs via StartAITurn coroutine
            return false;
        }

        /// <summary>
        /// Start AI turn (returns coroutine)
        /// </summary>
        public IEnumerator StartAITurn()
        {
            isRunning = true;
            turnComplete = false;

            Debug.Log("[AI] Starting turn");

            // Wait before acting
            yield return new WaitForSeconds(thinkDelay);

            // Main phase - AI decisions
            while (ShouldContinuePlaying() && aiPlayer.Energy > 0)
            {
                if (!TryPlayBestCard())
                {
                    break;
                }
                yield return new WaitForSeconds(thinkDelay);
            }

            // Attack phase
            yield return ExecuteAttacks();

            // End turn
            turnComplete = true;
            isRunning = false;
            Debug.Log("[AI] Ending turn");
        }

        /// <summary>
        /// Should AI continue playing?
        /// </summary>
        private bool ShouldContinuePlaying()
        {
            if (aiPlayer == null || deck == null) return false;
            
            // Check if AI has playable cards
            foreach (var card in deck.Hand)
            {
                if (aiPlayer.Energy >= card.Cost)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Try to play the best available card
        /// </summary>
        private bool TryPlayBestCard()
        {
            if (aiPlayer == null || deck == null) return false;
            
            CardInstance bestCard = null;
            int bestScore = -1;

            // Find best card to play
            foreach (var card in deck.Hand)
            {
                if (aiPlayer.Energy < card.Cost) continue;

                int score = EvaluateCardPlay(card);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestCard = card;
                }
            }

            if (bestCard != null && bestScore > 0)
            {
                // Place on board
                if (board != null && board.CanPlaceCard(false))
                {
                    board.PlaceCard(bestCard, false);
                    deck.PlayCard(bestCard);
                    Debug.Log($"[AI] Played: {bestCard.CardName}");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Evaluate card play value
        /// </summary>
        private int EvaluateCardPlay(CardInstance card)
        {
            int score = 0;

            // Attack value
            score += card.BaseAttack * 2;

            // Health value
            score += card.MaxHealth;

            // Efficiency
            if (card.BaseAttack >= card.Cost * 2)
            {
                score += 5; // Good efficiency
            }

            // Element synergy (prefer same element)
            if (deck != null)
            {
                int sameElement = 0;
                foreach (var fieldCard in deck.Field)
                {
                    if (fieldCard.Element == card.Element)
                    {
                        sameElement++;
                    }
                }
                score += sameElement * 3;
            }

            return score;
        }

        /// <summary>
        /// Execute AI attacks
        /// </summary>
        private IEnumerator ExecuteAttacks()
        {
            if (deck == null || combatManager == null) yield break;

            yield return new WaitForSeconds(0.5f);

            foreach (var attacker in deck.Field)
            {
                if (!attacker.CanAttack) continue;

                var targets = combatManager.GetValidTargets(attacker);
                if (targets.Count == 0) continue;

                // Find best target
                CardInstance bestTarget = null;
                int bestValue = -1;

                foreach (var target in targets)
                {
                    int value = target.EffectiveAttack + target.CurrentHealth;
                    // Prefer targets we can kill
                    if (target.CurrentHealth <= attacker.EffectiveAttack)
                    {
                        value += 100;
                    }
                    if (value > bestValue)
                    {
                        bestValue = value;
                        bestTarget = target;
                    }
                }

                if (bestTarget != null)
                {
                    combatManager.ExecuteAttack(attacker, bestTarget);
                    Debug.Log($"[AI] Attacked with {attacker.CardName}");
                    yield return new WaitForSeconds(thinkDelay);
                }
            }
        }

        /// <summary>
        /// Set think delay (for difficulty adjustment)
        /// </summary>
        public void SetThinkDelay(float delay)
        {
            thinkDelay = Mathf.Max(0.5f, delay);
        }
    }
}
