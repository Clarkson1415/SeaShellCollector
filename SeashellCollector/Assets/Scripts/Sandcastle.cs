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

        [SerializeField] private ItemShop? currentCritterShop;

        [SerializeField] private GameObject CritterItemShopPrefab;

        [SerializeField] private Transform placeToPutCritterShop;

        [SerializeField] private List<GameObject> CritterShopItemsToSell = new();

        private Coroutine? newShopCoroutine;

        [Header("Castle Shop Settings")]

        [SerializeField] private float TimeUntilSandShopSpawn = 10f;

        [SerializeField] private ItemShop? currentSandShop;

        [SerializeField] private GameObject SandShopPrefab;

        [SerializeField] private Transform placeToPutSandShop;

        [SerializeField] private List<GameObject> SandShopItems = new();

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
            newShopCoroutine = StartCoroutine(SpawnNewShop(true));
            newShopCoroutine = StartCoroutine(SpawnNewShop(false));
            castlePickupDisplay.UpdatePickupDisplay(privatePickupList);
        }

        private IEnumerator SpawnNewShop(bool spawnCritterShop, bool initialDelay = false)
        {
            var waitTime = spawnCritterShop ? TimeUntilCritterShopSpawn : TimeUntilSandShopSpawn;
            var shopToSpawn = spawnCritterShop ? CritterItemShopPrefab : SandShopPrefab;

            if (initialDelay)
            {
                yield return new WaitForSeconds(waitTime);
            }

            while (true)
            {
                if (spawnCritterShop)
                {
                    this.SpawnCritterShop();
                }
                else
                {
                    this.SpawnSandShop();
                }
                
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void SpawnCritterShop()
        {
            if (currentCritterShop != null)
            {
                return; // automation shops get destroyed anyway whenb player buys automation item.
            }

            currentCritterShop = Instantiate(CritterItemShopPrefab, this.placeToPutCritterShop.transform).GetComponent<ItemShop>();
            this.currentCritterShop.AllItemDrops = this.CritterShopItemsToSell;
            this.currentCritterShop.SandcastleSpawnedBy = this;
        }

        private void SpawnSandShop()
        {
            if (currentSandShop != null)
            {
                return; // automation shops get destroyed anyway when player buys Automation item.
            }

            currentSandShop = Instantiate(SandShopPrefab, this.placeToPutSandShop.transform).GetComponent<ItemShop>();
            this.currentSandShop.AllItemDrops = this.SandShopItems;
            this.currentSandShop.SandcastleSpawnedBy = this;
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
