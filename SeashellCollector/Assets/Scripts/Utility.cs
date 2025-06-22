using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Utility : MonoBehaviour
    {
        public static float CalculateDistanceBetween(Vector3 point2, Vector3 point1)
        {
            return Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2));
        }

        public static T GetClosest<T>(Vector3 position) where T : MonoBehaviour
        {
            var objects = FindObjectsByType<T>(FindObjectsSortMode.None);

            if (objects.Length == 0)
                return null;

            T closest = null;
            float closestDist = float.MaxValue;

            foreach (var obj in objects)
            {
                float dist = Vector3.Distance(position, obj.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = obj;
                }
            }

            return closest;
        }

        public static Pickup GetClosestPickup(Vector3 position, List<PickupType> types)
        {
            var objects = FindObjectsByType<Pickup>(FindObjectsSortMode.None).Where(x => types.Contains(x.PickupType));

            if (objects.Count() == 0)
                return null;

            Pickup closest = null;
            float closestDist = float.MaxValue;

            foreach (var obj in objects)
            {
                float dist = Vector3.Distance(position, obj.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = obj;
                }
            }

            return closest;
        }
    }
}
