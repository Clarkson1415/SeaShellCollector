using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is the list of pickups in the players top UI.
/// </summary>
public class PickupList : MonoBehaviour
{
    private Dictionary<ShopItem, GameObject> pickup_index = new();

    public void AddToList(ShopItem pickup)
    {
        // Create a new GameObject
        GameObject itemImage = new("ItemImage");
        pickup_index.Add(pickup, itemImage);

        // Set this object as the parent
        itemImage.transform.SetParent(this.transform, false); // 'false' keeps local scale/position

        // Add an Image component
        Image image = itemImage.AddComponent<Image>();

        image.sprite = pickup.GetComponent<SpriteRenderer>().sprite;
    }

    public void Remove(ShopItem item)
    {
        Destroy(pickup_index[item]);
        pickup_index.Remove(item);
    }
}
