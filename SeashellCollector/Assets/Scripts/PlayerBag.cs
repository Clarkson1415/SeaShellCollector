using Assets.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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
    private void UpdatePinkShellCounter(int shells)
    {
        pinkShellCount.text = shells.ToString();
    }

    /// <summary>
    /// Updates total, pink shells, coral and pearl counter.
    /// </summary>
    /// <param name="total"></param>
    public void UpdateMoneyCounterUi(List<Pickup> total)
    {
        totalShells = total.Count;
        this.UpdateText();

        // TODO update pink shells here
        this.UpdatePinkShellCounter(total.Where(x => x.PickupType == PickupType.PinkShell).Count());
        Debug.Log("Update coral, update pearl counter");
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
