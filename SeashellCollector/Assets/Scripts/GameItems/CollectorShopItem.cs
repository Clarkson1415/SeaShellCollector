using System;
using UnityEngine;
#nullable enable

namespace Assets.Scripts.GameItems
{
    internal class CollectorShopItem : AutomationShopItem
    {
        public override void ApplyItemEffects(Player player)
        {
            if (this.ShopBelongsTo == null)
            {
                throw new ArgumentNullException("Shop belong to cannot be null when spawning in item. destroy after.");
            }

            var automationItemSpawn = Instantiate(PrefabToSpawn, this.ShopBelongsTo.transform.position, Quaternion.identity);

            // if collector:
            if (!automationItemSpawn.TryGetComponent<Collector>(out var col))
            {
                throw new ArgumentException("PrefabToSpawn must have Collector component attached to it.");
            }

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
