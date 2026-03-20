// Card Instance (Runtime Card State)
// Version: 0.1.0
// Created: 2026-03-20

using System;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// Runtime card instance with state tracking
    /// </summary>
    [Serializable]
    public class CardInstance
    {
        // Reference to card template
        [SerializeField] private CardData template;
        
        // Runtime state
        [SerializeField] private int currentHealth;
        [SerializeField] private int currentOverlayCount;
        [SerializeField] private CardState state;
        [SerializeField] private CardZone zone;
        [SerializeField] private int instanceId;
        [SerializeField] private bool hasAttackedThisTurn;

        // Static instance counter
        private static int instanceCounter = 0;

        /// <summary>
        /// Create card instance from template
        /// </summary>
        public CardInstance(CardData template)
        {
            this.template = template;
            this.instanceId = ++instanceCounter;
            this.currentHealth = template.health;
            this.currentOverlayCount = 0;
            this.state = CardState.Normal;
            this.zone = CardZone.Deck;
            this.hasAttackedThisTurn = false;
        }

        // Properties
        public CardData Template => template;
        public string CardId => template.cardId;
        public string CardName => template.cardName;
        public ElementType Element => template.element;
        public CardRarity Rarity => template.rarity;
        public CardType CardType => template.cardType;
        public int Cost => template.cost;
        public int BaseAttack => template.attack;
        public int MaxHealth => template.maxHealth;
        public Sprite Artwork => template.artwork;
        public string Description => template.description;
        public int InstanceId => instanceId;

        public int CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        }

        public int CurrentOverlayCount
        {
            get => currentOverlayCount;
            set => currentOverlayCount = Mathf.Clamp(value, 0, template.maxOverlayLayers);
        }

        public CardState State
        {
            get => state;
            set => state = value;
        }

        public CardZone Zone
        {
            get => zone;
            set => zone = value;
        }

        public bool HasAttackedThisTurn
        {
            get => hasAttackedThisTurn;
            set => hasAttackedThisTurn = value;
        }

        /// <summary>
        /// Get effective attack including overlay bonus
        /// </summary>
        public int EffectiveAttack => template.GetEffectiveAttack();

        /// <summary>
        /// Get effective health including overlay bonus
        /// </summary>
        public int EffectiveHealth => template.GetEffectiveHealth();

        /// <summary>
        /// Is this card alive?
        /// </summary>
        public bool IsAlive => currentHealth > 0 && state != CardState.Destroyed;

        /// <summary>
        /// Can this card attack?
        /// </summary>
        public bool CanAttack => IsAlive && state == CardState.Normal && !hasAttackedThisTurn && zone == CardZone.Field;

        /// <summary>
        /// Can this card be overlaid?
        /// </summary>
        public bool CanOverlay => template.canOverlay && currentOverlayCount < template.maxOverlayLayers;

        // === Actions ===

        /// <summary>
        /// Apply damage to this card
        /// </summary>
        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                state = CardState.Destroyed;
                zone = CardZone.Grave;
            }
        }

        /// <summary>
        /// Heal this card
        /// </summary>
        public void Heal(int amount)
        {
            if (amount <= 0) return;
            currentHealth = Mathf.Min(currentHealth + amount, MaxHealth);
        }

        /// <summary>
        /// Overlay another card onto this one
        /// </summary>
        public bool Overlay(CardInstance other)
        {
            if (!CanOverlay || other == null) return false;
            
            // Check element compatibility (can only overlay same element)
            if (Element != other.Element && Element != ElementType.None) return false;
            
            currentOverlayCount++;
            // Apply overlay bonus stats
            currentHealth += other.CurrentHealth;
            return true;
        }

        /// <summary>
        /// Reset for new turn
        /// </summary>
        public void OnTurnStart()
        {
            hasAttackedThisTurn = false;
            if (state == CardState.Exhausted)
                state = CardState.Normal;
        }

        /// <summary>
        /// End of turn effects
        /// </summary>
        public void OnTurnEnd()
        {
            hasAttackedThisTurn = false;
        }

        /// <summary>
        /// Attack another card
        /// </summary>
        public void Attack(CardInstance target)
        {
            if (!CanAttack || target == null) return;
            
            target.TakeDamage(EffectiveAttack);
            TakeDamage(target.EffectiveAttack);
            hasAttackedThisTurn = true;
            state = CardState.Exhausted;
        }

        /// <summary>
        /// Calculate elemental reaction with another card
        /// </summary>
        public ElementReaction GetReactionWith(CardInstance other)
        {
            if (other == null) return ElementReaction.None;

            return (Element, other.Element) switch
            {
                (ElementType.Fire, ElementType.Water) => ElementReaction.Evaporate,
                (ElementType.Water, ElementType.Fire) => ElementReaction.Evaporate,
                (ElementType.Fire, ElementType.Wood) => ElementReaction.Blight,
                (ElementType.Wood, ElementType.Fire) => ElementReaction.Blight,
                (ElementType.Water, ElementType.Wood) => ElementReaction.Mud,
                (ElementType.Wood, ElementType.Water) => ElementReaction.Mud,
                (ElementType.Water, ElementType.Wind) => ElementReaction.Storm,
                (ElementType.Wind, ElementType.Water) => ElementReaction.Storm,
                (ElementType.Fire, ElementType.Wind) => ElementReaction.Wildfire,
                (ElementType.Wind, ElementType.Fire) => ElementReaction.Wildfire,
                (ElementType.Wind, ElementType.Wood) => ElementReaction.Photosynthesis,
                (ElementType.Wood, ElementType.Wind) => ElementReaction.Photosynthesis,
                _ => ElementReaction.None
            };
        }
    }
}
