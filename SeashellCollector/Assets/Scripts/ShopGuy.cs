using UnityEngine;

public class ShopGuy : MonoBehaviour
{
    [SerializeField] private ShopItemSpawner spawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawner.SpawnRandoms();
    }
}
