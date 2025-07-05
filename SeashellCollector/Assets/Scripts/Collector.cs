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
        // Static collection to track all active collectors
        private static HashSet<Collector> AllCollectors = new HashSet<Collector>();

        [SerializeField] private float distanceToCheckForPickups = 50f;

        public static int CrittersInScene => AllCollectors.Count;

        private void OnEnable()
        {
            AllCollectors.Add(this);
        }

        private void OnDisable()
        {
            AllCollectors.Remove(this);
        }

        private void OnDestroy()
        {
            AllCollectors.Remove(this);
        }

        [SerializeField] private List<PickupType> canPickupThis; // What this guy can pick up.
        public List<Pickup> Pickups { get; set; } = new();

        [SerializeField] private int maxCap;

        private Vector3 target;

        public Sandcastle SandcastleHome; // Where to go when full to deposit. Or player can walk over the guy to collect.

        public float Speed = 1f;

        private void Awake()
        {
            PickNewPickupTarget();
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

                if (Pickups.Count + 1 > maxCap)
                {
                    this.SetPositionToHome();
                    return;
                }

                this.Pickups.Add(p);
                Debug.Log($"Critter pickup {p.name}");
                Destroy(p.gameObject);
                PickNewPickupTarget();
            }
            else if (collision.TryGetComponent<Sandcastle>(out var sandy))
            {
                if (this.Pickups.Count > 0)
                {
                    sandy.AddPickups(this.Pickups);
                    this.Pickups.Clear();
                }
            }
        }

        /// <summary>
        /// Set position to home castle to deposit.
        /// </summary>
        public void SetPositionToHome()
        {
            if (SandcastleHome == null)
            {
                Debug.Log($"Critter {this.name} has no home.");
                return;
            }

            this.target = SandcastleHome.transform.position;
        }

        private void PickNewPickupTarget()
        {
            var closest = Utility.GetClosestPickupWithinRange(this.transform.position, this.canPickupThis, distanceToCheckForPickups);
            if (closest == null)
            {
                this.SetPositionToHome();
                return;
            }

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
                if (direction.x > 0 && this.transform.localScale.x < 0)
                {
                    Flip();
                }
                else if (direction.x < 0 && this.transform.localScale.x > 0)
                {
                    Flip();
                }
            }
        }

        private void Flip()
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
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
