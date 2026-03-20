// Card Deck Manager
// Version: 0.1.0
// Created: 2026-03-20

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// Card deck for player management
    /// </summary>
    [Serializable]
    public class CardDeck
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;
        [SerializeField] private List<CardData> mainDeck = new List<CardData>();
        [SerializeField] private List<CardInstance> drawPile = new List<CardInstance>();
        [SerializeField] private List<CardInstance> hand = new List<CardInstance>();
        [SerializeField] private List<CardInstance> field = new List<CardInstance>();
        [SerializeField] private List<CardInstance> grave = new List<CardInstance>();
        
        private System.Random random = new System.Random();

        public string DeckId => deckId;
        public string DeckName => deckName;
        public int DeckSize => mainDeck.Count;
        public int DrawPileCount => drawPile.Count;
        public int HandCount => hand.Count;
        public int FieldCount => field.Count;
        public int GraveCount => grave.Count;

        public IReadOnlyList<CardInstance> Hand => hand.AsReadOnly();
        public IReadOnlyList<CardInstance> Field => field.AsReadOnly();
        public IReadOnlyList<CardInstance> Grave => grave.AsReadOnly();

        /// <summary>
        /// Create a new deck
        /// </summary>
        public CardDeck(string name)
        {
            deckId = Guid.NewGuid().ToString();
            deckName = name;
        }

        /// <summary>
        /// Set main deck from card templates
        /// </summary>
        public void SetMainDeck(List<CardData> cards)
        {
            mainDeck = new List<CardData>(cards);
        }

        /// <summary>
        /// Initialize deck for game start
        /// </summary>
        public void Initialize()
        {
            // Clear all zones
            drawPile.Clear();
            hand.Clear();
            field.Clear();
            grave.Clear();

            // Create instances from templates
            foreach (var card in mainDeck)
            {
                drawPile.Add(card.CreateInstance());
            }

            // Shuffle draw pile
            ShuffleDrawPile();

            // Draw initial hand (usually 5 cards)
            DrawCards(5);
        }

        /// <summary>
        /// Shuffle the draw pile
        /// </summary>
        public void ShuffleDrawPile()
        {
            for (int i = drawPile.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (drawPile[i], drawPile[j]) = (drawPile[j], drawPile[i]);
            }
        }

        /// <summary>
        /// Draw cards from draw pile to hand
        /// </summary>
        public List<CardInstance> DrawCards(int count)
        {
            List<CardInstance> drawn = new List<CardInstance>();
            
            for (int i = 0; i < count && drawPile.Count > 0; i++)
            {
                var card = drawPile[drawPile.Count - 1];
                drawPile.RemoveAt(drawPile.Count - 1);
                card.Zone = CardZone.Hand;
                hand.Add(card);
                drawn.Add(card);
            }
            
            return drawn;
        }

        /// <summary>
        /// Play a card from hand to field
        /// </summary>
        public bool PlayCard(CardInstance card)
        {
            if (card == null || !hand.Contains(card)) return false;
            if (field.Count >= 5) return false; // Max 5 cards on field
            
            hand.Remove(card);
            card.Zone = CardZone.Field;
            field.Add(card);
            return true;
        }

        /// <summary>
        /// Move card from field to grave
        /// </summary>
        public void SendToGrave(CardInstance card)
        {
            if (field.Contains(card))
            {
                field.Remove(card);
                card.Zone = CardZone.Grave;
                grave.Add(card);
            }
        }

        /// <summary>
        /// Get cards by element type
        /// </summary>
        public List<CardInstance> GetCardsByElement(ElementType element)
        {
            return hand.Where(c => c.Element == element).ToList();
        }

        /// <summary>
        /// Get cards by rarity
        /// </summary>
        public List<CardInstance> GetCardsByRarity(CardRarity rarity)
        {
            return hand.Where(c => c.Rarity == rarity).ToList();
        }

        /// <summary>
        /// Get lowest cost card in hand
        /// </summary>
        public CardInstance GetLowestCostCard()
        {
            return hand.OrderBy(c => c.Cost).FirstOrDefault();
        }

        /// <summary>
        /// Get highest attack card in hand
        /// </summary>
        public CardInstance GetHighestAttackCard()
        {
            return hand.OrderByDescending(c => c.EffectiveAttack).FirstOrDefault();
        }

        /// <summary>
        /// On turn start - refresh all field cards
        /// </summary>
        public void OnTurnStart()
        {
            foreach (var card in field)
            {
                card.OnTurnStart();
            }
        }

        /// <summary>
        /// Check if deck is empty
        /// </summary>
        public bool IsEmpty => drawPile.Count == 0 && hand.Count == 0;

        /// <summary>
        /// Get total cards in deck
        /// </summary>
        public int TotalCards => drawPile.Count + hand.Count + field.Count + grave.Count;
    }
}
