using TMPro;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    [SerializeField] private TMP_Text count;
    [SerializeField] private TMP_Text capacityText;
    [SerializeField] private TMP_Text totalSHells;

    /// <summary>
    /// Update fill amount of the bag UI As value between 0 and 1.
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdatePinkShellCounter(int shells)
    {
        count.text = shells.ToString();
    }

    public void UpdateTotalShellCounter(int total)
    {
        this.totalSHells.text = total.ToString();
    }

    public void UpdateMaxCapacity(int cap)
    {
        capacityText.text = cap.ToString();
    }
}
