using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// To go on gameobject to display how many pickups and image type.
    /// </summary>
    public class MultiPickupFeedback : MonoBehaviour
    {
        [SerializeField] float fadeTime = 0.5f;

        [SerializeField] GameObject shellContainer;
        [SerializeField] Image shellImage;
        [SerializeField] TextWithFeedback shellText;

        [SerializeField] GameObject coralContainer;
        [SerializeField] Image coralImage;
        [SerializeField] TextWithFeedback coralText;

        [SerializeField] GameObject pearlContainer;
        [SerializeField] Image pearlImage;
        [SerializeField] TextWithFeedback pearlText;

        private void Awake()
        {
            shellContainer.SetActive(false);
            coralContainer.SetActive(false);
            pearlContainer.SetActive(false);
        }

        public void ShowPickups(List<Pickup> pickups)
        {
            var seashells = pickups.Count(x => x.PickupType == PickupType.PinkShell);
            var coral = pickups.Count(x => x.PickupType == PickupType.Coral);
            var pearl = pickups.Count(x => x.PickupType == PickupType.Pearl);

            ShowShellUpdate(seashells);
            ShowCoralUpdate(coral);
            ShowPearlUpdate(pearl);
        }

        private void ShowPearlUpdate(int value)
        {
            if (value == 0)
            {
                pearlContainer.SetActive(false);
                return;
            }

            pearlContainer.SetActive(true);
            pearlText.ColourThenFade(value); // TODO fadeTime for this.
            StartCoroutine(ShowThenFadeImage(pearlImage));
        }

        private IEnumerator ShowThenFadeImage(Image image)
        {
            float elapsed = 0f;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            Color startColor = image.color;

            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
                image.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }

            image.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }

        private void ShowCoralUpdate(int value)
        {
            if (value == 0)
            {
                coralContainer.SetActive(false);
                return;
            }

            coralContainer.SetActive(true);
            coralText.ColourThenFade(value);
            StartCoroutine(ShowThenFadeImage(coralImage));
        }

        private void ShowShellUpdate(int value)
        {
            if (value == 0)
            {
                shellContainer.SetActive(false);
                return;
            }

            shellContainer.SetActive(true);
            shellText.ColourThenFade(value);
            StartCoroutine(ShowThenFadeImage(shellImage));
        }
    }
}
