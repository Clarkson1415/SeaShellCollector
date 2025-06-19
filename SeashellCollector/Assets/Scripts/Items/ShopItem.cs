using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    /// <summary>
    /// Whether should be removed from the shop.
    /// </summary>
    public bool OneTimeOnly;

    public int EffectValue;

    [SerializeField] private bool HasTimeout;

    [SerializeField] private float effectTimeout;

    public int Cost;

    public string Name;

    public string Description;

    [SerializeField] TextWithFeedback CostText;
    [SerializeField] TextWithFeedback NameText;
    [SerializeField] TextWithFeedback DescriptionText;

    public List<ItemEffect> Effects = new();

    public void FlashTextRed()
    {
        this.CostText.ColourThenFadeToColour(Color.red, Color.white, 1f);
        this.NameText.ColourThenFadeToColour(Color.red, Color.white, 1f);
        this.DescriptionText.ColourThenFadeToColour(Color.red, Color.white, 1f);
    }

    private void Awake()
    {
        this.CostText.PlainUpdateText(this.Cost.ToString());
        this.NameText.PlainUpdateText(this.Name.ToString());
        this.DescriptionText.PlainUpdateText(this.Description.ToString());
    }

    public void ApplyItemEffect(Player player)
    {
        if (this.HasTimeout)
        {
            foreach (var effect in Effects)
            {
                effect.Apply(player, this.EffectValue, this.effectTimeout);
            }

            return;
        }

        foreach (var effect in Effects)
        {
            effect.Apply(player, this.EffectValue);
        }
    }

    public void RemoveEffect(Player player)
    {
        foreach (var effect in Effects)
        {
            effect.Remove(player, this.EffectValue);
        }
    }
}
