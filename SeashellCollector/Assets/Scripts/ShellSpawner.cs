using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSpawner : MonoBehaviour
{
    public List<GameObject> Shells;

    public float TimeBetweenSpawns = 1f;

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
            Spawn();
            yield return new WaitForSeconds(this.TimeBetweenSpawns);
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
