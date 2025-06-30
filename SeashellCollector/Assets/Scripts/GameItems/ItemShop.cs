using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#nullable enable

public class ItemShop : MonoBehaviour
{
    /// <summary>
    /// If this shop was spawned by a sandcastle, this will be set to that sandcastle.
    /// </summary>
    public Sandcastle? SandcastleSpawnedBy;

    public bool IsOpen { get => this.currentlySpawnedItems.Any(); }

    [HideInInspector] public Coroutine? ShopTimeoutCoroutine = null;

    [SerializeField] private float animationDuration = 3f;

    /// <summary>
    /// How long player has to be not touching shop guy for shop to dissapear.
    /// </summary>
    public float notInShopTimeout = 5f;

    public List<GameObject> ItemSpawnLocations;

    public List<GameObject> AllItemDrops;

    [HideInInspector] public List<ShopItem> currentlySpawnedItems = new();

    private ItemShop[] GetActiveShops()
    {
        return FindObjectsByType<ItemShop>(FindObjectsSortMode.None);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<Player>(out var p))
        {
            return;
        }

        var shopOpen = this.GetActiveShops().FirstOrDefault(x => x.IsOpen);
        if (shopOpen != null && shopOpen != this)
        {
            shopOpen.CloseShop();
        }

        // If close routine in progress stop it. It will restart when you leave shop again.
        if (ShopTimeoutCoroutine != null)
        {
            StopCoroutine(ShopTimeoutCoroutine);
        }

        // If shop not spawned items then spawn them.
        if (!this.currentlySpawnedItems.Any())
        {
            StartCoroutine(SpawnRandomItems());
        }
    }

    private IEnumerator SpawnRandomItems()
    {
        List<GameObject> itemsToSpawn = new();
        var numberOfItemsToSpawn = Mathf.Min(3, AllItemDrops.Count); // Ensure we don't try to spawn more items than available.

        List<int> usedIndicies = new();
        var possibleIndices = Enumerable.Range(0, AllItemDrops.Count).ToList();

        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            possibleIndices.RemoveAll(i => usedIndicies.Contains(i)); // Remove already used indices
            var randomIndex = possibleIndices[Random.Range(0, possibleIndices.Count)];
            usedIndicies.Add(randomIndex);
            itemsToSpawn.Add(AllItemDrops[randomIndex]);
        }

        Debug.Log("Player coin cash shop sound or pop, pop, pop for the items appearing.");
        for (int i = 0; i < itemsToSpawn.Count; i++)
        {
            var newItem = Instantiate(itemsToSpawn[i], ItemSpawnLocations[i].transform);
            var shopItem = newItem.GetComponent<ShopItem>();
            this.currentlySpawnedItems.Add(shopItem);
            shopItem.ShopBelongsTo = this;

            // When all shop item animations done enable colliders.
            shopItem.Spawn(ItemSpawnLocations[i].transform.position, this.animationDuration);
            yield return new WaitForSeconds(animationDuration);
        }

        StartCoroutine(WhenFinishedAnimatingEnableBoxCollider());
    }

    private IEnumerator WhenFinishedAnimatingEnableBoxCollider()
    {
        var loopCount = 0;
        while (this.currentlySpawnedItems.Any(x => x.IsDoingAnimation))
        {
            loopCount++;
            Debug.Log($"Loop {loopCount}: Spawning animation. Items animating: {this.currentlySpawnedItems.Count(x => x.IsDoingAnimation)}");
            yield return null; // Wait until all animations are done
        }

        foreach (var obj in this.currentlySpawnedItems)
        {
            obj.GetComponent<BoxCollider2D>().enabled = true;
        }

        yield return null;
    }

    private IEnumerator WhenFinishedAnimatingDestroyItems()
    {
        var loopCount = 0;
        while (this.currentlySpawnedItems.Any(x => x.IsDoingAnimation))
        {
            loopCount++;
            yield return null; // Wait until all animations are done
        }

        foreach (var obj in this.currentlySpawnedItems)
        {
            Destroy(obj.gameObject);
        }

        this.currentlySpawnedItems.Clear();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ShopTimeoutCoroutine != null)
        {
            StopCoroutine(ShopTimeoutCoroutine);
            ShopTimeoutCoroutine = null;
        }

        if (!this.gameObject.activeSelf)
        {
            StopAllCoroutines();
            return;
        }

        this.ShopTimeoutCoroutine = StartCoroutine(ShopTimeout());
    }

    IEnumerator ShopTimeout()
    {
        Debug.Log("todo add a cool animation like the shop items float back while shrinking to the shop guy before they dissapear.");
        Debug.Log("add animation where shop items appear from the shop owner starting as tiny small growing to their size while floating to their positions.");

        yield return new WaitForSeconds(notInShopTimeout);
        StartCoroutine(RemoveOneAtATime());
        this.ShopTimeoutCoroutine = null;
    }

    public void CloseShop()
    {
        if (this.ShopTimeoutCoroutine != null)
        {
            StopCoroutine(this.ShopTimeoutCoroutine);
        }

        if (this.gameObject.activeSelf == false)
        {
            Debug.LogWarning("Shop already closed, cannot close again.");
            return;
        }

        StartCoroutine(RemoveOneAtATime());
    }

    public void BoughtItem(ShopItem item)
    {
        if (this.ShopTimeoutCoroutine != null)
        {
            StopCoroutine(this.ShopTimeoutCoroutine);
        }

        if (item.OneTimeOnly)
        {
            var matching = this.AllItemDrops.FirstOrDefault(x => x.GetComponent<ShopItem>().Name == item.Name);
            this.AllItemDrops.Remove(matching);
        }

        Destroy(item.gameObject);
        this.currentlySpawnedItems.Remove(item);

        StopAllCoroutines();

        if (item is AutomationShopItem)
        {
            foreach (var spawnedItem in this.currentlySpawnedItems)
            {
                Destroy(spawnedItem.gameObject);
            }

            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(RemoveOneAtATime());
        }
    }

    private IEnumerator RemoveOneAtATime()
    {
        Debug.Log("callong one at a time remove");
        var listCopy = new List<ShopItem>(this.currentlySpawnedItems); // Make a copy to avoid modifying the collection while iterating.
        foreach (var item in listCopy)
        {
            item.Despawn(this.transform.position, animationDuration);
            yield return new WaitForSeconds(this.animationDuration);
        }

        StartCoroutine(WhenFinishedAnimatingDestroyItems());
    }
}
