using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShopItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemSpawnLocations;

    public List<GameObject> AllItemDrops;

    public List<GameObject> current3items = new();

    /// <summary>
    /// Spawn 3 random item drops.
    /// </summary>
    public void SpawnRandoms()
    {
        List<int> indices = new();
        foreach (var location in ItemSpawnLocations)
        {
            var randomIndex = Random.Range(0, AllItemDrops.Count);
            while (indices.Contains(randomIndex))
            {
                randomIndex = Random.Range(0, AllItemDrops.Count);
            }
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

        current3items.Clear();
    }

    private void FixedUpdate()
    {
        if (!current3items.Any())
        {
            return;
        }

        foreach (var item in current3items.ToList())
        {
            if (item.IsDestroyed())
            {
                OnItemBought();
            }
        }
    }
}
