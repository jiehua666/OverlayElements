// Card Animation Controller
// Version: 0.1.0
// Created: 2026-03-20

using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace OverlayElements.UI
{
    /// <summary>
    /// Handles card animations and visual effects
    /// </summary>
    public class CardAnimationController : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float cardPlayDuration = 0.3f;
        [SerializeField] private float attackDuration = 0.4f;
        [SerializeField] private float hoverDuration = 0.2f;
        [SerializeField] private float destroyDuration = 0.5f;

        [Header("Easing")]
        [SerializeField] private Ease playEase = Ease.OutBack;
        [SerializeField] private Ease attackEase = Ease.InOutQuad;
        [SerializeField] private Ease destroyEase = Ease.InBack;

        /// <summary>
        /// Animate card from hand to field
        /// </summary>
        public void AnimateCardPlay(CardUI cardUI, Vector3 targetPosition, System.Action onComplete = null)
        {
            var sequence = DOTween.Sequence();
            
            sequence.Append(cardUI.transform.DOMove(targetPosition, cardPlayDuration)
                .SetEase(playEase));
            
            sequence.Append(cardUI.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f));
            
            sequence.OnComplete(() => onComplete?.Invoke());
        }

        /// <summary>
        /// Animate card attack
        /// </summary>
        public void AnimateAttack(CardUI attackerUI, CardUI targetUI, System.Action onComplete = null)
        {
            Vector3 originalPos = attackerUI.transform.position;
            Vector3 targetPos = targetUI.transform.position;
            
            var sequence = DOTween.Sequence();
            
            // Move toward target
            sequence.Append(attackerUI.transform.DOMove(targetPos, attackDuration / 2)
                .SetEase(Ease.OutQuad));
            
            // Move back
            sequence.Append(attackerUI.transform.DOMove(originalPos, attackDuration / 2)
                .SetEase(Ease.InQuad));
            
            // Target shake effect
            sequence.Insert(attackDuration / 2, targetUI.transform.DOPunchPosition(
                Random.insideUnitCircle * 10f, 0.2f));
            
            // Scale effect on attacker
            sequence.Insert(0, attackerUI.transform.DOPunchScale(Vector3.one * 0.1f, attackDuration));
            
            sequence.OnComplete(() => onComplete?.Invoke());
        }

        /// <summary>
        /// Animate card hover
        /// </summary>
        public void AnimateHover(CardUI cardUI, bool isHovering)
        {
            Vector3 targetScale = isHovering ? Vector3.one * 1.15f : Vector3.one;
            Vector3 targetPos = isHovering ? 
                cardUI.transform.position + Vector3.up * 30f : 
                cardUI.transform.position - Vector3.up * 30f;

            cardUI.transform.DOScale(targetScale, hoverDuration)
                .SetEase(Ease.OutQuad);
            
            cardUI.transform.DOMove(targetPos, hoverDuration)
                .SetEase(Ease.OutQuad);
        }

        /// <summary>
        /// Animate card destruction
        /// </summary>
        public void AnimateDestroy(CardUI cardUI, System.Action onComplete = null)
        {
            var sequence = DOTween.Sequence();
            
            sequence.Append(cardUI.transform.DOScale(Vector3.one * 1.3f, destroyDuration / 3)
                .SetEase(Ease.OutQuad));
            
            sequence.Append(cardUI.transform.DOScale(Vector3.zero, destroyDuration * 2 / 3)
                .SetEase(destroyEase));
            
            sequence.Join(cardUI.GetComponent<CanvasGroup>()?
                .DOFade(0, destroyDuration));
            
            sequence.OnComplete(() => onComplete?.Invoke());
        }

        /// <summary>
        /// Animate overlay effect
        /// </summary>
        public void AnimateOverlay(CardUI baseCardUI, CardUI overlayCardUI, System.Action onComplete = null)
        {
            Vector3 basePos = baseCardUI.transform.position;
            
            // Overlay card flies to base card
            var sequence = DOTween.Sequence();
            
            sequence.Append(overlayCardUI.transform.DOMove(basePos, 0.3f)
                .SetEase(Ease.InQuad));
            
            sequence.Join(overlayCardUI.transform.DOScale(Vector3.one * 0.5f, 0.3f));
            
            sequence.Append(baseCardUI.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f));
            
            sequence.OnComplete(() =>
            {
                overlayCardUI.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }

        /// <summary>
        /// Animate damage number
        /// </summary>
        public void ShowDamageNumber(Vector3 position, int damage, bool isHeal = false)
        {
            GameObject damageText = new GameObject("DamageNumber");
            damageText.transform.position = position;
            
            var textMesh = damageText.AddComponent<TMPro.TextMeshProUGUI>();
            textMesh.text = isHeal ? $"+{damage}" : $"-{damage}";
            textMesh.fontSize = 36;
            textMesh.color = isHeal ? Color.green : Color.red;
            textMesh.alignment = TMPro.TextAlignmentOptions.Center;
            
            var canvas = damageText.AddComponent<Canvas>();
            canvas.sortingOrder = 100;
            
            var sequence = DOTween.Sequence();
            sequence.Append(damageText.transform.DOMoveY(position.y + 50f, 0.8f)
                .SetEase(Ease.OutQuad));
            sequence.Join(textMesh.DOFade(0, 0.8f));
            sequence.OnComplete(() => Destroy(damageText));
        }

        /// <summary>
        /// Animate turn indicator
        /// </summary>
        public void AnimateTurnIndicator(GameObject indicator, string message, System.Action onComplete = null)
        {
            var text = indicator.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (text != null)
            {
                text.text = message;
            }

            indicator.SetActive(true);
            
            var sequence = DOTween.Sequence();
            
            sequence.Append(indicator.transform.DOScale(Vector3.one * 1.2f, 0.3f)
                .SetEase(Ease.OutQuad));
            
            sequence.Append(indicator.transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.InQuad));
            
            sequence.AppendInterval(1.5f);
            
            sequence.Append(indicator.GetComponent<CanvasGroup>()?
                .DOFade(0, 0.3f) ?? indicator.transform.DOScale(Vector3.zero, 0.3f));
            
            sequence.OnComplete(() =>
            {
                indicator.SetActive(false);
                onComplete?.Invoke();
            });
        }

        /// <summary>
        /// Animate element reaction effect
        /// </summary>
        public void ShowElementReaction(ElementType element, Vector3 position)
        {
            GameObject effect = new GameObject("ElementReaction");
            effect.transform.position = position;
            
            // Add particle-like effect based on element
            var sprite = effect.AddComponent<SpriteRenderer>();
            sprite.sprite = CreateElementSprite(element);
            sprite.color = GetElementColor(element);
            
            var sequence = DOTween.Sequence();
            sequence.Append(effect.transform.DOScale(Vector3.one * 2f, 0.5f)
                .SetEase(Ease.OutQuad));
            sequence.Join(effect.GetComponent<SpriteRenderer>().DOFade(0, 0.5f));
            sequence.OnComplete(() => Destroy(effect));
        }

        private Color GetElementColor(ElementType element)
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

        private Sprite CreateElementSprite(ElementType element)
        {
            // Create a simple circle sprite for now
            Texture2D tex = new Texture2D(64, 64);
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(32, 32));
                    tex.SetPixel(x, y, dist < 30 ? Color.white : Color.clear);
                }
            }
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f), 64);
        }
    }
}
