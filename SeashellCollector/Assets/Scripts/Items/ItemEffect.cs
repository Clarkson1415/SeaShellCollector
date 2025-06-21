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

        public abstract void ApplyWithoutTimeout(Player player);

        public void Apply(Player player)
        {
            this.ApplyWithoutTimeout(player);
        }

        public abstract void Remove(Player player);
    }
}
