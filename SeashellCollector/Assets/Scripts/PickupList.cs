using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is the list of pickups in the players top UI.
/// </summary>
public class PickupList : MonoBehaviour
{
    public void AddToList(ShopItem pickup)
    {
        // Create a new GameObject
        GameObject child = new GameObject("ChildWithImage");

        // Set this object as the parent
        child.transform.SetParent(this.transform, false); // 'false' keeps local scale/position

        // Add an Image component
        Image image = child.AddComponent<Image>();

        image.sprite = pickup.GetComponent<SpriteRenderer>().sprite;
    }
}
