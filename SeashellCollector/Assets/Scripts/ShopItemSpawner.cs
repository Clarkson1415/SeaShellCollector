using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemSpawnLocations;

    public List<GameObject> AllItemDrops;

    public List<GameObject> current3items;

    /// <summary>
    /// Spawn 3 random item drops.
    /// </summary>
    public void SpawnRandoms()
    {
        foreach (var location in ItemSpawnLocations)
        {
            var randomIndex = Random.Range(0, AllItemDrops.Count);
            var newItem = Instantiate(AllItemDrops[randomIndex], location.transform);
            current3items.Add(newItem);
        }
    }

    public void OnItemBought()
    {
        foreach(var item in current3items)
        {
            if (!item.IsDestroyed())
            {
                Destroy(item);
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (var item in current3items)
        {
            if (item.IsDestroyed())
            {
                OnItemBought();
            }
        }
    }
}
