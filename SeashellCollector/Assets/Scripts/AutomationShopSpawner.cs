using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Spawns random shops that player can buy sandcastles from.
/// </summary>
public class AutomationShopSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shopPrefab; // Prefab for the shop to spawn
    [SerializeField] private int numberOfShopsPerRegion = 5;
    [SerializeField] private float minDistanceFromSandcastles = 8f;
    [SerializeField] private float minDistanceFromOtherShops = 8f;
    [SerializeField] private BoxCollider2D spawnerRegion;
    [SerializeField] private float TimeBetweenSpawns = 10f; // Time between shop spawns
    private List<GameObject> spawnedShops = new();
    [SerializeField] private List<GameObject> itemsForShops = new();

    private void Start()
    {
        StartCoroutine(SpawnShops());
    }

    private IEnumerator SpawnShops()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenSpawns);

            if (this.spawnedShops.Count < this.numberOfShopsPerRegion)
            {
                Vector3 spawnPosition = GetNewSpawnPosition();
                var closestSandcastle = Utility.GetClosest<Sandcastle>(spawnPosition);
                var closestShop = Utility.GetClosest<ItemShop>(spawnPosition);

                // While 
                // if closest sandcastle not null and too close = increment
                // if closetst shop not null and too close increment
                // if out of spawn region dont spawn.

                while ((closestSandcastle != null && (Vector3.Distance(spawnPosition, closestSandcastle.transform.position) < minDistanceFromSandcastles)) ||
                    ((closestShop != null && Vector3.Distance(spawnPosition, closestShop.transform.position) < minDistanceFromOtherShops)))
                {
                    closestSandcastle = Utility.GetClosest<Sandcastle>(spawnPosition);
                    closestShop = Utility.GetClosest<ItemShop>(spawnPosition);

                    spawnPosition = GetNewSpawnPosition();
                }

                // shouldn't be outside of region but added this check for extra safety
                if (!this.spawnerRegion.OverlapPoint(spawnPosition))
                {
                    this.SpawnShop(spawnPosition);
                }
            }
        }
    }

    private Vector3 GetNewSpawnPosition()
    {
        Vector3 spawnPosition = new(
                    Random.Range(-spawnerRegion.size.x / 2, spawnerRegion.size.x / 2),
                    0f, // Assuming a flat ground for the shops
                    Random.Range(-spawnerRegion.size.y / 2, spawnerRegion.size.y / 2)
                );
        return spawnPosition;
    }

    private void SpawnShop(Vector3 spawnPosition)
    {
        Debug.Log("Spawned Shop. Check items are being assigned for real because loooks like theyre not.");
        var shop = Instantiate(shopPrefab, spawnPosition, Quaternion.identity);
        spawnedShops.Add(shop);
        shop.GetComponent<ItemShop>().AllItemDrops = itemsForShops;
    }
}
