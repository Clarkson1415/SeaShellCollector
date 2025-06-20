using System;
using System.Collections;
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
        var startPos = this.transform.position;

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
            var randomIndex = possibleIndices[UnityEngine.Random.Range(0, possibleIndices.Count)];
            indices.Add(randomIndex);
            var newItem = Instantiate(AllItemDrops[randomIndex], location.transform);
            currentlySpawnedItems.Add(newItem);

            // Start animation to spots.
            Debug.Log("Player coin cash shop sound or pop , pop for the items");
            newItem.transform.localScale = Vector3.zero;
            newItem.transform.position = this.transform.position;
            newItem.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ChangeSizeAndMove(newItem.transform, location.transform.position, Vector3.one, () => newItem.gameObject.GetComponent<BoxCollider2D>().enabled = true));
        }
    }

    [SerializeField] private float growDuration = 3f; // Duration for the grow animation
    [SerializeField] private float moveDuration = 3f; // Duration for the move

    private IEnumerator ChangeSizeAndMove(Transform objTransform, Vector3 targetPosition, Vector3 targetScale, Action action)
    {
        Vector3 initialPosition = objTransform.position;
        Vector3 initialScale = objTransform.localScale;
        float elapsed = 0f;

        while (elapsed < Mathf.Max(growDuration, moveDuration))
        {
            float tGrow = Mathf.Clamp01(elapsed / growDuration);
            float tMove = Mathf.Clamp01(elapsed / moveDuration);

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

    public void RemoveAllShopItems()
    {
        foreach (var item in currentlySpawnedItems)
        {
            if (!item.IsDestroyed())
            {
                StartCoroutine(ChangeSizeAndMove(item.transform, this.transform.position, new Vector3(0, 0, 0), () => Destroy(item)));
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
