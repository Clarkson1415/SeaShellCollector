using Assets.Scripts.GameItems;
using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn prefabs at random locations see wind spawner in scene. Spawns within camera then deactivates after some time.
/// </summary>
public class OnCamSpawnerUsesInterval : Spawner
{
    [SerializeField] protected float TimeUntilDeactivateAndReturnToPool = float.MaxValue; // For wind spawner this is 20.

    [SerializeField] private float spawnIntervalMin = 0f; // Time between spawns

    [SerializeField] private float spawnIntervalMax = 5f; // Time between spawns

    private Camera playerCam;

    private void Start()
    {
        Debug.Log("Have difference chances of spawning items, e.g. pink shell very high. pearl exreme low, coral medium");
        playerCam = Camera.main;
    }

    IEnumerator DeactivateAfterTime(GameObject obj)
    {
        yield return new WaitForSeconds(TimeUntilDeactivateAndReturnToPool);
        obj.SetActive(false);
    }

    protected override float GetSpawnInterval()
    {
        return UnityEngine.Random.Range(spawnIntervalMin, spawnIntervalMax);
    }

    protected override float GetMinX()
    {
        float height = playerCam.orthographicSize * 2f;
        float width = height * playerCam.aspect;
        Vector2 centre = playerCam.transform.position;
        var minX = centre.x - width / 2f;
        return minX;
    }

    protected override float GetMaxX()
    {
        float height = playerCam.orthographicSize * 2f;
        float width = height * playerCam.aspect;
        Vector2 centre = playerCam.transform.position;
        var maxX = centre.x + width / 2f;
        return maxX;
    }

    protected override float GetMinY()
    {
        float height = playerCam.orthographicSize * 2f;
        float width = height * playerCam.aspect;
        Vector2 centre = playerCam.transform.position;
        var minY = centre.y - height / 2f;
        return minY;
    }

    protected override float GetMaxY()
    {
        float height = playerCam.orthographicSize * 2f;
        float width = height * playerCam.aspect;
        Vector2 centre = playerCam.transform.position;
        var maxY = centre.y + height / 2f;
        return maxY;
    }

    protected override bool SpawnConditionsAreMet(Vector2 spawnPosition)
    {
        return true; // base already checks if spawn position is within min/max bounds
    }

    protected override void DoToItemAfterSpawn(GameObject newSpawnedItem)
    {
        this.StartCoroutine(DeactivateAfterTime(newSpawnedItem));
    }
}
