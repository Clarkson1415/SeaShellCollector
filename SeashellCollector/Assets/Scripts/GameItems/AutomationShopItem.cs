using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class for critter and sandcastles sold at the shop.
    /// </summary>
    public class AutomationShopItem : ShopItem
    {
        /// <summary>
        /// The Sandcastle or critter to spawn when this item is purchased.
        /// </summary>
        public GameObject PrefabToSpawn;

        /// <summary>
        /// If this is a sandcastle or critter will instantiate.
        /// </summary>
        /// <param name="player"></param>
        public override void ApplyItemEffects(Player player)
        {
            var automationItemSpawn = Instantiate(PrefabToSpawn, this.ShopBelongsTo.transform.position, Quaternion.identity);
            if (automationItemSpawn.TryGetComponent<Collector>(out var col))
            {
                var home = this.ShopBelongsTo.SandcastleSpawnedBy;
                if (home == null)
                {
                    Debug.Log($"No Home Sandcastle set on {this.ShopBelongsTo.name}");
                    home = Utility.GetClosest<Sandcastle>(this.transform.position);
                }

                col.SandcastleHome = home;
            }
        }
    }
}
