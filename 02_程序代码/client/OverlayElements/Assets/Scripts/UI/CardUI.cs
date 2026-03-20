// Card UI - Visual representation of a card
// Version: 0.1.0
// Created: 2026-03-20

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OverlayElements.Card;

namespace OverlayElements.UI
{
    /// <summary>
    /// Card UI component - displays card data visually
    /// </summary>
    public class CardUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image artworkImage;
        [SerializeField] private Image frameImage;
        [SerializeField] private Image elementIcon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI attackText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject attackGlow;
        [SerializeField] private GameObject exhaustedOverlay;
        [SerializeField] private GameObject overlayIndicator;

        // Card data
        private CardInstance cardInstance;
        private bool isSelected;
        private bool isHovered;

        // Properties
        public CardInstance CardInstance => cardInstance;
        public bool IsSelected => isSelected;

        // Events
        public event System.Action<CardUI> OnCardClicked;
        public event System.Action<CardUI> OnCardHovered;
        public event System.Action<CardUI> OnCardUnhovered;

        private void Start()
        {
            // Add click handler
            GetComponent<Button>()?.onClick.AddListener(OnClick);
        }

        /// <summary>
        /// Set card data to display
        /// </summary>
        public void SetCard(CardInstance card)
        {
            cardInstance = card;
            UpdateDisplay();
        }

        /// <summary>
        /// Update all UI elements
        /// </summary>
        public void UpdateDisplay()
        {
            if (cardInstance == null) return;

            var data = cardInstance.Template;

            // Basic info
            nameText.text = data.cardName;
            costText.text = data.cost.ToString();
            attackText.text = cardInstance.EffectiveAttack.ToString();
            healthText.text = cardInstance.CurrentHealth.ToString();
            descriptionText.text = data.description;

            // Artwork
            if (data.artwork != null)
            {
                artworkImage.sprite = data.artwork;
            }

            // Colors
            frameImage.color = data.GetRarityColor();
            elementIcon.color = data.GetElementColor();

            // State
            UpdateState();
        }

        /// <summary>
        /// Update visual state (attack glow, exhaustion, etc.)
        /// </summary>
        public void UpdateState()
        {
            if (cardInstance == null) return;

            // Can attack
            if (attackGlow != null)
            {
                attackGlow.SetActive(cardInstance.CanAttack);
            }

            // Exhausted
            if (exhaustedOverlay != null)
            {
                exhaustedOverlay.SetActive(cardInstance.State == CardState.Exhausted);
            }

            // Overlay count
            if (overlayIndicator != null)
            {
                overlayIndicator.SetActive(cardInstance.CurrentOverlayCount > 0);
            }
        }

        /// <summary>
        /// Set selection state
        /// </summary>
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            transform.localScale = selected ? Vector3.one * 1.1f : Vector3.one;
        }

        /// <summary>
        /// On card clicked
        /// </summary>
        private void OnClick()
        {
            OnCardClicked?.Invoke(this);
        }

        /// <summary>
        /// On mouse enter
        /// </summary>
        public void OnHover()
        {
            if (isHovered) return;
            isHovered = true;
            transform.localPosition += Vector3.up * 20f;
            OnCardHovered?.Invoke(this);
        }

        /// <summary>
        /// On mouse exit
        /// </summary>
        public void OnUnhover()
        {
            if (!isHovered) return;
            isHovered = false;
            transform.localPosition -= Vector3.up * 20f;
            OnCardUnhovered?.Invoke(this);
        }

        private void OnMouseEnter()
        {
            OnHover();
        }

        private void OnMouseExit()
        {
            OnUnhover();
        }
    }
}
