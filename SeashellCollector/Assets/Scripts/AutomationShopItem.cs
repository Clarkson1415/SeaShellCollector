using TMPro;
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
            Instantiate(PrefabToSpawn, this.ShopBelongsTo.transform.position, Quaternion.identity);
        }
    }
}
