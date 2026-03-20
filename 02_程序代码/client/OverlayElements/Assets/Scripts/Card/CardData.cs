// Card Data Structure
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// Card element types
    /// </summary>
    public enum ElementType
    {
        Fire,   // 火
        Water,  // 水
        Wind,   // 风
        Wood    // 木
    }

    /// <summary>
    /// Card rarity levels
    /// </summary>
    public enum CardRarity
    {
        Common = 1,     // 普通
        Rare = 2,       // 稀有
        Epic = 3,       // 史诗
        Legendary = 4,   // 传说
        Mythic = 5      // 神话
    }

    /// <summary>
    /// Card data template
    /// </summary>
    [Serializable]
    public class CardData
    {
        [Header("Basic Info")]
        public string cardId;          // Unique card ID (e.g., "FIRE_001")
        public string cardName;       // Card display name
        public ElementType element;   // Element type
        public CardRarity rarity;     // Rarity level

        [Header("Combat Stats")]
        public int cost;              // Energy cost
        public int attack;             // Attack power
        public int health;             // Health points
        public int maxHealth;          // Max health for this card

        [Header("Visual")]
        public string artworkPath;     // Path to artwork sprite
        public string description;     // Card description text

        [Header("Skills")]
        public List<string> skillIds; // List of skill IDs

        /// <summary>
        /// Initialize card with default values
        /// </summary>
        public void Initialize()
        {
            cardId = "";
            cardName = "";
            element = ElementType.Fire;
            rarity = CardRarity.Common;
            cost = 0;
            attack = 0;
            health = 0;
            maxHealth = 0;
            artworkPath = "";
            description = "";
            skillIds = new List<string>();
        }

        /// <summary>
        /// Get element color for UI display
        /// </summary>
        public Color GetElementColor()
        {
            return element switch
            {
                ElementType.Fire => new Color(1f, 0.3f, 0f),
                ElementType.Water => new Color(0.2f, 0.5f, 1f),
                ElementType.Wind => new Color(0.3f, 0.8f, 0.3f),
                ElementType.Wood => new Color(0.6f, 0.4f, 0.2f),
                _ => Color.white
            };
        }
    }
}
