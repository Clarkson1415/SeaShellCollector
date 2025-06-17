using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Parent class of item effects.
    /// </summary>
    public abstract class ItemEffect : ScriptableObject
    {
        /// <summary>
        /// The amount that the effect will change something.
        /// </summary>:
        public int Value { get; set; }

        /// <summary>
        /// Apply effect to the player.
        /// </summary>
        public abstract void Apply(Player player);
    }
}
