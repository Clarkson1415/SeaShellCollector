using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Capacity Effect", menuName = "Item Effects/Capacity")]
    public class CapacityEffect : ItemEffect
    {

        public override void ApplyWithoutTimeout(Player player)
        {
            player.ModifyMaxCap(this.Value);
        }

        public override void Remove(Player player)
        {
            player.ModifyMaxCap(-this.Value);
        }
    }
}
