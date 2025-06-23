using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Represents class that critters will come to to be hired and deposit shells at.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Sandcastle : MonoBehaviour
    {
        [SerializeField] private TextWithFeedback feedBackText;

        private List<Pickup> privatePickupList = new();

        [SerializeField] private float TimeUntilShopSpawn = 10f;

        [SerializeField] private ItemShop currentCritterShop;

        [SerializeField] private GameObject itemShopPrefab;

        [SerializeField] private Transform placeToPutShop;

        [SerializeField] private List<GameObject> ItemsToSell = new();

        private Coroutine newShopCo;

        [SerializeField] private CastlePickupDisplay castlePickupDisplay;

        private List<Pickup> Pickups
        {
            get => this.privatePickupList;
            set
            {
                this.feedBackText.ColourThenFade(value.Count - this.privatePickupList.Count);
                privatePickupList = value;
                castlePickupDisplay.UpdatePickupDisplay(privatePickupList);
            }
        }

        /// <summary>
        /// Must do this to trigger the setter.
        /// </summary>
        public void AddPickups(List<Pickup> picks)
        {
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
            var pickups = new List<Pickup>(this.Pickups);
            this.Pickups = new List<Pickup>();
            return pickups;
        }

        private void Start()
        {
            newShopCo = StartCoroutine(SpawnNewShop());
            castlePickupDisplay.UpdatePickupDisplay(privatePickupList);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (newShopCo != null)
            {
                StopCoroutine(newShopCo);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (newShopCo != null)
            {
                StopCoroutine(newShopCo);
            }

            newShopCo = StartCoroutine(SpawnNewShop(true));
        }

        private IEnumerator SpawnNewShop(bool initialDelay = false)
        {
            if (initialDelay)
            {
                yield return new WaitForSeconds(TimeUntilShopSpawn);
            }

            while (true)
            {
                if (currentCritterShop != null)
                {
                    yield return new WaitForSeconds(1f);
                }

                currentCritterShop = Instantiate(itemShopPrefab, this.placeToPutShop.transform).GetComponent<ItemShop>();
                this.currentCritterShop.AllItemDrops = this.ItemsToSell;
                yield return new WaitForSeconds(TimeUntilShopSpawn);
            }
        }
    }
}
