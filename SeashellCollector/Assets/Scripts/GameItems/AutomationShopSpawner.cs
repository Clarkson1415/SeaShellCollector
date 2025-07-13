using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns random shops that player can buy sandcastles from.
/// </summary>
public class AutomationShopSpawner : OffCamSpawnerUsesRatePerUnit
{
    [SerializeField] private BoxCollider2D spawnerRegion;
    [SerializeField] private List<GameObject> itemsForShops = new();

    // DO NOT HAVE A awake METHOD, the Spawner base class has one. Otherwise Spawner.Awake() will not be called.
    protected override void DoToItemAfterSpawn(GameObject newSpawnedItem)
    {
        newSpawnedItem.GetComponent<ItemShop>().AllItemDrops = itemsForShops;
    }

    protected override float GetSpawnInterval()
    {
        return UnityEngine.Random.Range(this.minTimeInverval, this.maxTimeInterval);
    }
}
