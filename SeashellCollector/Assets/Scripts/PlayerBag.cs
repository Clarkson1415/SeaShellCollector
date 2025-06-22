using TMPro;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    [SerializeField] private TMP_Text pinkShellCount; // pink shells.
    [SerializeField] private TMP_Text totalShellsText;
    private int maxCapacity;
    private int totalShells;

    /// <summary>
    /// Update fill amount of the bag UI As value between 0 and 1.
    /// </summary>
    /// <param name="fillAmount"></param>
    public void UpdatePinkShellCounter(int shells)
    {
        pinkShellCount.text = shells.ToString();
    }

    public void UpdateTotalShellCounter(int total)
    {
        totalShells = total;
        this.UpdateText();
    }

    public void UpdateMaxCapacity(int cap)
    {
        maxCapacity = cap;
        this.UpdateText();
    }

    private void UpdateText()
    {
        this.totalShellsText.text = $"{this.totalShells}/{this.maxCapacity}";
    }
}
