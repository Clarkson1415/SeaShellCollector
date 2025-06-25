using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class to show text +1 or -10 in red when buying something then to fade out.
    /// </summary>
    public class TextWithFeedback : MonoBehaviour
    {
        private TMP_Text tmpText;
        public float fadeOutToInvisibleTime = 0.5f;

        /// <summary>
        /// Update text string, option to set opacity. Otherwise fully visible opaque.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="opacity"></param>
        public void PlainUpdateText(string text, float opacity = 1)
        {
            tmpText = this.GetComponent<TMP_Text>();
            tmpText.text = text;
            tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, opacity);
        }

        /// <summary>
        /// Update text with colour.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colour"></param>
        public void PlainUpdateText(string text, Color colour)
        {
            tmpText = this.GetComponent<TMP_Text>();
            tmpText.text = text;
            tmpText.color = colour;
        }

        private void Awake()
        {
            tmpText = this.GetComponent<TMP_Text>();
        }

        private Coroutine FadeOutCoroutine = null;

        public void ColourThenFade(string text, Color color)
        {
            tmpText = this.GetComponent<TMP_Text>();
            this.tmpText.ForceMeshUpdate();
            this.tmpText.color = color;
            this.tmpText.text = text;
            this.tmpText.ForceMeshUpdate();

            if (FadeOutCoroutine != null)
            {
                StopCoroutine(FadeOutCoroutine);
            }

            FadeOutCoroutine = StartCoroutine(FadeOut());
        }

        public void ColourThenFade(int value)
        {
            this.ColourThenFade($"{GetSign(value)} {value}", this.GetColor(value));
        }

        private string GetSign(int value)
        {
            return value >= 0 ? "+" : ""; // Negative number has sign automatically.
        }

        private Color GetColor(int value)
        {
            return value >= 0 ? Color.green : Color.red;
        }

        /// <summary>
        /// Immediately change colour to start colour then fade to end colour.
        /// </summary>
        /// <param name="colorStart"></param>
        /// <param name="colorEnd"></param>
        public void ColourThenFadeToColour(Color colorStart, Color colorEnd, float time)
        {
            this.tmpText.ForceMeshUpdate();
            StartCoroutine(FadeFromTo(colorStart, colorEnd, time));
        }

        private IEnumerator FadeFromTo(Color startColor, Color endColor, float fadeTime)
        {
            tmpText.color = startColor;
            float elapsed = 0f;

            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                tmpText.color = Color.Lerp(startColor, endColor, elapsed / fadeTime);
                yield return null;
            }

            tmpText.color = endColor; // Ensure it ends exactly at the end color
        }

        private IEnumerator FadeOut()
        {
            float elapsed = 0f;
            Color startColor = tmpText.color;

            while (elapsed < fadeOutToInvisibleTime)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutToInvisibleTime);
                tmpText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                this.tmpText.ForceMeshUpdate();
                yield return null;
            }

            tmpText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
            FadeOutCoroutine = null;
        }
    }
}
