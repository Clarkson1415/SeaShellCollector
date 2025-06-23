using Assets.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// To display pickup icons above the castle when it has them.
/// </summary>
public class CastlePickupDisplay : MonoBehaviour
{
    [SerializeField] private Image shellImage;
    [SerializeField] private Image coralImage;
    [SerializeField] private Image pearlImage;


    public void UpdatePickupDisplay(List<Pickup> pickups)
    {
        shellImage.enabled = pickups.Any(x => x.PickupType == PickupType.PinkShell);
        coralImage.enabled = pickups.Any(x => x.PickupType == PickupType.Coral);
        pearlImage.enabled = pickups.Any(x => x.PickupType == PickupType.Pearl);
    }
}
