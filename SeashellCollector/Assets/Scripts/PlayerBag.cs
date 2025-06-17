using UnityEngine;
using UnityEngine.UI;

public class PlayerBag : MonoBehaviour
{
    [SerializeField] private Image BagFill;

    /// <summary>
    /// Update fill amount of the bag UI As value between 0 and 1.
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdateBagFill(float fillAmount)
    {
        BagFill.fillAmount = fillAmount;
    }


}
