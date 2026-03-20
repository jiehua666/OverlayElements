// Card Data Template
// Version: 0.2.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// Card data template (卡牌数据模板)
    /// This is a ScriptableObject that defines card properties
    /// </summary>
    [CreateAssetMenu(fileName = "NewCard", menuName = "OverlayElements/Card")]
    public class CardData : ScriptableObject
    {
        [Header("=== Basic Info ===")]
        [Tooltip("Unique card ID, e.g. FIRE_001")]
        public string cardId;
        
        [Tooltip("Card display name")]
        public string cardName;
        
        [Tooltip("Element type")]
        public ElementType element;
        
        [Tooltip("Card rarity")]
        public CardRarity rarity;
        
        [Tooltip("Card type/category")]
        public CardType cardType;

        [Header("=== Combat Stats ===")]
        [Tooltip("Energy cost to play")]
        [Range(0, 10)]
        public int cost;
        
        [Tooltip("Attack power")]
        [Range(0, 20)]
        public int attack;
        
        [Tooltip("Current health")]
        [Range(1, 100)]
        public int health;
        
        [Tooltip("Maximum health")]
        [Range(1, 100)]
        public int maxHealth;

        [Header("=== Overlay Mechanics ===")]
        [Tooltip("Can this card be overlaid?")]
        public bool canOverlay = true;
        
        [Tooltip("Overlay power multiplier (1.0 = normal)")]
        [Range(0.5f, 3f)]
        public float overlayMultiplier = 1.0f;
        
        [Tooltip("Maximum overlay layers allowed")]
        [Range(1, 5)]
        public int maxOverlayLayers = 3;
        
        [Tooltip("Current overlay count")]
        [HideInInspector]
        public int currentOverlayCount = 0;

        [Header("=== Visual ===")]
        [Tooltip("Card artwork sprite")]
        public Sprite artwork;
        
        [Tooltip("Card description")]
        [TextArea(2, 4)]
        public string description;
        
        [Tooltip("Card frame color tint")]
        public Color frameTint = Color.white;

        [Header("=== Skills ===")]
        [Tooltip("Passive skill ID")]
        public string passiveSkillId;
        
        [Tooltip("Active skill IDs (comma separated)")]
        public List<string> activeSkillIds = new List<string>();

        [Header("=== Audio ===")]
        public AudioClip playSound;
        public AudioClip attackSound;
        public AudioClip deathSound;

        /// <summary>
        /// Get element color for UI
        /// </summary>
        public Color GetElementColor()
        {
            return element switch
            {
                ElementType.Fire => new Color(0.9f, 0.25f, 0.05f),
                ElementType.Water => new Color(0.15f, 0.45f, 0.95f),
                ElementType.Wind => new Color(0.25f, 0.75f, 0.35f),
                ElementType.Wood => new Color(0.55f, 0.35f, 0.15f),
                _ => Color.gray
            };
        }

        /// <summary>
        /// Get rarity color
        /// </summary>
        public Color GetRarityColor()
        {
            return rarity switch
            {
                CardRarity.Common => new Color(0.7f, 0.7f, 0.7f),
                CardRarity.Rare => new Color(0.3f, 0.5f, 1f),
                CardRarity.Epic => new Color(0.7f, 0.3f, 1f),
                CardRarity.Legendary => new Color(1f, 0.6f, 0.1f),
                CardRarity.Mythic => new Color(1f, 0.2f, 0.2f),
                _ => Color.white
            };
        }

        /// <summary>
        /// Get effective attack with overlay bonus
        /// </summary>
        public int GetEffectiveAttack()
        {
            return Mathf.RoundToInt(attack * (1 + (currentOverlayCount * overlayMultiplier * 0.25f)));
        }

        /// <summary>
        /// Get effective health with overlay bonus
        /// </summary>
        public int GetEffectiveHealth()
        {
            return maxHealth + (currentOverlayCount * 5);
        }

        /// <summary>
        /// Create a runtime card instance from this template
        /// </summary>
        public CardInstance CreateInstance()
        {
            return new CardInstance(this);
        }
    }
}
