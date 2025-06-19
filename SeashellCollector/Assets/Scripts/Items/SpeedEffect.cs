using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Speed Effect", menuName = "Item Effects/Speed")]
    public class SpeedEffect : ItemEffect
    {
        public override void Apply(Player player, int value)
        {
            player.ModifySpeed(value);
        }

        public override void Apply(Player player, int value, float timeout)
        {
            player.ModifySpeed(value, timeout);
        }

        public override void Remove(Player player, int value)
        {
            player.ModifySpeed(-value);
        }
    }
}
