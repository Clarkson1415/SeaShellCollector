using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private float timeUntilNewCritterShopSpawns = 10f;

        [SerializeField] private ItemShop currentShop;

        [SerializeField] private GameObject itemShopPrefab;

        [SerializeField] private Transform placeToPutShop;

        [SerializeField] private List<GameObject> ItemsToSell = new();

        private Coroutine newShopCo;

        public List<Pickup> PickupStore
        {
            get => this.privatePickupList;
            set
            {
                this.feedBackText.ColourThenFade(value.Count);
                privatePickupList.AddRange(value);
            }
        }

        private void Start()
        {
            newShopCo = StartCoroutine(SpawnNewShop());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            StopCoroutine(newShopCo);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            newShopCo = StartCoroutine(SpawnNewShop(true));
        }

        private IEnumerator SpawnNewShop(bool initialDelay = false)
        {
            if (initialDelay)
            {
                yield return new WaitForSeconds(timeUntilNewCritterShopSpawns);
            }

            while (true)
            {
                if (currentShop != null)
                { 
                    Destroy(currentShop.gameObject);
                }

                currentShop = Instantiate(itemShopPrefab, this.placeToPutShop.transform).GetComponent<ItemShop>();
                this.currentShop.AllItemDrops = this.ItemsToSell;
                yield return new WaitForSeconds(timeUntilNewCritterShopSpawns);
            }
        }
    }
}
