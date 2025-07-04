using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
#nullable enable

namespace Assets.Scripts.GameItems
{
    [RequireComponent(typeof(ObjectPooler))]
    public abstract class Spawner : MonoBehaviour
    {
        private ObjectPooler objectPooler;

        protected abstract float GetMinX();
        protected abstract float GetMaxX();
        protected abstract float GetMinY();
        protected abstract float GetMaxY();

        private GameObject? SpawnPooledObject(Vector3 position, Quaternion rotation)
        {
            GameObject? objFromPool = this.objectPooler.GetPooledObject();
            if (objFromPool != null)
            {
                objFromPool.transform.position = position;
                objFromPool.transform.rotation = rotation;
                objFromPool.SetActive(true);
            }
            else
            {
                MyLog.LogWarning("No available object in pool got null.");
            }

            return objFromPool;
        }

        private void Awake()
        {
            objectPooler = this.GetComponent<ObjectPooler>();
            if (objectPooler == null)
            {
                throw new NullReferenceException("No object pooler");
            }

            StartCoroutine(SpawnLoop());
        }

        private Vector2 GetNewSpawnPosition()
        {
            Vector2 spawnPosition = new(
                        UnityEngine.Random.Range(this.GetMinX(), this.GetMaxX()),
                        UnityEngine.Random.Range(this.GetMinY(), this.GetMaxY())
                    );
            return spawnPosition;
        }

        protected abstract float GetSpawnInterval();

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(GetSpawnInterval());

                Vector2 spawnPosition = GetNewSpawnPosition();

                int maxAttempts = 100; // Prevent infinite loop in case of no valid spawn location.
                int attempts = 0;

                // While outside of spawn or too close to sandcastle or shop get new position.
                while (!this.SpawnConditionsAreMet(spawnPosition))
                {
                    spawnPosition = GetNewSpawnPosition();
                    attempts++;
                    if (attempts > maxAttempts)
                    {
                        break;
                    }
                }

                if (!this.SpawnConditionsAreMet(spawnPosition))
                {
                    continue;
                }

                var newSpawnedItem = this.SpawnPooledObject(spawnPosition, Quaternion.identity);
                if (newSpawnedItem == null)
                {
                    MyLog.LogWarning($"Failed to spawn item: {this.gameObject.name} No available object in pool. Continue, wait for available.");
                    continue; // Skip to the next iteration if no object was spawned
                }

                newSpawnedItem.transform.SetParent(this.transform); // Set the parent to this object for organization
                this.DoToItemAfterSpawn(newSpawnedItem);
            }
        }

        protected abstract bool SpawnConditionsAreMet(Vector2 spawnPosition);

        protected abstract void DoToItemAfterSpawn(GameObject newSpawnedItem);
    }
}
