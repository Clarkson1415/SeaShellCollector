using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int Cost;

    public string Name;

    [SerializeField] TextWithFeedback CostText;
    [SerializeField] TextWithFeedback NameText;

    public List<ItemEffect> Effects = new();

    public void FlashTextRed()
    {
        this.CostText.ColourThenFadeToColour(Color.red, Color.white, 1f);
        this.NameText.ColourThenFadeToColour(Color.red, Color.white, 1f);
    }

    private void Awake()
    {
        this.CostText.PlainUpdateText(this.Cost.ToString());
        this.NameText.PlainUpdateText(this.Name.ToString());
    }

    public void ApplyItemEffect(Player player)
    {
        foreach(var effect in Effects)
        {
            effect.Apply(player);
        }
    }

    /// <summary>
    /// Function To be called when the item is bought. e.g. For storage box it will spawn a storage box down the bottom of the screen. So maybe this needs to be the shop item parent class then? but then I have to make a separate class for each item :( 
    /// </summary>
    //public void OnBought()
    //{
    //    throw new NotImplementedException();
    //}
}
