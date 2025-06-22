using Assets.Scripts;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>
/// Spawns random shops that player can buy sandcastles from.
/// </summary>
public class AutomationShopSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shopPrefab; // Prefab for the shop to spawn
    [SerializeField] private int numberOfShopsPerRegion = 5;
    [SerializeField] private float minDistanceApart = 5f;
    [SerializeField] private BoxCollider2D spawnerRegion;
    [SerializeField] private float TimeBetweenSpawns = 10f; // Time between shop spawns
    private List<GameObject> shops = new();
    [SerializeField] private List<GameObject> itemsForShops = new();

    private void Start()
    {
        StartCoroutine(SpawnShops());
    }

    private IEnumerator SpawnShops()
    {
        yield return new WaitForSeconds(TimeBetweenSpawns);

        if (this.shops.Count < this.numberOfShopsPerRegion)
        {
            Vector3 spawnPosition = new(
                Random.Range(-spawnerRegion.size.x / 2, spawnerRegion.size.x / 2),
                0f, // Assuming a flat ground for the shops
                Random.Range(-spawnerRegion.size.y / 2, spawnerRegion.size.y / 2)
            );

            var closestShop = Utility.GetClosest<Sandcastle>(spawnPosition);

            while (closestShop != null && Vector3.Distance(spawnPosition, closestShop.transform.position) < minDistanceApart)
            {
                spawnPosition.x += 1;
                spawnPosition.y += 1;

                if (!this.spawnerRegion.OverlapPoint(spawnPosition)) // if outside spawn region give up.
                    yield break;
            }

            var shop = Instantiate(shopPrefab, spawnPosition, Quaternion.identity);
            shops.Add(shop);
            shop.GetComponent<ItemShop>().AllItemDrops = itemsForShops;
        }
    }
}
