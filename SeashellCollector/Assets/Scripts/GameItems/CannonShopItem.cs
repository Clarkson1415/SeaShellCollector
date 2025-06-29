using System;
using UnityEngine;
namespace Assets.Scripts.GameItems
{
    public class CannonShopItem : AutomationShopItem
    {
        public override void ApplyItemEffects(Player player)
        {
            if (this.ShopBelongsTo == null)
            {
                throw new ArgumentNullException("Shop belong to cannot be null when spawning in item. destroy after.");
            }

            var automationItemSpawn = Instantiate(PrefabToSpawn, this.ShopBelongsTo.transform.position, Quaternion.identity);

            if (this.ShopBelongsTo.SandcastleSpawnedBy == null)
            {
                throw new ArgumentNullException("shop null");
            }

            this.ShopBelongsTo.SandcastleSpawnedBy.AddPowerup(automationItemSpawn.GetComponent<Cannon>());
        }
    }
}
