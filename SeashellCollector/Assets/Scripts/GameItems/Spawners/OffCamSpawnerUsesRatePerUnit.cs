using Assets.Scripts;
using Assets.Scripts.GameItems;
using UnityEditor.PackageManager;
using UnityEngine;

public class OffCamSpawnerUsesRatePerUnit : Spawner
{
    public float spawnRatePerUnit = 2f;

    public float minDistanceFromClosestPickup = 5f;

    private BoxCollider2D spawnRegion;

    private Camera playerCam;

    [SerializeField] private float minDistanceFromSandcastles = 10f; // Minimum distance from sandcastles to spawn shops

    [SerializeField] private float minDistanceFromOtherShops = 5f; // Minimum distance from other shops to spawn new shop

    // DO NOT HAVE A Awake METHOD, the Spawner base class has one. Otherwise Spawner.Start() will not be called.
    private void Start()
    {
        spawnRegion = this.GetComponent<BoxCollider2D>();
        Debug.Log("Have difference chances of spawning items, e.g. pink shell very high. pearl exreme low, coral medium");
        playerCam = Camera.main;
    }

    protected override float GetSpawnInterval()
    {
        // TODO will need to remove thiese findobjects by type calls, as they are expensive.
        var crittersInScene = FindObjectsByType<Collector>(FindObjectsSortMode.None);
        var players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        var peopleInScene = crittersInScene.Length + players.Length;
        var spawnRate = peopleInScene * spawnRatePerUnit;
        var wait = peopleInScene == 0 ? 1f : (1f / spawnRate);
        return wait;
    }

    protected override float GetMinX()
    {
        return this.spawnRegion.bounds.min.x;
    }

    protected override float GetMaxX()
    {
        return this.spawnRegion.bounds.max.x;
    }

    protected override float GetMinY()
    {
        return this.spawnRegion.bounds.min.y;
    }

    protected override float GetMaxY()
    {
        return this.spawnRegion.bounds.max.y;
    }

    protected override bool SpawnConditionsAreMet(Vector2 spawnPosition)
    {

        var offScreen = IsSpawnOffScreen(spawnPosition);
        var inBounds = spawnRegion.bounds.Contains(spawnPosition);
        var farFromSandcastleAndShops = IsFarEnoughFromSandcastleAndShops(spawnPosition);
        var farFromLevelDecor = IsFarEnoughFromLevelDecor(spawnPosition);
        var farFromPickups = IsFarEnoughFromPickups(spawnPosition);

        Debug.Log($"Spawn at {spawnPosition}: OffScreen={offScreen}, InBounds={inBounds}, " +
          $"FarFromSandcastle={farFromSandcastleAndShops}, FarFromDecor={farFromLevelDecor}, " +
          $"FarFromPickups={farFromPickups}");

        return offScreen && inBounds && farFromSandcastleAndShops && farFromLevelDecor && farFromPickups;
    }

    private bool IsFarEnoughFromSandcastleAndShops(Vector2 spawnPosition)
    {
        var closestSandcastle = Utility.GetClosest<Sandcastle>(spawnPosition);
        var closestShop = Utility.GetClosest<ItemShop>(spawnPosition);

        // If no sandcastles exist, we're automatically far enough from them
        var farFromSandC = closestSandcastle == null || Vector3.Distance(spawnPosition, closestSandcastle.transform.position) >= minDistanceFromSandcastles;

        // If no shops exist, we're automatically far enough from them
        var farFromShop = closestShop == null || Vector3.Distance(spawnPosition, closestShop.transform.position) >= minDistanceFromOtherShops;

        return farFromSandC && farFromShop;
    }

    private bool IsSpawnOffScreen(Vector2 spawnPos)
    {
        float height = playerCam.orthographicSize * 2f;
        float width = height * playerCam.aspect;
        Vector2 centre = playerCam.transform.position;

        return spawnPos.x < centre.x - width / 2f || spawnPos.x > centre.x + width / 2f ||
            spawnPos.y < centre.y - height / 2f || spawnPos.y > centre.y + height / 2f;
    }

    [SerializeField] private float minDistanceFromLevelDecor = 10f; // Minimum distance from level decor to spawn shops

    private bool IsFarEnoughFromLevelDecor(Vector2 spawnPosition)
    {
        // Use Physics2D.OverlapCircle to check for any LevelDecor objects within the minimum distance
        // This is more efficient than finding all objects and checking distances

        // Create a layer mask for objects with "LevelDecor" tag (if using layers)
        // Or use OverlapCircle and filter by tag
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(spawnPosition, minDistanceFromLevelDecor);

        // Check if any of the nearby colliders have the "LevelDecor" tag
        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider != null && collider.gameObject.activeInHierarchy && collider.CompareTag("LevelDecor"))
            {
                // Found a level decor object within the minimum distance - too close!
                return false;
            }
        }

        // No level decor found within the minimum distance - far enough!
        return true;
    }

    private bool IsFarEnoughFromPickups(Vector2 spawnPosition)
    {
        // Use Physics2D.OverlapCircle to check for any LevelDecor objects within the minimum distance
        // This is more efficient than finding all objects and checking distances

        // Create a layer mask for objects with "LevelDecor" tag (if using layers)
        // Or use OverlapCircle and filter by tag
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(spawnPosition, minDistanceFromClosestPickup);

        // Check if any of the nearby colliders have the "LevelDecor" tag
        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider != null && collider.gameObject.activeInHierarchy && collider.GetComponent<Pickup>())
            {
                return false;
            }
        }

        return true;
    }

    protected override void DoToItemAfterSpawn(GameObject newSpawnedItem)
    {
        // TODO ? if i need.
    }
}
