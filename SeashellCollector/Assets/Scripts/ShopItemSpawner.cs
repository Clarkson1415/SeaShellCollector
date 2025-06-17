using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemSpawnLocations;

    public List<GameObject> AllItemDrops;

    public List<GameObject> currentlySpawnedItems = new();

    /// <summary>
    /// Spawn 3 random item drops.
    /// </summary>
    public void SpawnRandoms()
    {
        List<int> indices = new();

        if (this.AllItemDrops.Count < 3)
        {
            foreach (var item in this.AllItemDrops)
            {
                var newItem = Instantiate(item);
                currentlySpawnedItems.Add(newItem);
            }

            return;
        }

        foreach (var location in ItemSpawnLocations)
        {
            var possibleIndices = Enumerable.Range(0, AllItemDrops.Count).ToList();
            possibleIndices.RemoveAll(i => indices.Contains(i)); // Remove already used indices
            var randomIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
            indices.Add(randomIndex);
            var newItem = Instantiate(AllItemDrops[randomIndex], location.transform);
            currentlySpawnedItems.Add(newItem);
        }
    }

    public void DespawnRandoms()
    {
        RemoveAllShopItems();
    }

    public void RemoveAllShopItems()
    {
        foreach (var item in currentlySpawnedItems)
        {
            if (!item.IsDestroyed())
            {
                Destroy(item);
            }
        }

        currentlySpawnedItems.Clear();
    }

    public void RemoveItem(GameObject item)
    {
        this.AllItemDrops.Remove(item);
        Destroy(item.gameObject);
        RemoveAllShopItems();
    }
}
