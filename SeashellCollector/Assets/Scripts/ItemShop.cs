using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    private float timeExitShop;
    [HideInInspector] public Coroutine ShopTimeoutCoroutine = null;

    /// <summary>
    /// How long player has to be not touching shop guy for shop to dissapear.
    /// </summary>
    public float notInShopTimeout = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Player>(out var p))
        {
            return;
        }

        // If shop not got items then spawn them.
        if (!this.currentlySpawnedItems.Any())
        {
            this.SpawnRandoms();
            return;
        }

        // If close routine in progress stop it. It will restart when you leave shop again.
        if (ShopTimeoutCoroutine != null)
        {
            StopCoroutine(ShopTimeoutCoroutine);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ShopTimeoutCoroutine != null)
        {
            StopCoroutine(ShopTimeoutCoroutine);
        }

        this.ShopTimeoutCoroutine = StartCoroutine(ShopTimeout());
    }

    IEnumerator ShopTimeout()
    {
        Debug.Log("todo add a cool animation like the shop items float back while shrinking to the shop guy before they dissapear.");
        Debug.Log("add animation where shop items appear from the shop owner starting as tiny small growing to their size while floating to their positions.");

        yield return new WaitForSeconds(notInShopTimeout);        
        this.RemoveAllShopItemsAnimated();
        this.ShopTimeoutCoroutine = null;
    }

    public List<GameObject> ItemSpawnLocations;

    public List<GameObject> AllItemDrops;

    [HideInInspector] public List<GameObject> currentlySpawnedItems = new();

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
            newItem.GetComponent<ShopItem>().ShopBelongsTo = this;
            this.currentlySpawnedItems.Add(newItem);
            newItem.transform.localScale = Vector3.zero;
            newItem.transform.position = this.transform.position;
            newItem.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ChangeSizeAndMove(newItem.transform, ItemSpawnLocations[i].transform.position, Vector3.one));
            yield return new WaitForSeconds(animationDuration + 0.1f);
            newItem.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    [SerializeField] private float animationDuration = 3f; // Duration for the grow animation

    private IEnumerator ChangeSizeAndMove(Transform objTransform, Vector3 targetPosition, Vector3 targetScale)
    {
        if (objTransform == null)
        {
            yield break; // Exit if the object transform is null
        }

        Vector3 initialPosition = objTransform.position;
        Vector3 initialScale = objTransform.localScale;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            if (objTransform == null)
            {
                yield break; // Exit if the object transform is null
            }

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
    
    private IEnumerator RemoveOneAtATime(bool destroyShop)
    {
        var itemsCopy = new List<GameObject>(currentlySpawnedItems);
        foreach (var item in itemsCopy)
        {
            item.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ChangeSizeAndMove(item.transform, this.transform.position, new Vector3(0, 0, 0)));
            yield return new WaitForSeconds(animationDuration + 0.1f);
            Destroy(item);
        }

        currentlySpawnedItems.Clear();

        if (destroyShop)
        {
            if (ShopTimeoutCoroutine != null)
            {
                StopCoroutine(ShopTimeoutCoroutine);
            }

            Destroy(this.gameObject);
        }

        RemovingItems = null; // Reset the coroutine reference
    }

    private Coroutine RemovingItems = null;

    public void RemoveAllShopItemsAnimated(bool destroyShop = false)
    {
        if (RemovingItems != null)
        {
            StopCoroutine(RemovingItems);
            return;
        }

        if (destroyShop)
        {
            this.animationDuration = 0f;
        }

        RemovingItems = StartCoroutine(RemoveOneAtATime(destroyShop));
    }

    public void BoughtItem(ShopItem item)
    {
        if (item.OneTimeOnly)
        {
            this.AllItemDrops.Remove(item.gameObject);
        }

        currentlySpawnedItems.Remove(item.gameObject);
        Destroy(item.gameObject);
        bool shouldDestroyShop = item is AutomationShopItem;
        RemoveAllShopItemsAnimated(shouldDestroyShop);
    }
}
