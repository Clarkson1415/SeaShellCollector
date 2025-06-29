using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#nullable enable

namespace Assets.Scripts
{
    /// <summary>
    /// Represents class that critters will come to to be hired and deposit shells at.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Sandcastle : MonoBehaviour
    {
        [SerializeField] private TextWithFeedback feedBackText;

        [SerializeField] private CastlePickupDisplay castlePickupDisplay;

        private List<Pickup> privatePickupList = new();

        [Header("Critter Shop Settings")]

        [SerializeField] private float TimeUntilCritterShopSpawn = 10f;

        [SerializeField] private ItemShop currentCritterShop;

        [SerializeField] private GameObject CritterItemShopPrefab;

        [SerializeField] private Transform placeToPutCritterShop;

        [SerializeField] private List<GameObject> CritterShopItemsToSell = new();

        private Coroutine? newCritterShopCoroutine;

        [Header("Castle Shop Settings")]

        [SerializeField] private float TimeUntilSandShopSpawn = 10f;

        [SerializeField] private ItemShop currentSandShop;

        [SerializeField] private GameObject SandShopPrefab;

        [SerializeField] private Transform placeToPutSandShop;

        [SerializeField] private List<GameObject> SandShopItems = new();

        private Coroutine? newSandShopCoroutine;


        private List<Pickup> Pickups
        {
            get => this.privatePickupList;
            set
            {
                castlePickupDisplay.UpdatePickupDisplay(value);
                privatePickupList = value;
            }
        }

        /// <summary>
        /// Must do this to trigger the setter.
        /// </summary>
        public void AddPickups(List<Pickup> picks)
        {
            this.feedBackText.ColourThenFade(picks.Count);
            this.Pickups = new List<Pickup>(this.Pickups.Concat(picks));
        }

        public List<Pickup> GetCopyOfPickups()
        {
            return new List<Pickup>(this.Pickups);
        }

        /// <summary>
        /// Must do this to trigger the setter.
        /// </summary>
        public List<Pickup> TakePickups()
        {
            this.feedBackText.ColourThenFade(-this.Pickups.Count);
            var pickups = new List<Pickup>(this.Pickups);
            this.Pickups = new List<Pickup>();
            return pickups;
        }

        private void Start()
        {
            newCritterShopCoroutine = StartCoroutine(SpawnNewShop());
            castlePickupDisplay.UpdatePickupDisplay(privatePickupList);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (newCritterShopCoroutine != null)
            {
                StopCoroutine(newCritterShopCoroutine);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (newCritterShopCoroutine != null)
            {
                StopCoroutine(newCritterShopCoroutine);
            }

            newCritterShopCoroutine = StartCoroutine(SpawnNewShop(true));
        }

        private IEnumerator SpawnNewShop(bool initialDelay = false)
        {
            if (initialDelay)
            {
                yield return new WaitForSeconds(TimeUntilCritterShopSpawn);
            }

            while (true)
            {
                if (currentCritterShop != null)
                {
                    Destroy(currentCritterShop);
                }

                currentCritterShop = Instantiate(CritterItemShopPrefab, this.placeToPutCritterShop.transform).GetComponent<ItemShop>();
                this.currentCritterShop.AllItemDrops = this.CritterShopItemsToSell;
                this.currentCritterShop.SandcastleSpawnedBy = this;
                yield return new WaitForSeconds(TimeUntilCritterShopSpawn);
            }
        }

        private Cannon? cannon;

        public void AddPowerup(Cannon cannon)
        {
            this.cannon = cannon;
            StartCoroutine(ShootCannonAfterTime());
        }

        private IEnumerator ShootCannonAfterTime()
        {
            yield return new WaitForSeconds(30f);
            Debug.Log("shooting pickups to player");
            if (this.cannon == null)
            {
                Debug.LogError("Cannon is null, cannot shoot pickups to player.");
                yield break;
            }

            this.cannon.ShootToPlayer(this.TakePickups());
        }
    }
}
