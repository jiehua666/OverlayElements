// Player - Player data and state
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Linq;
using UnityEngine;
using OverlayElements.Card;

namespace OverlayElements.Game
{
    /// <summary>
    /// Player data and state
    /// </summary>
    [Serializable]
    public class Player
    {
        [SerializeField] private string name;
        [SerializeField] private int health;
        [SerializeField] private int maxHealth;
        [SerializeField] private int energy;
        [SerializeField] private int maxEnergy;
        [SerializeField] private int manaCrystals;
        [SerializeField] private CardDeck deck;
        [SerializeField] private bool isAI;

        public string Name => name;
        public int Health => health;
        public int MaxHealth => maxHealth;
        public int Energy => energy;
        public int MaxEnergy => maxEnergy;
        public int ManaCrystals => manaCrystals;
        public CardDeck Deck => deck;
        public bool IsAI => isAI;
        public bool IsAlive => health > 0;

        // Events
        public event Action<int> OnHealthChanged;
        public event Action<int> OnEnergyChanged;
        public event Action<CardInstance> OnCardDrawn;
        public event Action<CardInstance> OnCardPlayed;

        public Player(string name, int maxHealth, int maxEnergy)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.maxEnergy = maxEnergy;
            this.energy = maxEnergy;
            this.manaCrystals = 0;
        }

        /// <summary>
        /// Set player's deck
        /// </summary>
        public void SetDeck(CardDeck deck)
        {
            this.deck = deck;
        }

        /// <summary>
        /// Take damage
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            health = Mathf.Max(0, health - damage);
            Debug.Log($"[{name}] Takes {damage} damage. Health: {health}/{maxHealth}");
            OnHealthChanged?.Invoke(health);
        }

        /// <summary>
        /// Heal
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0) return;
            int oldHealth = health;
            health = Mathf.Min(maxHealth, health + amount);
            int actualHeal = health - oldHealth;
            if (actualHeal > 0)
            {
                Debug.Log($"[{name}] Heals {actualHeal}. Health: {health}/{maxHealth}");
                OnHealthChanged?.Invoke(health);
            }
        }

        /// <summary>
        /// Spend energy
        /// </summary>
        public bool SpendEnergy(int amount)
        {
            if (energy < amount) return false;
            energy -= amount;
            OnEnergyChanged?.Invoke(energy);
            return true;
        }

        /// <summary>
        /// Gain energy (new turn)
        /// </summary>
        public void RefreshEnergy()
        {
            // Increase max energy each turn (cap at 10)
            maxEnergy = Mathf.Min(10, maxEnergy + 1);
            energy = maxEnergy;
            Debug.Log($"[{name}] Energy refreshed: {energy}/{maxEnergy}");
            OnEnergyChanged?.Invoke(energy);
        }

        /// <summary>
        /// Draw cards
        /// </summary>
        public void DrawCards(int count)
        {
            if (deck == null) return;
            
            var drawn = deck.DrawCards(count);
            foreach (var card in drawn)
            {
                Debug.Log($"[{name}] Drew: {card.CardName}");
                OnCardDrawn?.Invoke(card);
            }
        }

        /// <summary>
        /// Play card from hand
        /// </summary>
        public bool PlayCard(CardInstance card)
        {
            if (card == null || deck.Hand == null || !deck.Hand.Any(c => c == card)) return false;
            if (!SpendEnergy(card.Cost)) return false;
            
            if (deck.PlayCard(card))
            {
                Debug.Log($"[{name}] Played: {card.CardName}");
                OnCardPlayed?.Invoke(card);
                return true;
            }
            return false;
        }

        /// <summary>
        /// On turn start
        /// </summary>
        public void OnTurnStart()
        {
            RefreshEnergy();
            DrawCards(1);
        }

        /// <summary>
        /// On turn end
        /// </summary>
        public void OnTurnEnd()
        {
            // Reset any exhausted cards
            foreach (var card in deck.Field)
            {
                card.OnTurnEnd();
            }
        }
    }
}
