using System.Collections.Generic;
using UnityEngine;
#nullable enable

namespace Assets.Scripts.GameItems
{
    public class ObjectPooler : MonoBehaviour
    {
        private List<GameObject> pooledObjects;
        [SerializeField] private GameObject objectToPool;
        [SerializeField] private int amountToPool;

        void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, this.transform);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }

        public GameObject? GetPooledObject()
        {
            for (int i = 0; i < amountToPool; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }
            return null;
        }
    }
}
