using System.Collections;
using UnityEngine;

public class ShopGuy : MonoBehaviour
{
    [SerializeField] private ShopItemSpawner spawner;
    private float timeEnteredShop;
    
    /// <summary>
    /// How long player has to be not touching shop guy for shop to dissapear.
    /// </summary>
    public float notInShopTimeout = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawner.SpawnRandoms();
        timeEnteredShop = Time.time;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(ShopTimeout());
    }

    IEnumerator ShopTimeout()
    {
        Debug.Log("todo add a cool animation like the shop items float back while shrinking to the shop guy before they dissapear.");
        Debug.Log("add animation where shop items appear from the shop owner starting as tiny small growing to their size while floating to their positions.");

        while (Time.time < timeEnteredShop + notInShopTimeout)
        {
            yield return null;
        }

        spawner.DespawnRandoms();
    }
}
