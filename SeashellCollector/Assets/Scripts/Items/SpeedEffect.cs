using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Speed Effect", menuName = "Item Effects/Speed")]
    public class SpeedEffect : ItemEffect
    {
        /// <summary>
        /// Increase speed by value percent.
        /// </summary>
        /// <param name="player"></param>
        public override void ApplyWithoutTimeout(Player player)
        {
            player.ModifySpeed(this.Value);
        }

        public override void Remove(Player player)
        {
            player.ModifySpeed(-this.Value);
        }
    }
}
