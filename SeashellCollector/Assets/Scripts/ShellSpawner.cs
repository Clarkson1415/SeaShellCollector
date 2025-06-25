using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public List<GameObject> Shells;

    public float spawnRatePerCritterInScene = 2f; // Shells spawned per second per Collector in scene + player.

    public float minDistanceFromClosestShell = 5f;

    private BoxCollider2D bounds;

    private void Awake()
    {
        bounds = this.GetComponent<BoxCollider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLoop());
        Debug.Log("Have difference chances of spawning items, e.g. pink shell very high. pearl exreme low, coral medium");
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            var crittersInScene = FindObjectsByType<Collector>(FindObjectsSortMode.None);
            var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
            var peopleInScene = crittersInScene.Length + players.Length;
            for (int i = 0; i < peopleInScene; i++)
            {
                Spawn();
                var wait = peopleInScene == 0 ? 1f : (1f / peopleInScene);
                // spawn each shell over an even spacing of 1 sec.
                yield return new WaitForSeconds(wait);
            }
        }
    }

    private void Spawn()
    {
        var posX = Random.Range(bounds.bounds.min.x, bounds.bounds.max.x);
        var posY = Random.Range(bounds.bounds.min.y, bounds.bounds.max.y);

        var index = Random.Range(0, Shells.Count);

        // Don't spawn on top of player.
        var player = FindFirstObjectByType<Player>();
        while (player.GetComponent<BoxCollider2D>().OverlapPoint(new Vector2(posX, posY)) || TooCloseToShell(new Vector2(posX, posY)))
        {
            posX = Random.Range(bounds.bounds.min.x, bounds.bounds.max.x);
            posY = Random.Range(bounds.bounds.min.y, bounds.bounds.max.y);
        }

        var shell = Instantiate(Shells[index], this.transform);
        shell.transform.position = new Vector3(posX, posY);
    }

    /// <summary>
    /// Returns true if the shell is too close to a pink shell pickup.
    /// </summary>
    /// <returns></returns>
    private bool TooCloseToShell(Vector2 possibleSpawn)
    {
        var nearestShell = Utility.GetClosestPickup(possibleSpawn, new List<PickupType>() {PickupType.PinkShell});
        return nearestShell != null && Vector2.Distance(possibleSpawn, nearestShell.transform.position) < minDistanceFromClosestShell;
    }
}
