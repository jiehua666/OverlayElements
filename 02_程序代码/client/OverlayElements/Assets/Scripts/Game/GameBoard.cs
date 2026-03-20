// Game Board - Board state representation
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;
using OverlayElements.Card;

namespace OverlayElements.Game
{
    /// <summary>
    /// Game board - manages battlefield zones
    /// </summary>
    [Serializable]
    public class GameBoard
    {
        [SerializeField] private List<CardInstance> playerField = new List<CardInstance>();
        [SerializeField] private List<CardInstance> opponentField = new List<CardInstance>();
        [SerializeField] private int maxCardsPerSide = 5;

        public IReadOnlyList<CardInstance> PlayerField => playerField.AsReadOnly();
        public IReadOnlyList<CardInstance> OpponentField => opponentField.AsReadOnly();
        public int MaxCardsPerSide => maxCardsPerSide;

        // Events
        public event Action<CardInstance> OnCardPlaced;
        public event Action<CardInstance> OnCardRemoved;
        public event Action<CardInstance, CardInstance> OnCardAttacked;
        public event Action<CardInstance, CardInstance> OnOverlay;

        /// <summary>
        /// Can place card on player's side?
        /// </summary>
        public bool CanPlaceCard(bool isPlayer)
        {
            var field = isPlayer ? playerField : opponentField;
            return field.Count < maxCardsPerSide;
        }

        /// <summary>
        /// Place card on board
        /// </summary>
        public bool PlaceCard(CardInstance card, bool isPlayer)
        {
            var field = isPlayer ? playerField : opponentField;
            
            if (!CanPlaceCard(isPlayer)) return false;
            if (card == null) return false;

            card.Zone = CardZone.Field;
            field.Add(card);
            OnCardPlaced?.Invoke(card);
            Debug.Log($"[Board] {card.CardName} placed on {(isPlayer ? "player" : "opponent")} field");
            return true;
        }

        /// <summary>
        /// Remove card from board
        /// </summary>
        public void RemoveCard(CardInstance card)
        {
            if (playerField.Any(c => c == card))
            {
                playerField.Remove(card);
                card.Zone = CardZone.Grave;
                OnCardRemoved?.Invoke(card);
            }
            else if (opponentField.Any(c => c == card))
            {
                opponentField.Remove(card);
                card.Zone = CardZone.Grave;
                OnCardRemoved?.Invoke(card);
            }
        }

        /// <summary>
        /// Attack card
        /// </summary>
        public bool Attack(CardInstance attacker, CardInstance defender)
        {
            if (attacker == null || defender == null) return false;
            if (!attacker.CanAttack) return false;

            Debug.Log($"[Board] {attacker.CardName} attacks {defender.CardName}");
            
            attacker.Attack(defender);
            OnCardAttacked?.Invoke(attacker, defender);

            // Check for destruction
            CheckDestruction(attacker);
            CheckDestruction(defender);

            return true;
        }

        /// <summary>
        /// Overlay cards
        /// </summary>
        public bool Overlay(CardInstance baseCard, CardInstance overlayCard, bool isPlayer)
        {
            if (baseCard == null || overlayCard == null) return false;
            if (!baseCard.CanOverlay) return false;

            if (baseCard.Overlay(overlayCard))
            {
                // Remove overlay card from its original location
                RemoveCard(overlayCard);
                OnOverlay?.Invoke(baseCard, overlayCard);
                Debug.Log($"[Board] {baseCard.CardName} overlaid with {overlayCard.CardName}");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if card should be destroyed
        /// </summary>
        private void CheckDestruction(CardInstance card)
        {
            if (!card.IsAlive)
            {
                RemoveCard(card);
                Debug.Log($"[Board] {card.CardName} was destroyed");
            }
        }

        /// <summary>
        /// Get all cards of element type on field
        /// </summary>
        public List<CardInstance> GetCardsByElement(ElementType element, bool isPlayer)
        {
            var field = isPlayer ? playerField : opponentField;
            return field.FindAll(c => c.Element == element);
        }

        /// <summary>
        /// Calculate total attack on field
        /// </summary>
        public int GetTotalAttack(bool isPlayer)
        {
            var field = isPlayer ? playerField : opponentField;
            int total = 0;
            foreach (var card in field)
            {
                total += card.EffectiveAttack;
            }
            return total;
        }

        /// <summary>
        /// Clear board (for new game)
        /// </summary>
        public void Clear()
        {
            playerField.Clear();
            opponentField.Clear();
        }
    }
}
