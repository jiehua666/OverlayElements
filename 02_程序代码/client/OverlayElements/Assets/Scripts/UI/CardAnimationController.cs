// Card Animation Controller
// Version: 0.1.0
// Created: 2026-03-20

using System.Collections;
using UnityEngine;

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

        /// <summary>
        /// Animate card from hand to field
        /// </summary>
        public void AnimateCardPlay(CardUI cardUI, Vector3 targetPosition, System.Action onComplete = null)
        {
            StartCoroutine(AnimateCardPlayCoroutine(cardUI, targetPosition, onComplete));
        }

        private IEnumerator AnimateCardPlayCoroutine(CardUI cardUI, Vector3 targetPosition, System.Action onComplete)
        {
            Vector3 startPos = cardUI.transform.position;
            Vector3 startScale = cardUI.transform.localScale;
            float elapsed = 0f;

            while (elapsed < cardPlayDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / cardPlayDuration);
                float easeT = EaseOutBack(t);

                cardUI.transform.position = Vector3.Lerp(startPos, targetPosition, easeT);
                cardUI.transform.localScale = startScale * (1f + 0.2f * (1f - t));

                yield return null;
            }

            cardUI.transform.position = targetPosition;
            cardUI.transform.localScale = startScale;
            onComplete?.Invoke();
        }

        /// <summary>
        /// Animate card attack
        /// </summary>
        public void AnimateAttack(CardUI attackerUI, CardUI targetUI, System.Action onComplete = null)
        {
            StartCoroutine(AnimateAttackCoroutine(attackerUI, targetUI, onComplete));
        }

        private IEnumerator AnimateAttackCoroutine(CardUI attackerUI, CardUI targetUI, System.Action onComplete)
        {
            Vector3 originalPos = attackerUI.transform.position;
            Vector3 targetPos = targetUI.transform.position;
            float halfDuration = attackDuration / 2f;

            // Move toward target
            float elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / halfDuration);
                attackerUI.transform.position = Vector3.Lerp(originalPos, targetPos, EaseOutQuad(t));
                yield return null;
            }

            // Impact effect
            targetUI.transform.localPosition = Random.insideUnitCircle * 10f;
            yield return new WaitForSeconds(0.05f);
            targetUI.transform.localPosition = Vector3.zero;

            // Move back
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / halfDuration);
                attackerUI.transform.position = Vector3.Lerp(targetPos, originalPos, EaseInQuad(t));
                yield return null;
            }

            attackerUI.transform.position = originalPos;
            onComplete?.Invoke();
        }

        /// <summary>
        /// Animate card hover
        /// </summary>
        public void AnimateHover(CardUI cardUI, bool isHovering)
        {
            StopCoroutine("HoverCoroutine");
            StartCoroutine(HoverCoroutine(cardUI, isHovering));
        }

        private IEnumerator HoverCoroutine(CardUI cardUI, bool isHovering)
        {
            Vector3 startScale = cardUI.transform.localScale;
            Vector3 targetScale = isHovering ? Vector3.one * 1.15f : Vector3.one;
            float elapsed = 0f;

            while (elapsed < hoverDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / hoverDuration);
                cardUI.transform.localScale = Vector3.Lerp(startScale, targetScale, EaseOutQuad(t));
                yield return null;
            }

            cardUI.transform.localScale = targetScale;
        }

        /// <summary>
        /// Animate card destruction
        /// </summary>
        public void AnimateDestroy(CardUI cardUI, System.Action onComplete = null)
        {
            StartCoroutine(AnimateDestroyCoroutine(cardUI, onComplete));
        }

        private IEnumerator AnimateDestroyCoroutine(CardUI cardUI, System.Action onComplete)
        {
            Vector3 startScale = cardUI.transform.localScale;
            float elapsed = 0f;

            // Scale up phase
            while (elapsed < destroyDuration / 3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (destroyDuration / 3f));
                cardUI.transform.localScale = startScale * (1f + 0.3f * t);
                yield return null;
            }

            // Scale down phase
            elapsed = 0f;
            float startScaleVal = startScale.x * 1.3f;
            while (elapsed < destroyDuration * 2f / 3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / (destroyDuration * 2f / 3f));
                float scale = Mathf.Lerp(startScaleVal, 0f, EaseInBack(t));
                cardUI.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            cardUI.gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        /// <summary>
        /// Animate overlay effect
        /// </summary>
        public void AnimateOverlay(CardUI baseCardUI, CardUI overlayCardUI, System.Action onComplete = null)
        {
            StartCoroutine(AnimateOverlayCoroutine(baseCardUI, overlayCardUI, onComplete));
        }

        private IEnumerator AnimateOverlayCoroutine(CardUI baseCardUI, CardUI overlayCardUI, System.Action onComplete)
        {
            Vector3 startPos = overlayCardUI.transform.position;
            Vector3 startScale = overlayCardUI.transform.localScale;
            Vector3 targetPos = baseCardUI.transform.position;
            float elapsed = 0f;

            while (elapsed < 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / 0.3f);
                overlayCardUI.transform.position = Vector3.Lerp(startPos, targetPos, EaseInQuad(t));
                overlayCardUI.transform.localScale = Vector3.Lerp(startScale, Vector3.one * 0.5f, t);
                yield return null;
            }

            // Punch effect on base card
            baseCardUI.transform.localScale = Vector3.one * 1.3f;
            yield return new WaitForSeconds(0.1f);

            elapsed = 0f;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / 0.2f);
                baseCardUI.transform.localScale = Vector3.Lerp(Vector3.one * 1.3f, Vector3.one, EaseOutQuad(t));
                yield return null;
            }

            overlayCardUI.gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        /// <summary>
        /// Show damage number
        /// </summary>
        public void ShowDamageNumber(Vector3 position, int damage, bool isHeal = false)
        {
            StartCoroutine(ShowDamageNumberCoroutine(position, damage, isHeal));
        }

        private IEnumerator ShowDamageNumberCoroutine(Vector3 position, int damage, bool isHeal)
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

            float elapsed = 0f;
            float duration = 0.8f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                damageText.transform.position = position + Vector3.up * (50f * t);
                textMesh.alpha = 1f - t;

                yield return null;
            }

            Destroy(damageText);
        }

        /// <summary>
        /// Animate turn indicator
        /// </summary>
        public void AnimateTurnIndicator(GameObject indicator, string message, System.Action onComplete = null)
        {
            StartCoroutine(AnimateTurnIndicatorCoroutine(indicator, message, onComplete));
        }

        private IEnumerator AnimateTurnIndicatorCoroutine(GameObject indicator, string message, System.Action onComplete)
        {
            var text = indicator.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (text != null)
            {
                text.text = message;
            }

            indicator.SetActive(true);
            Vector3 startScale = indicator.transform.localScale;

            // Scale up
            float elapsed = 0f;
            while (elapsed < 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / 0.3f);
                indicator.transform.localScale = startScale * (0.8f + 0.4f * EaseOutQuad(t));
                yield return null;
            }

            // Scale down to normal
            elapsed = 0f;
            while (elapsed < 0.2f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / 0.2f);
                indicator.transform.localScale = Vector3.one * EaseInQuad(t);
                yield return null;
            }

            // Hold
            yield return new WaitForSeconds(1.5f);

            // Fade out
            var canvasGroup = indicator.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = indicator.AddComponent<CanvasGroup>();
            }

            elapsed = 0f;
            while (elapsed < 0.3f)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / 0.3f);
                canvasGroup.alpha = 1f - t;
                yield return null;
            }

            indicator.SetActive(false);
            onComplete?.Invoke();
        }

        /// <summary>
        /// Animate element reaction effect
        /// </summary>
        public void ShowElementReaction(OverlayElements.Card.ElementType element, Vector3 position)
        {
            StartCoroutine(ShowElementReactionCoroutine(element, position));
        }

        private IEnumerator ShowElementReactionCoroutine(OverlayElements.Card.ElementType element, Vector3 position)
        {
            GameObject effect = new GameObject("ElementReaction");
            effect.transform.position = position;

            var sprite = effect.AddComponent<SpriteRenderer>();
            sprite.sprite = CreateElementSprite();
            sprite.color = GetElementColor(element);

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                effect.transform.localScale = Vector3.one * (1f + t);
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f - t);

                yield return null;
            }

            Destroy(effect);
        }

        private Color GetElementColor(OverlayElements.Card.ElementType element)
        {
            return element switch
            {
                OverlayElements.Card.ElementType.Fire => new Color(1f, 0.3f, 0f),
                OverlayElements.Card.ElementType.Water => new Color(0.2f, 0.5f, 1f),
                OverlayElements.Card.ElementType.Wind => new Color(0.3f, 0.8f, 0.3f),
                OverlayElements.Card.ElementType.Wood => new Color(0.6f, 0.4f, 0.2f),
                _ => Color.white
            };
        }

        private Sprite CreateElementSprite()
        {
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

        // Easing functions
        private float EaseOutQuad(float t) => 1f - (1f - t) * (1f - t);
        private float EaseInQuad(float t) => t * t;
        private float EaseOutBack(float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }
        private float EaseInBack(float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            return c3 * t * t * t - c1 * t * t;
        }
    }
}
