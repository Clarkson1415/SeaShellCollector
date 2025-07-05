using Assets.Scripts;
using Assets.Scripts.GameItems;
using UnityEngine;
using UnityEngine.Rendering;

public class OffCamSpawnerUsesRatePerUnit : Spawner
{
    [SerializeField] private float minTimeInverval = 1f;

    [SerializeField] private float maxTimeInterval = 5f;

    public float spawnTimeDecreaseModifier = 1f;

    public float spawnTimeDecreasePerUnit = 2f;

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

    private int numPlayers = 1;

    protected override float GetSpawnInterval()
    {
        // Fast static access - no expensive FindObjectsByType calls
        var crittersInScene = Collector.CrittersInScene;
        var players = Player.PlayersInScene;
        var peopleInScene = crittersInScene + players;

        var spawnInt = Random.Range(this.minTimeInverval, this.maxTimeInterval);

        var SpawnAfterDecreaseMod = spawnInt - this.spawnTimeDecreaseModifier;
        var spawnAfterUnitDecrease = SpawnAfterDecreaseMod - this.spawnTimeDecreasePerUnit * peopleInScene;

        return spawnAfterUnitDecrease;
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
        var farFromStuff = IsFarEnoughFromStuff(spawnPosition);

        return offScreen && inBounds && farFromStuff;
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

    private bool IsFarEnoughFromStuff(Vector2 spawnPosition)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(spawnPosition, minDistanceFromLevelDecor);

        // Check if any of the nearby colliders have the "LevelDecor" tag
        foreach (Collider2D collider in nearbyColliders)
        {
            if (collider != null && collider.gameObject.activeInHierarchy && collider.CompareTag("LevelDecor"))
            {
                // Found a level decor object within the minimum distance - too close!
                return false;
            }

            if (collider != null && collider.gameObject.activeInHierarchy && collider.GetComponent<Pickup>())
            {
                return false;
            }

            if (collider != null && collider.gameObject.activeInHierarchy && collider.GetComponent<Sandcastle>())
            {
                return false;
            }

            if (collider != null && collider.gameObject.activeInHierarchy && collider.GetComponent<ItemShop>())
            {
                return false;
            }
        }

        // No level decor found within the minimum distance - far enough!
        return true;
    }

    protected override void DoToItemAfterSpawn(GameObject newSpawnedItem)
    {
        // TODO ? if i need.
    }
}
