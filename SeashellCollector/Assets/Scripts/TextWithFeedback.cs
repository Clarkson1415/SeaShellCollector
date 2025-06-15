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

        public void PlainUpdateText(string text)
        {
            tmpText.color = Color.white;
            tmpText.text = text;
        }

        private void Awake()
        {
            tmpText = this.GetComponent<TMP_Text>();
            this.tmpText.text = string.Empty;
        }

        public void ColourThenFade(string text, Color color)
        {
            this.tmpText.ForceMeshUpdate();
            this.tmpText.color = color;
            this.tmpText.text = text;
            this.tmpText.ForceMeshUpdate();

            StartCoroutine(FadeOut());
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
        }
    }
}
