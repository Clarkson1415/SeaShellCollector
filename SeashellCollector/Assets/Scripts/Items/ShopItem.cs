using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public class ShopItem : MonoBehaviour
{
    public float Timeout = -1f;

    /// <summary>
    /// Whether should be removed from the shop.
    /// </summary>
    public bool OneTimeOnly;

    public int PinkShellCost;

    public int CoralCost;

    public int PearlCost;

    public string Name;

    public string Description;

    [SerializeField] TextWithFeedback CostText;
    [SerializeField] TextWithFeedback NameText;
    [SerializeField] TextWithFeedback DescriptionText;

    public List<ItemEffect> Effects = new();

    /// <summary>
    /// Shop belongs to. Could be null after bought, as shop closes.
    /// </summary>
    public ItemShop? ShopBelongsTo;

    public void FlashTextRed()
    {
        this.CostText.ColourThenFadeToColour(Color.red, Color.white, 1f);
        this.NameText.ColourThenFadeToColour(Color.red, Color.white, 1f);
        this.DescriptionText.ColourThenFadeToColour(Color.red, Color.white, 1f);
    }

    private void Awake()
    {
        this.CostText.PlainUpdateText(this.PinkShellCost.ToString());
        this.NameText.PlainUpdateText(this.Name.ToString());
        this.DescriptionText.PlainUpdateText(this.Description.ToString());
    }

    public virtual void ApplyItemEffects(Player player)
    {
        foreach (var effect in Effects)
        {
            effect.Apply(player);
        }
    }

    public virtual void RemoveEffects(Player player)
    {
        foreach (var effect in Effects)
        {
            effect.Remove(player);
        }
    }

    public bool IsDoingAnimation;

    public void Spawn(Vector3 finishPosition, float animationDuration)
    {
        IsDoingAnimation = true;
        this.transform.localScale = Vector3.zero;
        this.transform.position = this.ShopBelongsTo.transform.position;
        this.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(ChangeSizeAndMove(finishPosition, Vector3.one, animationDuration));
    }


    public void Despawn(Vector3 finishPosition, float animationDuration)
    {
        IsDoingAnimation = true;

        if (this == null || gameObject == null)
        {
            Debug.Log($"this object was already destroyed");
            return;
        }

        if (TryGetComponent<BoxCollider2D>(out var collider))
        {
            collider.enabled = false;
        }

        StartCoroutine(ChangeSizeAndMove(finishPosition, Vector3.zero, animationDuration));
    }

    private IEnumerator ChangeSizeAndMove(Vector3 targetPosition, Vector3 targetScale, float animationDuration)
    {
        Vector3 initialPosition = this.transform.position;
        Vector3 initialScale = this.transform.localScale;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            float tGrow = Mathf.Clamp01(elapsed / animationDuration);
            float tMove = Mathf.Clamp01(elapsed / animationDuration);

            // Lerp scale and position
            this.transform.localScale = Vector3.Lerp(initialScale, targetScale, tGrow);
            this.transform.position = Vector3.Lerp(initialPosition, targetPosition, tMove);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        this.transform.localScale = targetScale;
        this.transform.position = targetPosition;
        IsDoingAnimation = false;
    }
}
