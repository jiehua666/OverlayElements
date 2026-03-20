// Combat Manager - Handles combat logic
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;
using OverlayElements.Card;

namespace OverlayElements.Game
{
    /// <summary>
    /// Combat manager - handles all combat interactions
    /// </summary>
    public class CombatManager
    {
        private GameBoard board;
        private TurnManager turnManager;

        public event Action<CardInstance, CardInstance, int> OnDamageDealt;
        public event Action<CardInstance, int> OnCardDestroyed;
        public event Action<CardInstance, CardInstance> OnReactionTriggered;

        public CombatManager(GameBoard board, TurnManager turnManager)
        {
            this.board = board;
            this.turnManager = turnManager;
        }

        /// <summary>
        /// Execute attack between two cards
        /// </summary>
        public bool ExecuteAttack(CardInstance attacker, CardInstance defender)
        {
            if (attacker == null || defender == null) return false;
            if (!attacker.CanAttack) return false;

            // Calculate damage
            int attackerDamage = attacker.EffectiveAttack;
            int defenderDamage = defender.EffectiveAttack;

            // Apply elemental reaction
            var reaction = attacker.GetReactionWith(defender);
            if (reaction != ElementReaction.None)
            {
                ApplyReaction(attacker, defender, reaction);
            }

            // Apply damage
            defender.TakeDamage(attackerDamage);
            attacker.TakeDamage(defenderDamage);

            // Events
            OnDamageDealt?.Invoke(attacker, defender, attackerDamage);

            // Check for destruction
            CheckAndProcessDestruction(attacker);
            CheckAndProcessDestruction(defender);

            return true;
        }

        /// <summary>
        /// Apply elemental reaction effects
        /// </summary>
        private void ApplyReaction(CardInstance attacker, CardInstance defender, ElementReaction reaction)
        {
            int bonusDamage = 0;
            int bonusHeal = 0;

            switch (reaction)
            {
                case ElementReaction.Evaporate:
                    bonusDamage = 15;
                    break;
                case ElementReaction.Blight:
                    bonusDamage = 8;
                    defender.TakeDamage(8); // Extra DOT
                    break;
                case ElementReaction.Storm:
                    bonusDamage = 20;
                    break;
                case ElementReaction.Wildfire:
                    bonusDamage = 12;
                    break;
                case ElementReaction.Photosynthesis:
                    bonusHeal = 10;
                    attacker.Heal(10);
                    break;
            }

            if (bonusDamage > 0)
            {
                defender.TakeDamage(bonusDamage);
                Debug.Log($"[Combat] {reaction} reaction! Bonus damage: {bonusDamage}");
            }

            OnReactionTriggered?.Invoke(attacker, defender);
        }

        /// <summary>
        /// Check and process card destruction
        /// </summary>
        private void CheckAndProcessDestruction(CardInstance card)
        {
            if (!card.IsAlive)
            {
                board.RemoveCard(card);
                OnCardDestroyed?.Invoke(card, 0);
                Debug.Log($"[Combat] {card.CardName} was destroyed");
            }
        }

        /// <summary>
        /// Execute overlay action
        /// </summary>
        public bool ExecuteOverlay(CardInstance baseCard, CardInstance overlayCard, bool isPlayer)
        {
            if (baseCard == null || overlayCard == null) return false;
            if (!baseCard.CanOverlay) return false;
            if (!CanOverlay(isPlayer)) return false;

            // Element check
            if (baseCard.Element != overlayCard.Element && 
                baseCard.Element != ElementType.None && 
                overlayCard.Element != ElementType.None)
            {
                Debug.Log("[Combat] Cannot overlay different elements");
                return false;
            }

            // Execute overlay
            if (board.Overlay(baseCard, overlayCard, isPlayer))
            {
                // Apply stats
                baseCard.CurrentHealth += overlayCard.CurrentHealth;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Can player perform overlay?
        /// </summary>
        public bool CanOverlay(bool isPlayer)
        {
            var field = isPlayer ? board.PlayerField : board.OpponentField;
            foreach (var card in field)
            {
                if (card.CanOverlay) return true;
            }
            return false;
        }

        /// <summary>
        /// Get valid attack targets for attacker
        /// </summary>
        public List<CardInstance> GetValidTargets(CardInstance attacker)
        {
            var targets = new List<CardInstance>();
            bool isPlayer = board.PlayerField.Contains(attacker);

            var enemyField = isPlayer ? board.OpponentField : board.PlayerField;
            
            foreach (var target in enemyField)
            {
                if (target.IsAlive)
                {
                    targets.Add(target);
                }
            }

            return targets;
        }

        /// <summary>
        /// Calculate board advantage
        /// </summary>
        public int GetBoardAdvantage()
        {
            int playerValue = 0;
            int opponentValue = 0;

            foreach (var card in board.PlayerField)
            {
                playerValue += card.EffectiveAttack + card.CurrentHealth;
            }

            foreach (var card in board.OpponentField)
            {
                opponentValue += card.EffectiveAttack + card.CurrentHealth;
            }

            return playerValue - opponentValue;
        }
    }
}
