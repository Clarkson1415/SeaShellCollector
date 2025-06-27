using Assets.Scripts.GameItems;
using System.Collections;
using UnityEngine;

/// <summary>
/// Spawn prefabs at random locations see wind spawner in scene. Spawns within camera.
/// </summary>
public class WindSpawner : Spawner
{
    [SerializeField] protected float TimeUntilDeactivateAndReturnToPool = float.MaxValue; // For wind spawner this is 20.

    IEnumerator DeactivateAfterTime(GameObject obj)
    {
        yield return new WaitForSeconds(TimeUntilDeactivateAndReturnToPool);
        obj.SetActive(false);
    }

    protected override float GetMinX()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        Vector2 centre = Camera.main.transform.position;
        var minX = centre.x - width / 2f;
        return minX;
    }

    protected override float GetMaxX()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        Vector2 centre = Camera.main.transform.position;
        var maxX = centre.x + width / 2f;
        return maxX;
    }

    protected override float GetMinY()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        Vector2 centre = Camera.main.transform.position;
        var minY = centre.y - height / 2f;
        return minY;
    }

    protected override float GetMaxY()
    {
        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;
        Vector2 centre = Camera.main.transform.position;
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
