using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
        StartCoroutine(SpawnShopItemsOneAtATime());
    }

    IEnumerator SpawnShopItemsOneAtATime()
    {
        List<GameObject> itemsToSpawn = new();
        var numberOfItemsToSpawn = Mathf.Min(3, AllItemDrops.Count); // Ensure we don't try to spawn more items than available.

        List<int> usedIndicies = new();
        var possibleIndices = Enumerable.Range(0, AllItemDrops.Count).ToList();

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            possibleIndices.RemoveAll(i => usedIndicies.Contains(i)); // Remove already used indices
            var randomIndex = possibleIndices[UnityEngine.Random.Range(0, possibleIndices.Count)];
            usedIndicies.Add(randomIndex);
            itemsToSpawn.Add(AllItemDrops[randomIndex]);
        }

        Debug.Log("Player coin cash shop sound or pop, pop, pop for the items appearing.");
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            var newItem = Instantiate(itemsToSpawn[i], ItemSpawnLocations[i].transform);
            this.currentlySpawnedItems.Add(newItem);
            newItem.transform.localScale = Vector3.zero;
            newItem.transform.position = this.transform.position;
            newItem.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ChangeSizeAndMove(newItem.transform, ItemSpawnLocations[i].transform.position, Vector3.one));
            yield return new WaitForSeconds(animationDuration);
            newItem.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    [SerializeField] private float animationDuration = 3f; // Duration for the grow animation

    private IEnumerator ChangeSizeAndMove(Transform objTransform, Vector3 targetPosition, Vector3 targetScale)
    {
        Vector3 initialPosition = objTransform.position;
        Vector3 initialScale = objTransform.localScale;
        float elapsed = 0f;

        while (elapsed < Mathf.Max(animationDuration, animationDuration))
        {
            float tGrow = Mathf.Clamp01(elapsed / animationDuration);
            float tMove = Mathf.Clamp01(elapsed / animationDuration);

            // Lerp scale and position
            objTransform.localScale = Vector3.Lerp(initialScale, targetScale, tGrow);
            objTransform.position = Vector3.Lerp(initialPosition, targetPosition, tMove);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        objTransform.localScale = targetScale;
        objTransform.position = targetPosition;
    }

    public void DespawnRandoms()
    {
        RemoveAllShopItems();
    }

    private IEnumerator RemoveOneAtATime()
    {
        foreach (var item in currentlySpawnedItems)
        {
            item.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ChangeSizeAndMove(item.transform, this.transform.position, new Vector3(0, 0, 0)));
            yield return new WaitForSeconds(animationDuration);
            Destroy(item);
        }

        currentlySpawnedItems.Clear();
    }

    public void RemoveAllShopItems()
    {
        StartCoroutine(RemoveOneAtATime());
    }

    public void RemoveItem(GameObject item)
    {
        this.AllItemDrops.Remove(item);
        currentlySpawnedItems.Remove(item);
        Destroy(item);
        RemoveAllShopItems();
    }
}
