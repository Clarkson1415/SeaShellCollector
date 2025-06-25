using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawn prefabs at random locations see wind spawner in scene. Spawns within camera.
/// </summary>
public class SpawnPrefabs : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs; // The prefab to spawn
    [SerializeField] private float spawnIntervalMin = 0f; // Time between spawns
    [SerializeField] private float spawnIntervalMax = 5f; // Time between spawns
    [SerializeField] private float TimeUntilDelete = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            float height = Camera.main.orthographicSize * 2f;
            float width = height * Camera.main.aspect;
            Vector2 centre = Camera.main.transform.position;
            var minX = centre.x - width / 2f;
            var maxX = centre.x + width / 2f;
            var minY = centre.y - height / 2f;
            var maxY = centre.y + height / 2f;

            // Choose a random prefab from the list
            GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
            // Instantiate the prefab at a random position within the bounds of the scene
            // + 5 minus 5 to avoid spawning too close to the edges
            Vector3 randomPosition = new Vector3(Random.Range(minX + 5, maxX - 5), Random.Range(minY + 5, maxY - 5), 0f);
            var wind = Instantiate(prefab, randomPosition, Quaternion.identity);
            wind.transform.SetParent(this.transform); // Set the parent to this object for organization
            StartCoroutine(DeleteAfterTime(wind));
        }
    }

    IEnumerator DeleteAfterTime(GameObject obj)
    {
        yield return new WaitForSeconds(TimeUntilDelete);
        Destroy(obj);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
