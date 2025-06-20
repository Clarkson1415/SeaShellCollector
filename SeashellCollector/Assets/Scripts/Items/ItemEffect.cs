using NUnit.Framework.Internal.Commands;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Parent class of item effects.
    /// </summary>
    public abstract class ItemEffect : ScriptableObject
    {
        [SerializeField] protected int Value;

        [SerializeField] protected float Timeout;

        [SerializeField] protected bool HasTimeout;

        public abstract void ApplyWithoutTimeout(Player player);

        public abstract void ApplyWithTimeout(Player player);

        public void Apply(Player player)
        {
            if (this.HasTimeout)
            {
                this.ApplyWithTimeout(player);
            }
            else
            {
                this.ApplyWithoutTimeout(player);
            }
        }

        public abstract void Remove(Player player);
    }
}
