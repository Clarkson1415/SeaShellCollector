using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public List<GameObject> Shells;

    public float spawnRatePerCritterInScene = 2f; // Shells spawned per second per Collector in scene + player.

    private BoxCollider2D bounds;

    private void Awake()
    {
        bounds = this.GetComponent<BoxCollider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLoop());
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

        var shell = Instantiate(Shells[index], this.transform);
        shell.transform.position = new Vector3(posX, posY);
    }
}
