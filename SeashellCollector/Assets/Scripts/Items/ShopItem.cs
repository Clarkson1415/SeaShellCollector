using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

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

    public ItemShop ShopBelongsTo;

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
}
