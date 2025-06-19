using NUnit.Framework.Internal.Commands;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Parent class of item effects.
    /// </summary>
    public abstract class ItemEffect : ScriptableObject
    {
        public abstract void Apply(Player player, int value);

        public abstract void Apply(Player player, int value, float timeout);

        public abstract void Remove(Player player, int value);
    }
}
