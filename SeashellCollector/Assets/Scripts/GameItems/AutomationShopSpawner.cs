using Assets.Scripts;
using Assets.Scripts.GameItems;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns random shops that player can buy sandcastles from.
/// </summary>
public class AutomationShopSpawner : Spawner
{
    [SerializeField] private BoxCollider2D spawnerRegion;
    [SerializeField] private List<GameObject> itemsForShops = new();

    [SerializeField] private float minDistanceFromSandcastles = 10f; // Minimum distance from sandcastles to spawn shops
    [SerializeField] private float minDistanceFromOtherShops = 5f; // Minimum distance from other shops to spawn new shop

    // DO NOT HAVE A awake METHOD, the Spawner base class has one. Otherwise Spawner.Awake() will not be called.

    protected override void DoToItemAfterSpawn(GameObject newSpawnedItem)
    {
        newSpawnedItem.GetComponent<ItemShop>().AllItemDrops = itemsForShops;
    }

    protected override float GetMaxX()
    {
        return this.spawnerRegion.bounds.max.x;
    }

    protected override float GetMaxY()
    {
        return this.spawnerRegion.bounds.max.y;
    }

    protected override float GetMinX()
    {
        return this.spawnerRegion.bounds.min.x;
    }

    protected override float GetMinY()
    {
        return this.spawnerRegion.bounds.min.y;
    }

    protected override bool SpawnConditionsAreMet(Vector2 spawnPosition)
    {
        return IsFarEnoughFromSandAndShop(spawnPosition) && spawnerRegion.bounds.Contains(spawnPosition);
    }

    private bool IsFarEnoughFromSandAndShop(Vector2 spawnPosition)
    {
        var closestSandcastle = Utility.GetClosest<Sandcastle>(spawnPosition);
        var closestShop = Utility.GetClosest<ItemShop>(spawnPosition);

        var farFromSandC = closestSandcastle != null && Vector3.Distance(spawnPosition, closestSandcastle.transform.position) >= minDistanceFromSandcastles;
        var farFromShop = closestShop != null && Vector3.Distance(spawnPosition, closestShop.transform.position) >= minDistanceFromOtherShops;
        return farFromSandC && farFromShop;
    }
}
