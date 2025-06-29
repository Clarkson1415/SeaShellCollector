using System;
using UnityEngine;
#nullable enable

namespace Assets.Scripts.GameItems
{
    public class SandcastleShopItem : AutomationShopItem
    {
        public override void ApplyItemEffects(Player player)
        {
            if (this.ShopBelongsTo == null)
            {
                throw new ArgumentNullException("Shop belong to cannot be null when spawning in item. destroy after.");
            }

            var sandcastle = this.ShopBelongsTo.SandcastleSpawnedBy;
            var position = this.ShopBelongsTo.transform.position;

            // If is sandcastle we will replace it upgrade it.
            if (sandcastle != null)
            {
                Debug.Log("Upgrade sandcastle todo animation of dust");
                position = sandcastle.gameObject.transform.position;
                sandcastle.StopAllCoroutines();
                Destroy(sandcastle.gameObject);
            }
            
            var automationItemSpawn = Instantiate(PrefabToSpawn, position, Quaternion.identity);
        }
    }
}
