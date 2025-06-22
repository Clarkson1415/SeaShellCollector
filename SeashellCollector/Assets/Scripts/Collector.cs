using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class of critter helper that can collect items and deposit at sandcastle.
    /// </summary>
    [RequireComponent(typeof(Collector))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Collector : MonoBehaviour
    {
        [SerializeField] private List<PickupType> canPickupThis; // What this guy can pick up.

        public List<Pickup> Pickups { get; set; } = new();

        [SerializeField] private int maxCap;

        private Vector3 target;

        [SerializeField] private Transform SandcastleHome; // Where to go when full to deposit. Or player can walk over the guy to collect.

        public float Speed = 1f;

        private void Awake()
        {
            PickNewPickupTarget();
            this.SandcastleHome = FindObjectsByType<Sandcastle>(FindObjectsSortMode.None).FirstOrDefault().transform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Pickup>(out var p))
            {
                if (!this.canPickupThis.Contains(p.PickupType))
                {
                    Debug.Log($"Critter did not pickup {p.name}, wrong type.");
                    return;
                }

                this.Pickups.Add(p);
                Debug.Log($"Critter pickup {p.name}");
                Destroy(p.gameObject);

                if (Pickups.Count == maxCap)
                {
                    this.SetPositionToHome();
                    return;
                }
                else
                {
                    PickNewPickupTarget();
                }
            }
            else if (collision.TryGetComponent<Sandcastle>(out var s))
            {
                if (this.Pickups.Count > 0)
                {
                    s.PickupStore = new List<Pickup>(s.PickupStore.Concat(this.Pickups)); // Must do this to trigger the setter.
                    this.Pickups.Clear();
                }
            }
        }

        /// <summary>
        /// Set position to home castle to deposit.
        /// </summary>
        public void SetPositionToHome()
        {
            this.target = SandcastleHome.position;
        }

        private void PickNewPickupTarget()
        {
            var closest = Utility.GetClosestPickup(this.transform.position, this.canPickupThis);
            target = closest.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                PickNewPickupTarget();
            }
            else if (NearPosition())
            {
                Debug.Log("new position");
                PickNewPickupTarget();
            }
            else
            {
                // Move towards the target position
                Vector3 direction = (target - this.transform.position).normalized;
                this.transform.position += direction * Time.deltaTime * Speed;
            }
        }

        private bool NearPosition()
        {
            if (this.transform.position.x < target.x + 0.2f &&
                this.transform.position.x > target.x - 0.2f &&
                this.transform.position.y < target.y + 0.2f &&
                this.transform.position.y > target.y - 0.2f)
            {
                return true;
            }

            return false;
        }
    }
}
