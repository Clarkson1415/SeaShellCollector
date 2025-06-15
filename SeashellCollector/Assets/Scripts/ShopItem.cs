using Assets.Scripts;
using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public int Cost;

    public string Name;

    [SerializeField] TextWithFeedback CostText;
    [SerializeField] TextWithFeedback NameText;

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
}
