using Assets.Scripts;
using Assets.Scripts.GameItems;
using System.Collections.Generic;
using UnityEngine;

public class OffCamPickupSpawner : Spawner
{
    public float spawnRatePerCritterInScene = 2f; // Shells spawned per second per Collector in scene + player.

    public float minDistanceFromClosestShell = 5f;

    private BoxCollider2D bounds;

    private Camera playerCam;

    [SerializeField] private PickupType thisPickupType;

    // DO NOT HAVE A START METHOD, the Spawner base class has one. Otherwise Spawner.Start() will not be called.
    private void Awake()
    {
        bounds = this.GetComponent<BoxCollider2D>();
        Debug.Log("Have difference chances of spawning items, e.g. pink shell very high. pearl exreme low, coral medium");
        playerCam = Camera.main;
    }

    protected override float GetSpawnInterval()
    {
        // TODO will need to remove thiese findobjects by type calls, as they are expensive.
        var crittersInScene = FindObjectsByType<Collector>(FindObjectsSortMode.None);
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        var peopleInScene = crittersInScene.Length + players.Length;
        var spawnRate = peopleInScene * spawnRatePerCritterInScene;
        var wait = peopleInScene == 0 ? 1f : (1f / spawnRate);
        return wait;
    }

    /// <summary>
    /// Returns true if the shell is too close to a pink shell pickup.
    /// </summary>
    /// <returns></returns>
    private bool TooCloseToShell(Vector2 possibleSpawn)
    {
        // TODO will need to remove this getclosetst findobjectsbytype calls, as they are expensive.
        var nearestPickup = Utility.GetClosestPickup(possibleSpawn, new List<PickupType>() { thisPickupType });
        return nearestPickup != null && Vector2.Distance(possibleSpawn, nearestPickup.transform.position) < minDistanceFromClosestShell;
    }

    protected override float GetMinX()
    {
        return this.bounds.bounds.min.x;
    }

    protected override float GetMaxX()
    {
        return this.bounds.bounds.max.x;
    }

    protected override float GetMinY()
    {
        return this.bounds.bounds.min.y;
    }

    protected override float GetMaxY()
    {
        return this.bounds.bounds.max.y;
    }

    protected override bool SpawnConditionsAreMet(Vector2 spawnPosition)
    {
        return IsSpawnOffScreen(spawnPosition) && !TooCloseToShell(spawnPosition) && bounds.bounds.Contains(spawnPosition);
    }

    private bool IsSpawnOffScreen(Vector2 spawnPos)
    {
        float height = playerCam.orthographicSize * 2f;
        float width = height * playerCam.aspect;
        Vector2 centre = playerCam.transform.position;

        return spawnPos.x < centre.x - width / 2f || spawnPos.x > centre.x + width / 2f ||
               spawnPos.y < centre.y - height / 2f || spawnPos.y > centre.y + height / 2f;
    }

    protected override void DoToItemAfterSpawn(GameObject newSpawnedItem)
    {
        // TODO ? if i need.
    }
}
