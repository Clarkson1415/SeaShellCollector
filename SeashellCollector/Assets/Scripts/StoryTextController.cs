using System;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
#nullable enable

public class StoryTextController : MonoBehaviour
{
    public Player player1;

    [SerializeField] TMP_Text text;

    [SerializeField] GameObject textBoxBG;

    /// <summary>
    /// For Fades for everything
    /// </summary>
    [SerializeField] private float fadeDuration = 1f;

    [SerializeField] private AudioSource gameMusic;

    [SerializeField] private float amountToTurnDownGameMusic = 0.5f;

    [SerializeField] private AudioSource InGameWaterSound;

    [SerializeField] private AudioSource ForTextWaterSound;

    private readonly int[] pickupMilestones = { 20, 50, 200, 500, 800, 1200, 1500, 2000, 2500, 3500, 4000, 6000, 10000 };

    private readonly string[] pickupMilestoneText = { "What are you collecting for? Or Maybe you don't know yet...", "It feels good to collect doesn't it…", "You’re starting to get the hang of it. Shells, coral, pearls. Value. Numbers. Meaing?", "Big numbers, Do you feel superior? Or emptier?", "You've learned to optimize. To accumulate. But what happens if you take it all?", "You thought the beach used to feel bigger. Now it’s just a loop.", "You're not sure when you stopped noticing the waves. When you stopped looking up.", "What would you do for answers? Maybe its what you're not doing", "The beach is empty now. Or maybe it always was. Maybe it was you that filled it. OR The beach is empty now. Or maybe it always was. Maybe it was you’re hopes and dreams that filled it." };

    private int currentMilestoneIndex = 0;

    private CinemachineVolumeSettings? volumeSettings;

    private IEnumerator FadeOutGameSound()
    {
        this.OriginalGameMusicVolume = gameMusic.volume;
        float targetGameMusicVolume = this.OriginalGameMusicVolume - amountToTurnDownGameMusic;
        this.OriginalGameWaterSoundVolume = InGameWaterSound.volume;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            gameMusic.volume = Mathf.Lerp(this.OriginalGameMusicVolume, targetGameMusicVolume, elapsed / fadeDuration);
            ForTextWaterSound.volume = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            InGameWaterSound.volume = Mathf.Lerp(InGameWaterSound.volume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        gameMusic.volume = targetGameMusicVolume;
        ForTextWaterSound.volume = 1f;
        InGameWaterSound.volume = 0f;
    }

    private float OriginalGameWaterSoundVolume;
    private float OriginalGameMusicVolume;

    private IEnumerator FadeInGameSound()
    {
        float startVolume = gameMusic.volume;
        float targetGameMusicVolume = startVolume - amountToTurnDownGameMusic;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            gameMusic.volume = Mathf.Lerp(startVolume, OriginalGameMusicVolume, elapsed / fadeDuration);
            ForTextWaterSound.volume = Mathf.Lerp(ForTextWaterSound.volume, 0, elapsed / fadeDuration);
            InGameWaterSound.volume = Mathf.Lerp(0, OriginalGameWaterSoundVolume, elapsed / fadeDuration);
            yield return null;
        }

        gameMusic.volume = this.OriginalGameMusicVolume;
        ForTextWaterSound.volume = 0f;
        InGameWaterSound.volume = this.OriginalGameWaterSoundVolume;
    }

    private Coroutine? CameraFading = null;

    private IEnumerator FadeCameraToBlackWhite()
    {
        if (!volumeSettings!.Profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            yield break;

        this.normalSaturation = colorAdjustments.saturation.value;
        float end = -100f; // full black and white
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            colorAdjustments.saturation.value = Mathf.Lerp(normalSaturation, end, t);
            yield return null;
        }

        colorAdjustments.saturation.value = end;
        CameraFading = null;
    }

    private float normalSaturation = 0f;

    private IEnumerator FadeCameraBackToNormal()
    {
        if (!volumeSettings!.Profile.TryGet<ColorAdjustments>(out var colorAdjustments))
            yield break;

        float start = colorAdjustments.saturation.value;
        float end = normalSaturation;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            colorAdjustments.saturation.value = Mathf.Lerp(start, end, t);
            yield return null;
        }

        colorAdjustments.saturation.value = end;
        CameraFading = null;
    }

    private void Start()
    {
        volumeSettings = FindFirstObjectByType<CinemachineVolumeSettings>();
        if (volumeSettings == null)
        {
            throw new ArgumentNullException("vol settings");
        }

        this.TextBoxfullSizeScale = this.textBoxBG.transform.localScale;
    }

    void Update()
    {
        int pickupCount = player1.GetCopyOfPickups().Count;

        // Check all untriggered milestones in order
        if (this.textBoxBG.activeSelf || this.LoadingInTextAndAnim || ScalingTextBox != null || this.CameraFading != null)
        {
            return; // already showing text, skip further checks
        }

        if (currentMilestoneIndex < pickupMilestones.Length && pickupCount >= pickupMilestones[currentMilestoneIndex])
        {
            int milestone = pickupMilestones[currentMilestoneIndex];
            Debug.Log($"Story beat {currentMilestoneIndex} reached at {milestone} pickups: {pickupMilestoneText[currentMilestoneIndex]}");
            StartStoryText();
        }
    }

    [SerializeField] private float timeBetweenLetters = 0.3f;

    private void StartStoryText()
    {
        // Animate textbox appear
        this.text.gameObject.SetActive(true);
        this.CameraFading = StartCoroutine(FadeCameraToBlackWhite());
        StartCoroutine(FadeOutGameSound());
        this.textBoxBG.SetActive(true);
        ScalingTextBox = StartCoroutine(ScalePopIn(this.textBoxBG.transform, 0.5f));
    }

    private bool LoadingInTextAndAnim;

    private Vector3 TextBoxfullSizeScale;

    public IEnumerator ScalePopIn(Transform target, float duration)
    {
        LoadingInTextAndAnim = true;
        
        var startVector = new Vector3(0, TextBoxfullSizeScale.y, TextBoxfullSizeScale.z);
        target.localScale = startVector;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(startVector, TextBoxfullSizeScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = TextBoxfullSizeScale; // Ensure final scale is exact

        // write text
        this.text.text = "";
        var milestoneTExt = pickupMilestoneText[currentMilestoneIndex];
        for (var i = 0; i < milestoneTExt.Length; i++)
        {
            this.text.text += milestoneTExt[i];
            yield return new WaitForSeconds(timeBetweenLetters);
        }

        LoadingInTextAndAnim = false;
        ScalingTextBox = null;
    }

    public IEnumerator ScalePopOut(Transform target, float duration)
    {
        Vector3 originalScale = target.localScale;
        var finalScale = new Vector3(0, originalScale.y, originalScale.z);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(originalScale, finalScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = finalScale; // Ensure final scale is exact
        target.gameObject.SetActive(false);
        currentMilestoneIndex++;
        ScalingTextBox = null;
    }

    public void OnSpaceOrEnter(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        // Return to game.
        if (ScalingTextBox == null && !this.LoadingInTextAndAnim)
        {
            StopAllCoroutines();
            this.text.text = "";
            this.text.gameObject.SetActive(false);
            StartCoroutine(FadeInGameSound());
            this.CameraFading = StartCoroutine(FadeCameraBackToNormal());
            ScalingTextBox = StartCoroutine(ScalePopOut(this.textBoxBG.transform, 0.5f));
            return;
        }

        // Skip text
        if (this.LoadingInTextAndAnim)
        {
            this.text.text = this.pickupMilestoneText[currentMilestoneIndex];
            StopAllCoroutines();
            LoadingInTextAndAnim = false;

            // TODO have a Set To On function that will just set all values to should be like saturation.
            ScalingTextBox = null;
            this.CameraFading = null;
        }
    }

    private Coroutine? ScalingTextBox = null;
}
