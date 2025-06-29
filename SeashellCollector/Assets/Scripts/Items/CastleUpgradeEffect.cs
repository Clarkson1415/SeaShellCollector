using UnityEngine;

namespace Assets.Scripts.Items
{
    public class CastleUpgradeEffect : ItemEffect
    {
        public override void ApplyWithoutTimeout(Player player)
        {
            Debug.Log("Spawning castle upgrade");
        }

        public override void Remove(Player player)
        {
            throw new System.NotImplementedException();
        }
    }
}
