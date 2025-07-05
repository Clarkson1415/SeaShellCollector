using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ChanceEffect : ItemEffect
    {
        [SerializeField] private PickupType pickupTypeToChangeChanceFor;

        /// <summary>
        /// Increase speed by value percent.
        /// </summary>
        /// <param name="player"></param>
        public override void ApplyWithoutTimeout(Player player)
        {
            var spawners = FindObjectsByType<OffCamSpawnerUsesRatePerUnit>(FindObjectsSortMode.None);
            var spawn = spawners.FirstOrDefault(x => x.PickupTypeSpawned == this.pickupTypeToChangeChanceFor);
            cachedSpawner = spawn;
            cachedSpawner.spawnTimeDecreaseModifier += (this.Value / 100);
        }

        private OffCamSpawnerUsesRatePerUnit cachedSpawner;

        public override void Remove(Player player)
        {
            this.cachedSpawner.spawnTimeDecreaseModifier -= (this.Value / 100);
        }
    }
}
