using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Speed Effect", menuName = "Item Effects/Speed")]
    public class SpeedEffect : ItemEffect
    {
        public override void ApplyWithoutTimeout(Player player)
        {
            player.ModifySpeed(this.Value);
        }

        public override void ApplyWithTimeout(Player player)
        {
            player.ModifySpeed(this.Value, this.Timeout);
        }

        public override void Remove(Player player)
        {
            player.ModifySpeed(-this.Value);
        }
    }
}
