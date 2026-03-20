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
        private CardDeck deck;
        private CombatManager combatManager;
        private float thinkDelay = 1.5f;

        public AIController(Player aiPlayer, CombatManager combatManager)
        {
            this.aiPlayer = aiPlayer;
            this.deck = aiPlayer.Deck;
            this.combatManager = combatManager;
        }

        /// <summary>
        /// Execute AI turn
        /// </summary>
        public IEnumerator ExecuteTurn(Action onComplete)
        {
            Debug.Log("[AI] Starting turn");
            
            // Wait a bit before acting
            yield return new WaitForSeconds(thinkDelay);

            // Draw phase (handled by Player class)
            yield return new WaitForSeconds(0.5f);

            // Main phase - AI decisions
            while (ShouldContinuePlaying())
            {
                // Try to play a card
                if (!TryPlayBestCard())
                {
                    break;
                }
                yield return new WaitForSeconds(thinkDelay);
            }

            // Attack phase
            yield return ExecuteAttacks();

            // End turn
            Debug.Log("[AI] Ending turn");
            onComplete?.Invoke();
        }

        /// <summary>
        /// Should AI continue playing?
        /// </summary>
        private bool ShouldContinuePlaying()
        {
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
                aiPlayer.PlayCard(bestCard);
                Debug.Log($"[AI] Played: {bestCard.CardName}");
                return true;
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
            int sameElement = 0;
            foreach (var fieldCard in deck.Field)
            {
                if (fieldCard.Element == card.Element)
                {
                    sameElement++;
                }
            }
            score += sameElement * 3;

            return score;
        }

        /// <summary>
        /// Execute AI attacks
        /// </summary>
        private IEnumerator ExecuteAttacks()
        {
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
