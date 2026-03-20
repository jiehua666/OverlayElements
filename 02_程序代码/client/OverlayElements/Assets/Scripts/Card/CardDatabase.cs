// Card Database
// Version: 0.1.0
// Created: 2026-03-20

using System.Collections.Generic;
using UnityEngine;

namespace OverlayElements.Card
{
    /// <summary>
    /// ScriptableObject database for all card definitions
    /// </summary>
    [CreateAssetMenu(fileName = "CardDatabase", menuName = "OverlayElements/Card Database")]
    public class CardDatabase : ScriptableObject
    {
        [SerializeField] private List<CardData> cards = new List<CardData>();
        
        private Dictionary<string, CardData> cardDict;

        /// <summary>
        /// Get all cards
        /// </summary>
        public IReadOnlyList<CardData> Cards => cards.AsReadOnly();

        /// <summary>
        /// Initialize dictionary for fast lookup
        /// </summary>
        public void Initialize()
        {
            cardDict = new Dictionary<string, CardData>();
            foreach (var card in cards)
            {
                if (!string.IsNullOrEmpty(card.cardId))
                {
                    cardDict[card.cardId] = card;
                }
            }
        }

        /// <summary>
        /// Get card by ID
        /// </summary>
        public CardData GetCard(string cardId)
        {
            if (cardDict == null) Initialize();
            return cardDict.TryGetValue(cardId, out var card) ? card : null;
        }

        /// <summary>
        /// Get cards by element
        /// </summary>
        public List<CardData> GetCardsByElement(ElementType element)
        {
            return cards.FindAll(c => c.element == element);
        }

        /// <summary>
        /// Get cards by rarity
        /// </summary>
        public List<CardData> GetCardsByRarity(CardRarity rarity)
        {
            return cards.FindAll(c => c.rarity == rarity);
        }

        /// <summary>
        /// Get starter deck (predefined)
        /// </summary>
        public List<CardData> GetStarterDeck()
        {
            List<CardData> deck = new List<CardData>();
            
            // Add 2 copies of each starter card
            foreach (var card in cards)
            {
                if (card.rarity <= CardRarity.Rare)
                {
                    deck.Add(card);
                    deck.Add(card);
                }
            }
            
            return deck;
        }

        /// <summary>
        /// Create a random deck
        /// </summary>
        public List<CardData> CreateRandomDeck(int size = 20)
        {
            List<CardData> deck = new List<CardData>();
            List<CardData> available = new List<CardData>(cards);
            
            for (int i = 0; i < size && available.Count > 0; i++)
            {
                int index = Random.Range(0, available.Count);
                deck.Add(available[index]);
                available.RemoveAt(index);
            }
            
            return deck;
        }
    }
}
