using System;
using UnityEngine;
#nullable enable

namespace Assets.Scripts
{
    /// <summary>
    /// Class for critter and sandcastles sold at the shop.
    /// </summary>
    public abstract class AutomationShopItem : ShopItem
    {
        /// <summary>
        /// The Sandcastle or critter to spawn when this item is purchased.
        /// </summary>
        public GameObject PrefabToSpawn;
    }
}
